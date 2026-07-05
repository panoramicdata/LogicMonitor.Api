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

		// v3 structured Uptime creation body (flat top-level fields, not customProperties/serviceParameters).
		json["type"]!.Value<string>().Should().Be("uptimepingcheck");
		json["model"]!.Value<string>().Should().Be("websiteDevice");
		json["deviceType"]!.Value<int>().Should().Be(19);
		json["displayName"]!.Value<string>().Should().Be("ping-1"); // defaulted from Name
		json["isInternal"]!.Value<bool>().Should().BeTrue();
		((JArray)json["groupIds"]!).Select(t => t.Value<int>()).Should().Equal(1);

		json["host"]!.Value<string>().Should().Be("8.8.8.8");
		json["count"]!.Value<int>().Should().Be(5);
		json["timeoutInMSPktsNotReceive"]!.Value<int>().Should().Be(500);
		json["percentPktsNotReceiveInTime"]!.Value<int>().Should().Be(80);
		json["pollingInterval"]!.Value<int>().Should().Be(5);
		json["overallAlertLevel"]!.Value<string>().Should().Be("critical");
		json["individualAlertLevel"]!.Value<string>().Should().Be("warn");
		json["globalSmAlertCond"]!.Value<int>().Should().Be(0);

		// Internal checks reference real Collectors via testLocation.collectorIds; smgIds is empty.
		((JArray)json["testLocation"]!["collectorIds"]!).Select(t => t.Value<int>()).Should().Equal(7);
		((JArray)json["testLocation"]!["smgIds"]!).Should().BeEmpty();
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

		// v3 structured Uptime creation body: web fields are top-level and steps are a real JSON array.
		json["type"]!.Value<string>().Should().Be("uptimewebcheck");
		json["deviceType"]!.Value<int>().Should().Be(18);
		json["domain"]!.Value<string>().Should().Be("www.google.com");
		json["schema"]!.Value<string>().Should().Be("https");
		json["ignoreSSL"]!.Value<bool>().Should().BeTrue();
		json["isInternal"]!.Value<bool>().Should().BeTrue();

		var steps = (JArray)json["steps"]!;
		steps.Should().HaveCount(1);
		var step0 = (JObject)steps[0];
		step0["type"]!.Value<string>().Should().Be("script"); // internal web step
		step0["HTTPMethod"]!.Value<string>().Should().Be("GET");
		step0["statusCode"]!.Value<string>().Should().Be("200");
	}
}

