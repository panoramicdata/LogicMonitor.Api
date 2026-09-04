using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

/// <summary>
/// Cover for issue #36.
///
/// A portal containing an Uptime Resource Overview report made every call to
/// GetAllAsync&lt;ReportBase&gt;() throw, because ReportConverter did not model the type - so all 241
/// of that portal's reports were unreachable because of 2.
///
/// The payload below is the live wire format, with identifying values replaced. The type-specific
/// fields match WebsiteOverviewReport exactly; that was confirmed against the portal rather than
/// assumed, which is why the two classes are siblings with the same shape.
/// </summary>
public class UptimeResourceOverviewReportTests
{
	private const string Json = """
		{
		  "id": 261,
		  "name": "Availability Raw Data - Overall",
		  "type": "Uptime Resource Overview",
		  "typeAlias": "Uptime Resource Overview",
		  "groupId": 0,
		  "format": "HTML",
		  "delivery": "none",
		  "dateRange": "Last calendar month",
		  "items": "Devices by Type/Temporary/Global Router",
		  "itemsType": "group",
		  "displayType": 2,
		  "includeTypes": [1, 2, 3],
		  "excludeSDT": false,
		  "exclude100Availability": false
		}
		""";

	[Fact]
	public void DeserializesToTheConcreteType()
	{
		var report = JsonConvert.DeserializeObject<ReportBase>(Json);

		report.Should().BeOfType<UptimeResourceOverviewReport>();
	}

	[Fact]
	public void PopulatesTheTypeSpecificFields()
	{
		var report = (UptimeResourceOverviewReport?)JsonConvert.DeserializeObject<ReportBase>(Json);

		report!.DateRange.Should().Be("Last calendar month");
		report.Items.Should().Be("Devices by Type/Temporary/Global Router");
		report.ItemsType.Should().Be("group");
		report.DisplayType.Should().Be(2);
		report.IncludeTypes.Should().HaveCount(3);
		report.ExcludeSdt.Should().BeFalse();
		report.Exclude100Availability.Should().BeFalse();
	}

	[Fact]
	public void PopulatesTheCommonFields()
	{
		var report = JsonConvert.DeserializeObject<ReportBase>(Json);

		report!.Id.Should().Be(261);
		report.Name.Should().Be("Availability Raw Data - Overall");
		report.Type.Should().Be("Uptime Resource Overview");
		report.Format.Should().Be("HTML");
	}

	[Fact]
	public void AnUnrecognisedTypeStillThrows()
	{
		// Deliberate, and unchanged by this fix: an unmodelled type is a genuine gap in this library
		// and must keep saying so, per issue #31. Whether that is fatal is the caller's policy.
		var json = """{"id": 1, "name": "Thing", "type": "somefuturereport"}""";

		var act = () => JsonConvert.DeserializeObject<ReportBase>(json);

		act.Should().Throw<NotSupportedException>()
			.Which.Message.Should().Contain("somefuturereport");
	}
}
