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
	/// <summary>
	/// A filter with the level and ack/sdt defaults neutralised, so that a test only sees the
	/// filter items it sets itself.
	/// </summary>
	private static AlertFilter Bare() => new()
	{
		Levels = [],
		IncludeCleared = null,
		OrderByProperty = string.Empty
	};

	private static FilterItem<Alert>? Item(Filter<Alert> filter, string property)
		=> filter.FilterItems.SingleOrDefault(fi => fi.Property == property);

	[Fact]
	public void GetFilter_WhenBothIsClearedAndIncludeClearedSet_Throws()
	{
		var filter = new AlertFilter { IsCleared = true, IncludeCleared = true };

		var act = filter.GetFilter;

		act.Should().Throw<InvalidOperationException>()
			.WithMessage("*IsCleared or IncludeCleared*");
	}

	[Fact]
	public void GetFilter_WhenIsClearedFalseAndEndEpochIsAfterSet_Throws()
	{
		var filter = new AlertFilter { IsCleared = false, IncludeCleared = null, EndEpochIsAfter = 1 };

		var act = filter.GetFilter;

		act.Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void GetFilter_WhenIsClearedFalseAndEndEpochIsBeforeSet_Throws()
	{
		var filter = new AlertFilter { IsCleared = false, IncludeCleared = null, EndEpochIsBefore = 1 };

		var act = filter.GetFilter;

		act.Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void GetFilter_WhenBothAlertTypeAndAlertTypesSet_Throws()
	{
		var filter = new AlertFilter
		{
			IncludeCleared = null,
			AlertType = AlertType.DataSource,
			AlertTypes = [AlertType.EventSource]
		};

		var act = filter.GetFilter;

		act.Should().Throw<InvalidOperationException>()
			.WithMessage("*AlertType or AlertTypes*");
	}

	[Fact]
	public void GetFilter_WhenIsClearedFalseAndNoEndEpochSet_DoesNotThrow()
	{
		var filter = Bare();
		filter.IsCleared = false;

		var act = filter.GetFilter;

		act.Should().NotThrow();
	}

	[Fact]
	public void GetFilter_WhenSkipAndTakeSet_CopiesThemOntoTheFilter()
	{
		var filter = Bare();
		filter.Skip = 30;
		filter.Take = 7;

		var result = filter.GetFilter();

		result.Skip.Should().Be(30);
		result.Take.Should().Be(7);
	}

	[Fact]
	public void GetFilter_WhenSkipAndTakeAreNull_LeavesFilterDefaults()
	{
		var result = Bare().GetFilter();

		result.Skip.Should().Be(0);
		result.Take.Should().Be(int.MaxValue);
	}

	[Fact]
	public void GetFilter_WhenOrderByPropertySet_SetsOrder()
	{
		var filter = Bare();
		filter.OrderByProperty = nameof(Alert.StartOnSeconds);
		filter.OrderDirection = OrderDirection.Asc;

		var result = filter.GetFilter();

		result.Order.Should().NotBeNull();
		result.Order!.Property.Should().Be(nameof(Alert.StartOnSeconds));
		result.Order.Direction.Should().Be(OrderDirection.Asc);
	}

	[Fact]
	public void GetFilter_WhenPropertiesSetWithoutId_AddsIdSoResultsCanBeIdentified()
	{
		var filter = Bare();
		filter.Properties = [nameof(Alert.Severity)];

		var result = filter.GetFilter();

		result.Properties.Should().Contain(nameof(Alert.Id));
		result.Properties.Should().Contain(nameof(Alert.Severity));
	}

	[Fact]
	public void GetFilter_WhenPropertiesAlreadyContainId_DoesNotAddItTwice()
	{
		var filter = Bare();
		filter.Properties = [nameof(Alert.Id), nameof(Alert.Severity)];

		var result = filter.GetFilter();

		result.Properties.Count(p => p == nameof(Alert.Id)).Should().Be(1);
	}

	/// <summary>
	/// Levels is a non-nullable list, so an empty Levels still appends a (valueless) severity
	/// item - AppendFilterItemIfNotNull only skips nulls. Pinned because it is surprising.
	/// </summary>
	[Fact]
	public void GetFilter_WhenNoFieldsSet_ProducesOnlyAnEmptySeverityItem()
	{
		var result = Bare().GetFilter();

		result.FilterItems.Should().ContainSingle();
		var only = result.FilterItems[0];
		only.Property.Should().Be(nameof(Alert.Severity));
		only.Value.Should().BeAssignableTo<List<string>>().Which.Should().BeEmpty();
	}

	[Fact]
	public void GetFilter_MapsSimpleScalarFields()
	{
		var filter = Bare();
		filter.Id = "DS1234";
		filter.InternalId = "internal-1";
		filter.AlertRuleName = "rule";
		filter.AlertRuleId = 11;
		filter.EscalationChainName = "chain";
		filter.EscalationChainId = "22";
		filter.NextRecipient = "recipient";
		filter.AckedBy = "acker";
		filter.MonitorObjectName = "resource";
		filter.MonitorObjectId = 33;
		filter.DataPointName = "dp";
		filter.DataPointId = "44";
		filter.ResourceTemplateId = 55;
		filter.InstanceId = "66";

		var result = filter.GetFilter();

		Item(result, nameof(Alert.Id))!.Value.Should().Be("DS1234");
		Item(result, nameof(Alert.InternalId))!.Value.Should().Be("internal-1");
		Item(result, nameof(Alert.AlertRuleName))!.Value.Should().Be("rule");
		Item(result, nameof(Alert.AlertRuleId))!.Value.Should().Be(11);
		Item(result, nameof(Alert.AlertEscalationChainName))!.Value.Should().Be("chain");
		Item(result, nameof(Alert.AlertEscalationChainId))!.Value.Should().Be("22");
		Item(result, nameof(Alert.NextRecipient))!.Value.Should().Be("recipient");
		Item(result, nameof(Alert.AckedBy))!.Value.Should().Be("acker");
		Item(result, nameof(Alert.MonitorObjectName))!.Value.Should().Be("resource");
		Item(result, nameof(Alert.MonitorObjectId))!.Value.Should().Be(33);
		Item(result, nameof(Alert.DataPointName))!.Value.Should().Be("dp");
		Item(result, nameof(Alert.DataPointId))!.Value.Should().Be("44");
		Item(result, nameof(Alert.ResourceTemplateId))!.Value.Should().Be(55);
		Item(result, nameof(Alert.InstanceId))!.Value.Should().Be("66");

		// Every one of these is an equality match
		result.FilterItems.Should().AllSatisfy(fi => fi.Operation.Should().Be(":"));
	}

	[Fact]
	public void GetFilter_MapsEpochBoundsToGreaterAndLessThanOperations()
	{
		var filter = Bare();
		filter.StartEpochIsAfter = 100;
		filter.StartEpochIsBefore = 200;
		filter.EndEpochIsAfter = 300;
		filter.EndEpochIsBefore = 400;

		var result = filter.GetFilter();

		var startItems = result.FilterItems.Where(fi => fi.Property == nameof(Alert.StartOnSeconds)).ToList();
		startItems.Should().ContainSingle(fi => fi.Operation == ">" && Equals(fi.Value, 100L));
		startItems.Should().ContainSingle(fi => fi.Operation == "<" && Equals(fi.Value, 200L));

		var endItems = result.FilterItems.Where(fi => fi.Property == nameof(Alert.EndOnSeconds)).ToList();
		endItems.Should().ContainSingle(fi => fi.Operation == ">" && Equals(fi.Value, 300L));
		endItems.Should().ContainSingle(fi => fi.Operation == "<" && Equals(fi.Value, 400L));
	}

	[Theory]
	[InlineData(AckFilter.Acked, "true")]
	[InlineData(AckFilter.Nonacked, "false")]
	public void GetFilter_WhenAckFilterIsNotAll_MapsToLowercaseBoolean(AckFilter ackFilter, string expected)
	{
		var filter = Bare();
		filter.AckFilter = ackFilter;

		var result = filter.GetFilter();

		Item(result, nameof(Alert.Acked))!.Value.Should().Be(expected);
	}

	[Fact]
	public void GetFilter_WhenAckFilterIsAll_OmitsTheAckedItem()
	{
		var filter = Bare();
		filter.AckFilter = AckFilter.All;

		var result = filter.GetFilter();

		Item(result, nameof(Alert.Acked)).Should().BeNull();
	}

	[Theory]
	[InlineData(SdtFilter.Sdt, "true")]
	[InlineData(SdtFilter.NonSdt, "false")]
	public void GetFilter_WhenSdtFilterIsNotAll_MapsToLowercaseBoolean(SdtFilter sdtFilter, string expected)
	{
		var filter = Bare();
		filter.SdtFilter = sdtFilter;

		var result = filter.GetFilter();

		Item(result, nameof(Alert.InScheduledDownTime))!.Value.Should().Be(expected);
	}

	[Fact]
	public void GetFilter_WhenSdtFilterIsAll_OmitsTheSdtItem()
	{
		var filter = Bare();
		filter.SdtFilter = SdtFilter.All;

		var result = filter.GetFilter();

		Item(result, nameof(Alert.InScheduledDownTime)).Should().BeNull();
	}

	[Fact]
	public void GetFilter_MapsLevelsToTheirIntegerValuesInDescendingOrder()
	{
		var filter = Bare();
		filter.Levels = [AlertLevel.Warning, AlertLevel.Critical, AlertLevel.Error];

		var result = filter.GetFilter();

		Item(result, nameof(Alert.Severity))!.Value
			.Should().BeEquivalentTo(new List<string> { "4", "3", "2" }, o => o.WithStrictOrdering());
	}
}
