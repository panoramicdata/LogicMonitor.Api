using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

/// <summary>
/// MS-26936 An Alerts Health Check report converts to its own type. The type-specific fields are a live portal's response; the common fields are placeholders.
/// </summary>
public class AlertsHealthCheckReportTests
{
	private const string Json = """
		{
		  "id": 123,
		  "name": "Alerts health check",
		  "description": "",
		  "type": "AlertsHealthCheck",
		  "typeAlias": "AlertsHealthCheck",
		  "groupId": 0,
		  "format": "HTML",
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
		  "dateRange": "Last hour",
		  "hostGroup": "",
		  "host": "*",
		  "dataSource": "*",
		  "instance": "*",
		  "dataPoint": "*",
		  "severity": "all",
		  "ackFilter": "all",
		  "anomaly": "all",
		  "sdtFilter": "all",
		  "activeOnly": true,
		  "clearFilter": "no",
		  "rule": "*",
		  "chain": "*",
		  "isEscalation": "all",
		  "isIncludeDeletedResourceOrWebsite": "yes",
		  "monitoredObjectGroups": "",
		  "dependencyRole": "all",
		  "dependencyRoutingState": "all",
		  "columns": [
		    { "name": "Severity", "isHidden": false },
		    { "name": "Began", "isHidden": false },
		    { "name": "Resource", "isHidden": false },
		    { "name": "Instance", "isHidden": false },
		    { "name": "Datapoint", "isHidden": false },
		    { "name": "Group", "isHidden": true },
		    { "name": "Thresholds", "isHidden": false },
		    { "name": "Value", "isHidden": false },
		    { "name": "End", "isHidden": false },
		    { "name": "Datasource", "isHidden": false },
		    { "name": "Rule", "isHidden": false },
		    { "name": "Chain", "isHidden": false },
		    { "name": "Acked", "isHidden": true },
		    { "name": "Acked On", "isHidden": true },
		    { "name": "Acked By", "isHidden": true },
		    { "name": "Notes", "isHidden": true },
		    { "name": "In SDT", "isHidden": true },
		    { "name": "dependencyRole", "isHidden": false },
		    { "name": "dependencyRoutingState", "isHidden": false }
		  ]
		}
		""";

	private static readonly string[] TextFields =
	[
		"hostGroup", "host", "dataSource", "instance", "dataPoint", "severity", "ackFilter", "anomaly", "sdtFilter",
		"clearFilter", "rule", "chain", "isEscalation", "isIncludeDeletedResourceOrWebsite", "monitoredObjectGroups",
		"dependencyRole", "dependencyRoutingState",
	];

	[Fact]
	public void DeserializesToTheConcreteType()
	{
		var report = JsonConvert.DeserializeObject<ReportBase>(Json);

		report.Should().BeOfType<AlertsHealthCheckReport>();
	}

	[Fact]
	public void PopulatesTheCommonFields()
	{
		var report = JsonConvert.DeserializeObject<ReportBase>(Json);

		report!.Id.Should().Be(123);
		report.Name.Should().Be("Alerts health check");
		report.Type.Should().Be("AlertsHealthCheck");
		report.Format.Should().Be("HTML");
	}

	[Fact]
	public void PopulatesTheTypeSpecificFields()
	{
		var report = (AlertsHealthCheckReport?)JsonConvert.DeserializeObject<ReportBase>(Json);

		report!.DateRange.Should().Be("Last hour");
		report.ActiveOnly.Should().BeTrue();
		report.Columns.Should().HaveCount(19);
		report.Columns.Where(column => column.IsHidden).Select(column => column.Name).Should().Equal("Group", "Acked", "Acked On", "Acked By", "Notes", "In SDT");
	}

	[Fact]
	public void EachTextFieldIsReadIntoItsOwnProperty()
	{
		// The live values repeat "*" and "all", which would hide two crossed mappings, so each field carries its own name here
		var json = JObject.Parse(Json);
		foreach (var field in TextFields)
		{
			json[field] = field;
		}

		var report = (AlertsHealthCheckReport?)JsonConvert.DeserializeObject<ReportBase>(json.ToString());

		report!.GroupFullPath.Should().Be("hostGroup");
		report.ResourceDisplayName.Should().Be("host");
		report.DataSource.Should().Be("dataSource");
		report.DataSourceInstanceName.Should().Be("instance");
		report.DataPoint.Should().Be("dataPoint");
		report.Level.Should().Be("severity");
		report.AckFilter.Should().Be("ackFilter");
		report.Anomaly.Should().Be("anomaly");
		report.SdtFilter.Should().Be("sdtFilter");
		report.ClearFilter.Should().Be("clearFilter");
		report.Rule.Should().Be("rule");
		report.Chain.Should().Be("chain");
		report.IsEscalation.Should().Be("isEscalation");
		report.IsIncludeDeletedResourceOrWebsite.Should().Be("isIncludeDeletedResourceOrWebsite");
		report.MonitoredObjectGroups.Should().Be("monitoredObjectGroups");
		report.DependencyRole.Should().Be("dependencyRole");
		report.DependencyRoutingState.Should().Be("dependencyRoutingState");
	}

	[Fact]
	public void PassesTheClientsDebugDeserialisation()
	{
		// The client's Debug settings, which fail on any field the model lacks
		var act = () => JsonConvert.DeserializeObject<ReportBase>(Json, new JsonSerializerSettings
		{
			MissingMemberHandling = MissingMemberHandling.Error,
			ContractResolver = new RequireObjectPropertiesContractResolver(),
		});

		act.Should().NotThrow();
	}
}
