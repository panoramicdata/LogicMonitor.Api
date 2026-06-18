using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.UptimeResources;

/// <summary>
/// Portal-free tests that exercise the Uptime wire mapping (no network/credentials required).
/// </summary>
public class UptimeResourceSerializationTests
{
	[Fact]
	public void PingCreationDto_SerializesToDeviceWireShape()
	{
		var dto = new PingCheckResourceCreationDto
		{
			Name = "ping-1",
			Description = "desc",
			ResourceGroupIds = "1",
			PreferredCollectorId = 7,
			HostName = "8.8.8.8",
			PollingIntervalMinutes = 5,
			PacketCount = 5,
			TimeoutMs = 500,
			PercentPacketsNotReceivedInTime = 80,
			SyntheticsCollectorIds = [7],
			TestLocation = new UptimeTestLocation { All = false, CollectorIds = [7], SmgIds = [] },
			Alerting = new UptimeAlertSettings
			{
				OverallAlertLevel = Level.Critical,
				IndividualAlertLevel = Level.Warning,
				IndividualCheckpointAlertsEnabled = true,
				FailedCheckCountBeforeAlerting = 1,
				AlertCondition = SiteMonitorAlertCondition.AllLocations
			}
		};

		var json = JObject.Parse(JsonConvert.SerializeObject(dto));

		json["deviceType"]!.Value<int>().Should().Be(19);
		json["displayName"]!.Value<string>().Should().Be("ping-1"); // defaulted from Name
		json["hostGroupIds"]!.Value<string>().Should().Be("1");
		json["testLocation"]!["all"]!.Value<bool>().Should().BeFalse();

		var customProperties = (JArray)json["customProperties"]!;
		string PropValue(string name) => customProperties.First(p => p["name"]!.Value<string>() == name)["value"]!.Value<string>()!;

		PropValue("system.categories").Should().Be("pingcheckdevice");
		PropValue("uptime.hostname").Should().Be("8.8.8.8");
		PropValue("uptime.pollingInterval").Should().Be("5");

		var serviceParameters = JObject.Parse(PropValue("website.private.serviceParameters"));
		serviceParameters["count"]!.Value<string>().Should().Be("5");
		serviceParameters["timeoutInMSPktsNotReceive"]!.Value<string>().Should().Be("500");
		serviceParameters["percentPktsNotReceiveInTime"]!.Value<string>().Should().Be("80");
		serviceParameters["overallAlertLevel"]!.Value<string>().Should().Be("critical");
		serviceParameters["individualAlertLevel"]!.Value<string>().Should().Be("warn");
		serviceParameters["isInternal"]!.Value<string>().Should().Be("true");
		serviceParameters["globalSmAlertCond"]!.Value<string>().Should().Be("0");
	}

	[Fact]
	public void PingResource_RoundTripsFromDeviceResponse()
	{
		var serviceParameters = new JObject
		{
			["count"] = "10",
			["timeoutInMSPktsNotReceive"] = "750",
			["percentPktsNotReceiveInTime"] = "90",
			["overallAlertLevel"] = "error",
			["individualAlertLevel"] = "warn",
			["individualSmAlertEnable"] = "true",
			["transition"] = "3",
			["globalSmAlertCond"] = "1",
			["isInternal"] = "true",
			["dns"] = "1.1.1.1"
		}.ToString(Formatting.None);

		var device = new JObject
		{
			["id"] = 42,
			["name"] = "ping-42",
			["displayName"] = "ping-42",
			["description"] = "d",
			["deviceType"] = 19,
			["customProperties"] = new JArray
			{
				new JObject { ["name"] = "uptime.hostname", ["value"] = "1.1.1.1" },
				new JObject { ["name"] = "uptime.pollingInterval", ["value"] = "5" },
				new JObject { ["name"] = "website.private.serviceParameters", ["value"] = serviceParameters }
			}
		};

		var resource = JsonConvert.DeserializeObject<PingCheckResource>(device.ToString());

		resource.Should().NotBeNull();
		resource!.Id.Should().Be(42);
		resource.ResourceType.Should().Be(ResourceType.Ping);
		resource.HostName.Should().Be("1.1.1.1");
		resource.PacketCount.Should().Be(10);
		resource.TimeoutMs.Should().Be(750);
		resource.PercentPacketsNotReceivedInTime.Should().Be(90);
		resource.Alerting.OverallAlertLevel.Should().Be(Level.Error);
		resource.Alerting.FailedCheckCountBeforeAlerting.Should().Be(3);
		resource.Alerting.AlertCondition.Should().Be(SiteMonitorAlertCondition.HalfOfLocations);
	}

	[Fact]
	public void WebCreationDto_SerializesTopLevelFields()
	{
		var dto = new WebCheckResourceCreationDto
		{
			Name = "web-1",
			HostName = "www.google.com",
			Domain = "www.google.com",
			Scheme = UptimeHttpScheme.Https,
			IgnoreSsl = true,
			PollingIntervalMinutes = 5,
			Steps = [new UptimeWebCheckStep { Name = "__step0", Url = "www.google.com", StatusCode = "200" }]
		};

		var json = JObject.Parse(JsonConvert.SerializeObject(dto));

		json["deviceType"]!.Value<int>().Should().Be(18);

		var customProperties = (JArray)json["customProperties"]!;
		string PropValue(string name) => customProperties.First(p => p["name"]!.Value<string>() == name)["value"]!.Value<string>()!;

		PropValue("system.categories").Should().Be("webcheckdevice");
		PropValue("uptime.url").Should().Be("https://www.google.com");

		// Web config (including steps) lives in the serviceParameters blob, with each step as a "__stepN" string.
		var serviceParameters = JObject.Parse(PropValue("website.private.serviceParameters"));
		serviceParameters["schema"]!.Value<string>().Should().Be("https");
		serviceParameters["domain"]!.Value<string>().Should().Be("www.google.com");
		serviceParameters["ignoreSSL"]!.Value<string>().Should().Be("true");
		serviceParameters["isInternal"]!.Value<string>().Should().Be("true");

		var step0 = JObject.Parse(serviceParameters["__step0"]!.Value<string>()!);
		step0["method"]!.Value<string>().Should().Be("GET");
		step0["statusCode"]!.Value<string>().Should().Be("200");
	}
}

