# Uptime Resources

> Strongly-typed creation and management of LogicMonitor **LM Uptime** ping and web checks.

## Background: LM Uptime is the new "Websites"

Historically, LogicMonitor synthetic checks (ping and web) were modelled as **Websites** and lived under
their own `/website/websites` API. That surface is still available in this library under the
`LogicMonitor.Api.Websites` namespace (`Website`, `WebsiteCreationDto`, `PingCheck`, `WebCheck`, …) and is
**unchanged** — existing integrations that depend on it keep working.

LogicMonitor has since replaced that product with **LM Uptime**, in which a check is no longer a "website"
but a first-class **Resource** (a "device") that happens to be of an Uptime type. Everything is created and
queried through the ordinary `device/devices` endpoint, with the check configuration carried in the
resource's **custom properties**.

The `LogicMonitor.Api.Resources.Uptime` namespace is this library's strongly-typed front-end for that new
model. It is intentionally **decoupled** from the legacy `Websites` namespace: it owns its own value types
and shares only neutral primitives (`Level`, `ResourceType`, `EntityProperty`). Use `Websites` for the old
product; use `Uptime` for LM Uptime.

### Reference documentation

- [Adding Uptime Devices](https://www.logicmonitor.com/support/adding-uptime-devices) — the REST API v3 contract for creating internal/external ping and web Uptime devices.
- [Internal Ping Check using LM Uptime](https://www.logicmonitor.com/support/internal-ping-check-using-lm-uptime) — the UI/field semantics (packet count, polling interval, alert tuning, checkpoints).

Both pages are also linked from the XML docs on `UptimeResource`.

## The four check types

LM Uptime has two dimensions: **ping vs web**, and **internal vs external**.

| | Internal (run by a Collector) | External (run by Site Monitors) |
|-----------|-------------------------------|---------------------------------|
| **Ping**  | `PingCheckResource` (`IsInternal = true`) | `PingCheckResource` (`IsInternal = false`) |
| **Web**   | `WebCheckResource` (`IsInternal = true`)  | `WebCheckResource` (`IsInternal = false`)  |

Both resource types derive from the abstract `UptimeResource`, and each has a matching creation DTO:

```
UptimeResource (abstract)          UptimeResourceCreationDto<T> (abstract)
├── PingCheckResource              ├── PingCheckResourceCreationDto
└── WebCheckResource               └── WebCheckResourceCreationDto
```

`UptimeResource` derives from `IdentifiedItem` and implements `IHasEndpoint` (`Endpoint() => "device/devices"`),
so it works directly with the existing generic client methods — there are **no new client methods** to learn.

## Usage

### Create an internal ping check

```csharp
using LogicMonitor.Api;
using LogicMonitor.Api.Resources.Uptime;

var creationDto = new PingCheckResourceCreationDto
{
    Name = "dns-google-ping",          // required
    HostName = "8.8.8.8",              // required
    DisplayName = "Google DNS (ping)",
    Description = "Reachability of 8.8.8.8",
    ResourceGroupIds = "1",
    IsInternal = true,
    PreferredCollectorId = 42,         // a real Collector
    SyntheticsCollectorIds = [42],
    PollingIntervalMinutes = 5,        // 1..10
    PacketCount = 5,                   // 1..50
    TimeoutMs = 500,
    PercentPacketsNotReceivedInTime = 80,
    TestLocation = new UptimeTestLocation { All = false, CollectorIds = [42] },
    Alerting = new UptimeAlertSettings
    {
        OverallAlertLevel = Level.Critical,
        IndividualAlertLevel = Level.Warning,
        IndividualCheckpointAlertsEnabled = true,
        FailedCheckCountBeforeAlerting = 1,                  // "transition"
        AlertCondition = SiteMonitorAlertCondition.AllLocations
    }
};

PingCheckResource ping = await client.CreateAsync(creationDto, cancellationToken);
```

### Create an external web check

External checks have no Collector; they run from **Site Monitor Groups** selected via `SmgIds`:

```csharp
var creationDto = new WebCheckResourceCreationDto
{
    Name = "acme-home-web",
    HostName = "www.acme.com",
    Domain = "www.acme.com",
    Scheme = UptimeHttpScheme.Https,
    IgnoreSsl = false,
    IsInternal = false,
    PollingIntervalMinutes = 5,
    PageLoadAlertTimeMs = 30000,
    // 2 = US-Washington DC, 3 = US-Oregon, 4 = Europe-Dublin, 5 = Asia-Singapore, 6 = Australia-Sydney
    TestLocation = new UptimeTestLocation { SmgIds = [2, 4] },
    Steps =
    [
        new UptimeWebCheckStep { Name = "__step0", Url = "/", HttpMethod = "GET", StatusCode = "200" }
    ]
};

WebCheckResource web = await client.CreateAsync(creationDto, cancellationToken);
```

### Fetch, update and delete

Once created, an Uptime resource behaves like any other identified resource:

```csharp
// Fetch (strongly typed)
var fetched = await client.GetAsync<PingCheckResource>(ping.Id, cancellationToken);
Console.WriteLine($"{fetched.HostName} every {fetched.PollingIntervalMinutes} min, {fetched.PacketCount} packets");

// Update
fetched.Description = "Updated description";
await client.PutAsync(fetched, cancellationToken);

// Delete
await client.DeleteAsync(fetched, cancellationToken);
```

### Required vs optional

On the creation DTOs, `Name` and `HostName` are declared `required` — the compiler enforces that you set
them. Everything else has a sensible default (polling 5 min, 5 packets, 500 ms timeout, 80% threshold, warn
alert levels). The `required` keyword is deliberately **not** applied to the resource POCOs, because those are
materialised by the deserializer via a parameterless constructor.

## How it works under the hood

This is where it gets nerdy. An Uptime check is not a bespoke entity — it is a **Resource** with:

1. a **`deviceType`** discriminator — `19` for ping, `18` for web (the `ResourceType.Ping` / `ResourceType.Web`
   enum values);
2. a set of **custom properties** that carry the configuration; and
3. for both ping and web, a single custom property — **`website.private.serviceParameters`** — whose value is
   an *escaped JSON document* (JSON-in-JSON) holding the check's tuning.

A freshly created internal ping check, on the wire, looks roughly like this:

```jsonc
{
  "name": "dns-google-ping",
  "displayName": "Google DNS (ping)",
  "deviceType": 19,
  "preferredCollectorId": 42,
  "syntheticsCollectorIds": [42],
  "testLocation": { "all": false, "collectorIds": [42], "smgIds": [] },
  "customProperties": [
    { "name": "system.categories",                "value": "pingcheckdevice" },
    { "name": "uptime.hostname",                   "value": "8.8.8.8" },
    { "name": "uptime.pollingInterval",            "value": "5" },
    { "name": "uptime.usedefaultalertsetting",     "value": "false" },
    { "name": "uptime.usedefaultlocationsetting",  "value": "false" },
    { "name": "website.private.serviceParameters", "value": "{\"percentPktsNotReceiveInTime\":\"80\",\"dns\":\"8.8.8.8\",\"count\":\"5\", … }" }
  ]
}
```

Web checks use the **same** mechanism, but with `system.categories = webcheckdevice`, `uptime.url` instead of
`uptime.hostname`, and a `serviceParameters` blob that carries the schema/domain/SSL settings plus each step
as a nested `__step0`, `__step1`, … JSON-string.

### The conversion layer

Hand-building that structure (and parsing it back) is exactly the tedium this namespace removes. The mapping
lives behind two Newtonsoft `JsonConverter`s registered via `[JsonConverter(...)]` attributes:

- **`UptimeResourceJsonConverter`** — read (GET responses) and write (PUT bodies) for `UptimeResource`.
- **`UptimeResourceCreationDtoJsonConverter`** — write-only, for the POST body.

Both delegate to a single internal **`UptimeResourceWireMapper`**, which is the one and only place the wire
format is defined. On write it flattens the typed surface into the device JSON (including serialising the
`serviceParameters` blob); on read it pulls the fields back out, preferring the `serviceParameters` blob and
falling back to the top-level fields LogicMonitor returns on a GET.

Using a class-level converter has a useful side effect: it bypasses the library's strict
`MissingMemberHandling.Error` deserialization, so an Uptime resource read ignores the (large) remainder of the
device payload instead of failing on unmapped fields.

### Internal vs external collectors

Internal checks reference a real **Collector** (`PreferredCollectorId` / `SyntheticsCollectorIds`). External
checks run from LogicMonitor's **Site Monitors**, which appear as negative "pseudo-collector" ids derived from
the Site Monitor Group ids you set in `TestLocation.SmgIds`:

| `SmgId` | Site Monitor | Collector id |
|--------:|--------------|-------------:|
| 2 | US – Washington DC | -7  |
| 3 | US – Oregon        | -8  |
| 4 | Europe – Dublin    | -9  |
| 5 | Asia – Singapore   | -10 |
| 6 | Australia – Sydney | -11 |

The mapper performs this translation for you (`collectorId = -(smgId + 5)`) when `IsInternal = false`, so you
only ever specify Site Monitor Group ids. If your portal's Site Monitor ids differ, that single mapping is
centralised in `UptimeResourceWireMapper.SmgIdToSiteMonitorCollectorId`.

## Field reference (and documented ranges)

| Typed property | Wire field | Notes |
|----------------|-----------|-------|
| `PollingIntervalMinutes` | `uptime.pollingInterval` / `pollingInterval` | 1–10, default 5 |
| `Alerting.FailedCheckCountBeforeAlerting` | `transition` | 1–10, 30, 60; default 1 |
| `Alerting.AlertCondition` | `globalSmAlertCond` | 0 All / 1 Half / 2 >1 / 3 Any |
| `Alerting.OverallAlertLevel`, `IndividualAlertLevel` | `overallAlertLevel`, `individualAlertLevel` | `warn` / `error` / `critical` |
| `PacketCount` (ping) | `count` | 1–50, default 5 |
| `TimeoutMs` (ping) | `timeoutInMSPktsNotReceive` | per-packet timeout |
| `PercentPacketsNotReceivedInTime` (ping) | `percentPktsNotReceiveInTime` | loss threshold |
| `Scheme`, `Domain`, `IgnoreSsl`, `PageLoadAlertTimeMs` (web) | `schema`, `domain`, `ignoreSSL`, `pageLoadAlertTimeInMS` | |
| `Steps` (web) | `__step0`, `__step1`, … | each a nested JSON-string |

## See also

- `LogicMonitor.Api.Resources.Uptime` — the public types described here.
- `LogicMonitor.Api.Websites` — the legacy Website surface (still supported, unrelated).
