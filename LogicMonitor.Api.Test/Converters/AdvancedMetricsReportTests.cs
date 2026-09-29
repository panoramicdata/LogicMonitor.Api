using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

/// <summary>
/// MS-26846 An Advanced Metrics report converts to its own type. The payload is the live wire format with identifying values replaced.
/// </summary>
public class AdvancedMetricsReportTests
{
	private const string Json = """
		{
		  "id": 123,
		  "name": "CPU busy by host",
		  "description": "",
		  "type": "Advanced Metrics",
		  "typeAlias": "Advanced Metrics",
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
		  "query": "CPUBusyPercent{datasource=\"WinCPU\"}",
		  "columns": [
		    { "name": "Host", "isHidden": false },
		    { "name": "Datasource", "isHidden": false },
		    { "name": "Instance", "isHidden": false },
		    { "name": "CPUBusyPercent", "isHidden": false },
		    { "name": "Reported At", "isHidden": false }
		  ],
		  "dateRange": "",
		  "metricSource": "lmql"
		}
		""";

	[Fact]
	public void DeserializesToTheConcreteType()
	{
		var report = JsonConvert.DeserializeObject<ReportBase>(Json);

		report.Should().BeOfType<AdvancedMetricsReport>();
	}

	[Fact]
	public void PopulatesTheTypeSpecificFields()
	{
		var report = (AdvancedMetricsReport?)JsonConvert.DeserializeObject<ReportBase>(Json);

		report!.Query.Should().Be("CPUBusyPercent{datasource=\"WinCPU\"}");
		report.MetricSource.Should().Be("lmql");
		report.DateRange.Should().BeEmpty();
		report.Columns.Select(column => column.Name).Should().Equal("Host", "Datasource", "Instance", "CPUBusyPercent", "Reported At");
		report.Columns.Should().OnlyContain(column => !column.IsHidden);
	}

	[Fact]
	public void PopulatesTheCommonFields()
	{
		var report = JsonConvert.DeserializeObject<ReportBase>(Json);

		report!.Id.Should().Be(123);
		report.Name.Should().Be("CPU busy by host");
		report.Type.Should().Be("Advanced Metrics");
		report.Format.Should().Be("CSV");
	}

	[Fact]
	public void EveryFieldOfTheWireFormatIsModelled()
	{
		// MissingMemberHandling.Error mirrors the client's Debug deserialisation, so this throws on any unmodelled field
		var act = () => JsonConvert.DeserializeObject<ReportBase>(Json, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });

		act.Should().NotThrow();
	}
}
