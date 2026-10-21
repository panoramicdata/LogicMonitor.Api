using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

/// <summary>
/// MS-27673 A Logs report converts to its own type. The payload is a live portal's wire format with identifying values replaced.
/// </summary>
public class LogsReportTests
{
	private const string Json = """
		{
		  "id": 123,
		  "name": "Error logs by resource",
		  "description": "Error log entries from web servers over the last day",
		  "type": "Logs",
		  "typeAlias": "Logs",
		  "groupId": 0,
		  "format": "CSV",
		  "delivery": "email",
		  "recipients": [
		    { "type": "admin", "method": "email", "addr": "report.reader@example.com", "additionInfo": "report.reader@example.com" },
		    { "type": "arbitrary", "method": "email", "addr": "service.desk@example.com", "additionInfo": "" }
		  ],
		  "schedule": "00 07 * * *",
		  "scheduleTimezone": "Europe/London",
		  "lastmodifyUserId": 7,
		  "lastmodifyUserName": "report.author@example.com",
		  "enableViewAsOtherUser": true,
		  "userPermission": "write",
		  "lastGenerateOn": 1788000000,
		  "lastGenerateSize": 1024,
		  "lastGeneratePages": 1,
		  "customReportTypeId": 0,
		  "customReportTypeName": "",
		  "reportLinkNum": 8,
		  "reportLinkExpire": "",
		  "dateRange": "Last 24 hours",
		  "query": "_resource.name ~ \"web\" AND message ~ \"error\"",
		  "timezone": "Europe/London",
		  "partitions": [],
		  "columns": [
		    { "name": "Time", "isHidden": false },
		    { "name": "Resource", "isHidden": false },
		    { "name": "Anomaly", "isHidden": false },
		    { "name": "Severity", "isHidden": false },
		    { "name": "Message", "isHidden": false },
		    { "name": "Groups", "isHidden": false },
		    { "name": "Group (including subgroups)", "isHidden": true }
		  ],
		  "logsDataType": "both",
		  "isVisualizationEnabled": false,
		  "graphType": "",
		  "pieDataColumn": "",
		  "pieSliceLabel": "",
		  "orderingConfig": []
		}
		""";

	private static readonly string[] TextFields =
	[
		"dateRange", "query", "timezone", "logsDataType", "graphType", "pieDataColumn", "pieSliceLabel",
	];

	[Fact]
	public void DeserializesToTheConcreteType()
	{
		var report = JsonConvert.DeserializeObject<ReportBase>(Json);

		report.Should().BeOfType<LogsReport>();
	}

	[Fact]
	public void PopulatesTheCommonFields()
	{
		var report = JsonConvert.DeserializeObject<ReportBase>(Json);

		report!.Id.Should().Be(123);
		report.Name.Should().Be("Error logs by resource");
		report.Type.Should().Be("Logs");
		report.Format.Should().Be("CSV");
		report.Delivery.Should().Be("email");
		report.Recipients.Select(recipient => recipient.Type).Should().Equal("admin", "arbitrary");
	}

	[Fact]
	public void PopulatesTheTypeSpecificFields()
	{
		var report = (LogsReport?)JsonConvert.DeserializeObject<ReportBase>(Json);

		report!.DateRange.Should().Be("Last 24 hours");
		report.Query.Should().Be("_resource.name ~ \"web\" AND message ~ \"error\"");
		report.Timezone.Should().Be("Europe/London");
		report.LogsDataType.Should().Be("both");
		report.IsVisualizationEnabled.Should().BeFalse();
		report.Partitions.Should().BeEmpty();
		report.OrderingConfig.Should().BeEmpty();
		report.Columns.Should().HaveCount(7);
		report.Columns.Where(column => column.IsHidden).Select(column => column.Name).Should().Equal("Group (including subgroups)");
	}

	[Fact]
	public void EachTextFieldIsReadIntoItsOwnProperty()
	{
		// The live values are mostly empty, which would hide two crossed mappings, so each field carries its own name here
		var json = JObject.Parse(Json);
		foreach (var field in TextFields)
		{
			json[field] = field;
		}

		var report = (LogsReport?)JsonConvert.DeserializeObject<ReportBase>(json.ToString());

		report!.DateRange.Should().Be("dateRange");
		report.Query.Should().Be("query");
		report.Timezone.Should().Be("timezone");
		report.LogsDataType.Should().Be("logsDataType");
		report.GraphType.Should().Be("graphType");
		report.PieDataColumn.Should().Be("pieDataColumn");
		report.PieSliceLabel.Should().Be("pieSliceLabel");
	}

	[Fact]
	public void PartitionsAndOrderingAcceptAnyElementShape()
	{
		// Both arrays were empty on the live portal, so their element shape is unknown and must not fail a read
		var json = JObject.Parse(Json);
		json["partitions"] = JArray.Parse("""["default", { "name": "audit" }]""");
		json["orderingConfig"] = JArray.Parse("""[{ "column": "Time", "order": "desc" }]""");

		var report = (LogsReport?)JsonConvert.DeserializeObject<ReportBase>(json.ToString());

		report!.Partitions.Should().HaveCount(2);
		report.OrderingConfig.Should().HaveCount(1);
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
