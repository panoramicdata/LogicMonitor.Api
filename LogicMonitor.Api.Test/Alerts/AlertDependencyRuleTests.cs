using LogicMonitor.Api.Alerts;
using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Alerts;

public class AlertDependencyRuleTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	// Real payload captured from a live portal's GET /setting/alert/dependencyrules/{id}.
	private const string RealRuleJson = """
	{
	  "id": 1,
	  "name": "test",
	  "priority": 1,
	  "description": "test desc",
	  "enableRoutingDelay": true,
	  "delayMinutes": 3,
	  "disableReachAlertRouting": true,
	  "disableNonReachAlertRouting": true,
	  "resourceMatchPatternList": [
	    { "group": "PDL - Panoramic Data", "resource": "AU-E1:networkinterface:pdl-col-au-01893_z1" }
	  ]
	}
	""";

	[Fact]
	public void AlertDependencyRule_DeserialisesRealSchema_WithNoMissingMembers()
	{
		// MissingMemberHandling.Error mirrors how the client deserialises portal responses, so this
		// fails if the model is missing any field the real API returns.
		var settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error };
		var rule = JsonConvert.DeserializeObject<AlertDependencyRule>(RealRuleJson, settings);

		rule.Should().NotBeNull();
		rule!.Id.Should().Be(1);
		rule.Name.Should().Be("test");
		rule.Description.Should().Be("test desc");
		rule.Priority.Should().Be(1);
		rule.EnableRoutingDelay.Should().BeTrue();
		rule.DelayMinutes.Should().Be(3);
		rule.DisableReachAlertRouting.Should().BeTrue();
		rule.DisableNonReachAlertRouting.Should().BeTrue();
		rule.ResourceMatchPatternList.Should().ContainSingle();
		rule.ResourceMatchPatternList[0].Group.Should().Be("PDL - Panoramic Data");
		rule.ResourceMatchPatternList[0].Resource.Should().Be("AU-E1:networkinterface:pdl-col-au-01893_z1");
		rule.Endpoint().Should().Be("setting/alert/dependencyrules");
	}

	[Fact]
	public async Task GetAllAlertDependencyRules_DoesNotThrow()
	{
		// Validates the endpoint wiring and that any rules present on the portal deserialise cleanly.
		var rules = await LogicMonitorClient
			.GetAllAsync<AlertDependencyRule>(CancellationToken);

		rules.Should().NotBeNull();
	}
}
