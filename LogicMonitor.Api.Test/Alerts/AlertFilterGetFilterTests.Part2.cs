namespace LogicMonitor.Api.Test.Alerts;

/// <summary>
/// Portal-free characterisation tests for <see cref="AlertFilter.GetFilter"/>.
/// </summary>
/// <remarks>
/// GetFilter builds the query that every alert request is issued with, so a silent change in
/// its output corrupts alert queries without failing anything. These tests pin the current
/// behaviour - every validation rule, every field mapping and every cleared/uncleared branch -
/// so that the method can be restructured safely.
/// </remarks>
public partial class AlertFilterGetFilterTests
{

	[Fact]
	public void GetFilter_WhenMonitorObjectGroupFullPathsSet_MapsThem()
	{
		var filter = Bare();
		filter.MonitorObjectGroupFullPaths = ["a/b", "c/d"];

		var result = filter.GetFilter();

		Item(result, nameof(Alert.MonitorObjectGroups))!.Value
			.Should().BeEquivalentTo(new List<string> { "a/b", "c/d" });
	}

	[Fact]
	public void GetFilter_EscapesBackslashesInResourceTemplateNameAndInstanceName()
	{
		var filter = Bare();
		filter.ResourceTemplateName = @"Win\Volume";
		filter.InstanceName = @"C:\Users";

		var result = filter.GetFilter();

		Item(result, nameof(Alert.ResourceTemplateName))!.Value.Should().Be(@"Win\\Volume");
		Item(result, nameof(Alert.InstanceName))!.Value.Should().Be(@"C:\\Users");
	}

	[Fact]
	public void GetFilter_WhenSingleAlertTypeSet_MapsItsQueryString()
	{
		var filter = Bare();
		filter.AlertType = AlertType.DataSource;

		var result = filter.GetFilter();

		Item(result, nameof(Alert.AlertType))!.Value
			.Should().BeEquivalentTo(new List<string> { AlertType.DataSource.GetQueryString() });
	}

	[Fact]
	public void GetFilter_WhenAlertTypesSet_MapsAllOfTheirQueryStrings()
	{
		var filter = Bare();
		filter.AlertTypes = [AlertType.DataSource, AlertType.Website];

		var result = filter.GetFilter();

		Item(result, nameof(Alert.AlertType))!.Value
			.Should().BeEquivalentTo(new List<string>
			{
				AlertType.DataSource.GetQueryString(),
				AlertType.Website.GetQueryString()
			});
	}

	[Fact]
	public void GetFilter_WhenNoAlertTypeSet_OmitsTheAlertTypeItem()
	{
		var result = Bare().GetFilter();

		Item(result, nameof(Alert.AlertType)).Should().BeNull();
	}

	[Fact]
	public void GetFilter_WhenIncludeClearedTrue_AddsWildcardIsClearedItem()
	{
		var filter = Bare();
		filter.IncludeCleared = true;

		var result = filter.GetFilter();

		Item(result, nameof(Alert.IsCleared))!.Value.Should().Be("*");
	}

	[Fact]
	public void GetFilter_WhenIncludeClearedFalse_AddsNoClearedItemAtAll()
	{
		var filter = Bare();
		filter.IncludeCleared = false;

		var result = filter.GetFilter();

		Item(result, nameof(Alert.IsCleared)).Should().BeNull();
		Item(result, nameof(Alert.EndOnSeconds)).Should().BeNull();
	}

	[Fact]
	public void GetFilter_WhenIsClearedTrue_RequiresAPositiveEndTime()
	{
		var filter = Bare();
		filter.IsCleared = true;

		var result = filter.GetFilter();

		var item = Item(result, nameof(Alert.EndOnSeconds));
		item.Should().NotBeNull();
		item!.Operation.Should().Be(">");
		item.Value.Should().Be(0);
	}

	[Fact]
	public void GetFilter_WhenIsClearedFalse_RequiresAZeroEndTime()
	{
		var filter = Bare();
		filter.IsCleared = false;

		var result = filter.GetFilter();

		var item = Item(result, nameof(Alert.EndOnSeconds));
		item.Should().NotBeNull();
		item!.Operation.Should().Be(":");
		item.Value.Should().Be(0);
	}

	[Fact]
	public void GetFilter_WhenIncludeClearedSet_TakesPrecedenceOverTheIsClearedBranches()
	{
		// IncludeCleared is checked first, so its branch wins and no EndOnSeconds item is added.
		var filter = Bare();
		filter.IncludeCleared = true;

		var result = filter.GetFilter();

		Item(result, nameof(Alert.EndOnSeconds)).Should().BeNull();
	}

	[Fact]
	public void GetAlertTypes_WhenNeitherSet_ReturnsNull()
		=> new AlertFilter().GetAlertTypes().Should().BeNull();

	[Fact]
	public void GetAlertTypes_WhenOnlyAlertTypeSet_ReturnsThatOne()
		=> new AlertFilter { AlertType = AlertType.Website }
			.GetAlertTypes().Should().BeEquivalentTo(new[] { AlertType.Website });

	[Fact]
	public void GetAlertTypes_WhenOnlyAlertTypesSet_ReturnsThem()
		=> new AlertFilter { AlertTypes = [AlertType.DataSource, AlertType.EventSource] }
			.GetAlertTypes().Should().BeEquivalentTo(new[] { AlertType.DataSource, AlertType.EventSource });

	/// <summary>
	/// The out-of-the-box filter is what most callers use, so its exact shape is worth pinning.
	/// </summary>
	[Fact]
	public void GetFilter_WithDefaultAlertFilter_ProducesTheExpectedDefaultQuery()
	{
		var result = new AlertFilter().GetFilter();

		result.Order.Should().NotBeNull();
		result.Order!.Property.Should().Be(nameof(Alert.StartOnSeconds));
		result.Order.Direction.Should().Be(OrderDirection.Desc);

		// Error and Critical, descending
		Item(result, nameof(Alert.Severity))!.Value
			.Should().BeEquivalentTo(new List<string> { "4", "3" }, o => o.WithStrictOrdering());

		// IncludeCleared defaults to true
		Item(result, nameof(Alert.IsCleared))!.Value.Should().Be("*");

		// Ack and SDT both default to All, so neither appears
		Item(result, nameof(Alert.Acked)).Should().BeNull();
		Item(result, nameof(Alert.InScheduledDownTime)).Should().BeNull();
	}
}
