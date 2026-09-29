using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

/// <summary>
/// MS-26846 A Host inventory report's property filter and metrics are read. The payload is the live wire format with identifying values replaced.
/// </summary>
public class ResourceInventoryReportTests
{
	private const string Json = """
		{
		  "id": 123,
		  "name": "Interface inventory",
		  "description": "",
		  "type": "Host inventory",
		  "typeAlias": "Device inventory",
		  "groupId": 0,
		  "format": "CSV",
		  "delivery": "none",
		  "recipients": [],
		  "schedule": "",
		  "scheduleTimezone": "",
		  "lastmodifyUserId": 7,
		  "lastmodifyUserName": "report.author@example.com",
		  "enableViewAsOtherUser": true,
		  "userPermission": "write",
		  "lastGenerateOn": 1788000000,
		  "lastGenerateSize": 1024,
		  "lastGeneratePages": 1,
		  "customReportTypeId": 0,
		  "customReportTypeName": "",
		  "reportLinkNum": 0,
		  "reportLinkExpire": "",
		  "hostsVal": "Devices by Type/Network",
		  "hostsValType": "group",
		  "sortedBy": "system.displayname",
		  "properties": [
		    "system.displayname",
		    "system.hostname"
		  ],
		  "metrics": [
		    {
		      "dataSourceId": 42,
		      "dataSourceFullName": "Network Interfaces",
		      "instances": "*"
		    }
		  ],
		  "propertyFilterMetric": {
		    "resourceFilter": {
		      "dynamic": [
		        {
		          "mode": "include",
		          "field": "system.categories",
		          "type": "RESOURCE_PROPERTY",
		          "expressions": [
		            { "operator": "EQ", "value": "router" },
		            { "operator": "EQ", "value": "switch" }
		          ]
		        }
		      ],
		      "selectedValues": [],
		      "propertyKey": null
		    },
		    "instanceFilter": null
		  }
		}
		""";

	private static ResourceInventoryReport Read(string json)
		=> JsonConvert.DeserializeObject<ReportBase>(json).Should().BeOfType<ResourceInventoryReport>().Subject;

	[Fact]
	public void PopulatesTheResourceFilter()
	{
		var rule = Read(Json).PropertyFilterMetric!.ResourceFilter!.Dynamic.Should().ContainSingle().Subject;

		rule.Mode.Should().Be("include");
		rule.Field.Should().Be("system.categories");
		rule.Type.Should().Be("RESOURCE_PROPERTY");
		rule.Expressions.Select(expression => expression.Operator).Should().Equal("EQ", "EQ");
		rule.Expressions.Select(expression => expression.Value).Should().Equal("router", "switch");
	}

	[Fact]
	public void AFilterThatIsNotSet_IsNull()
	{
		var filters = Read("""{"id": 1, "name": "Thing", "type": "Host inventory", "propertyFilterMetric": {"resourceFilter": null, "instanceFilter": null}}""")
			.PropertyFilterMetric!;

		filters.ResourceFilter.Should().BeNull();
		filters.InstanceFilter.Should().BeNull();
	}

	[Fact]
	public void PopulatesTheMetrics()
	{
		var metric = Read(Json).Metrics.Should().ContainSingle().Subject;

		metric.DataSourceId.Should().Be(42);
		metric.DataSourceFullName.Should().Be("Network Interfaces");
		metric.Instances.Should().Be("*");
	}

	[Fact]
	public void EveryFieldOfTheWireFormatIsModelled()
	{
		// MissingMemberHandling.Error mirrors the client's Debug deserialisation, so this throws on any unmodelled field
		var act = () => JsonConvert.DeserializeObject<ReportBase>(Json, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });

		act.Should().NotThrow();
	}
}
