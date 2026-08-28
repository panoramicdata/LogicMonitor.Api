using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

/// <summary>
/// Cover for issue #31.
///
/// The polymorphic converters pick a concrete subclass from a "type" discriminator. When "type" was
/// absent - because the caller projected it away with a field selection - they fell to the same
/// default branch as an unrecognised value and reported:
///
///     NotSupportedException: WidgetConverter.cs needs updating to include  widgets.
///
/// which says the library needs a code change when the fix was one word in the caller's field list.
/// The only hint was a double space where the type name should be.
///
/// Observed downstream rather than hypothesised: GET /dashboard/widgets?fields=id,name threw this,
/// and adding "type" to the same request returned 1,059 widgets. Same for /report/reports (110).
/// </summary>
public class PolymorphicConverterMessageTests
{
	[Fact]
	public void Widget_WhenTypeIsAbsent_SaysTheFieldIsMissingRatherThanBlamingTheLibrary()
	{
		var json = """{"id": 1, "name": "Alerts"}""";

		var act = () => JsonConvert.DeserializeObject<Widget>(json);

		var exception = act.Should().Throw<NotSupportedException>().Which;
		exception.Message.Should().Contain("no 'type' field");
		exception.Message.Should().Contain("include 'type'");
		exception.Message.Should().NotContain("needs updating");
	}

	[Fact]
	public void Widget_WhenTypeIsUnrecognised_StillAsksForTheLibraryToBeUpdated()
	{
		// The other half of the distinction: a genuinely unknown type IS a library gap, and must keep
		// saying so rather than sending the reader off to check their field selection.
		var json = """{"id": 1, "name": "Thing", "type": "somefuturewidget"}""";

		var act = () => JsonConvert.DeserializeObject<Widget>(json);

		var exception = act.Should().Throw<NotSupportedException>().Which;
		exception.Message.Should().Contain("needs updating");
		exception.Message.Should().Contain("somefuturewidget");
	}

	[Fact]
	public void Report_WhenTypeIsAbsent_SaysTheFieldIsMissing()
	{
		var json = """{"id": 2, "name": "Test report"}""";

		var act = () => JsonConvert.DeserializeObject<ReportBase>(json);

		var exception = act.Should().Throw<NotSupportedException>().Which;
		exception.Message.Should().Contain("no 'type' field");
		exception.Message.Should().NotContain("needs updating");
	}

	[Fact]
	public void Widget_WhenTypeIsPresent_DeserializesToTheConcreteSubtype()
	{
		// Guards the fix from being a blanket "type is null" bail-out that breaks the normal path.
		var json = """{"id": 6, "name": "Alerts", "type": "alert"}""";

		var widget = JsonConvert.DeserializeObject<Widget>(json);

		widget.Should().BeOfType<AlertWidget>();
		widget!.Name.Should().Be("Alerts");
	}
}
