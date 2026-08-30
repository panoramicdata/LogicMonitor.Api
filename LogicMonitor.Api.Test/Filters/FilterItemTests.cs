namespace LogicMonitor.Api.Test.Filters;

/// <summary>
/// Portal-free characterisation tests for <see cref="FilterItem{T}"/>.
/// </summary>
/// <remarks>
/// FilterItem.ToString renders one clause of every filtered query, including the quoting and
/// list-joining rules the portal expects. These pin the rendering for each value kind and each
/// null-sensitive operation so that the method can be restructured safely.
/// </remarks>
public class FilterItemTests
{
	private static string Render(string property, string operation, object? value)
		=> new FilterItem<Alert> { Property = property, Operation = operation, Value = value }.ToString();

	#region Value rendering

	[Fact]
	public void ToString_WithAStringValue_QuotesIt()
		=> Render(nameof(Alert.MonitorObjectName), ":", "server1")
			.Should().Be("monitorObjectName:\"server1\"");

	[Fact]
	public void ToString_WithAnIntegerValue_RendersItBare()
		=> Render(nameof(Alert.MonitorObjectId), ":", 42)
			.Should().Be("monitorObjectId:42");

	[Theory]
	[InlineData(true, "true")]
	[InlineData(false, "false")]
	public void ToString_WithABooleanValue_RendersItLowercase(bool value, string expected)
		=> Render(nameof(Alert.Acked), ":", value)
			.Should().Be($"acked:{expected}");

	[Fact]
	public void ToString_WithAListValue_QuotesEachItemAndJoinsWithAPipe()
		=> Render(nameof(Alert.Severity), ":", new List<string> { "4", "3" })
			.Should().Be("severity:\"4\"|\"3\"");

	[Fact]
	public void ToString_WithAnEmptyListValue_RendersNoValue()
		=> Render(nameof(Alert.Severity), ":", new List<string>())
			.Should().Be("severity:");

	[Fact]
	public void ToString_WithAnEnumValue_UsesItsSerializationName()
		=> Render(nameof(Alert.Severity), ":", AlertLevel.Critical)
			.Should().Be("severity:\"critical\"");

	#endregion

	#region Operations

	[Theory]
	[InlineData(">")]
	[InlineData("<")]
	[InlineData(">:")]
	[InlineData("!:")]
	[InlineData("~")]
	[InlineData("!~")]
	public void ToString_RendersTheOperationVerbatim(string operation)
		=> Render(nameof(Alert.MonitorObjectId), operation, 7)
			.Should().Be($"monitorObjectId{operation}7");

	[Theory]
	[InlineData(":::null")]
	[InlineData(":::empty")]
	[InlineData("!::null")]
	[InlineData("!::empty")]
	public void ToString_ForANullOnlyOperation_RequiresANullValue(string operation)
	{
		var act = () => Render(nameof(Alert.MonitorObjectName), operation, "not-null");

		act.Should().Throw<InvalidOperationException>()
			.WithMessage($"*must be null for the '{operation}'*");
	}

	[Theory]
	[InlineData(":::null")]
	[InlineData(":::empty")]
	[InlineData("!::null")]
	[InlineData("!::empty")]
	public void ToString_ForANullOnlyOperationWithANullValue_RendersWithNoValue(string operation)
		=> Render(nameof(Alert.MonitorObjectName), operation, null)
			.Should().Be($"monitorObjectName{operation}");

	[Fact]
	public void ToString_ForAnOrdinaryOperationWithANullValue_Throws()
	{
		var act = () => Render(nameof(Alert.MonitorObjectName), ":", null);

		act.Should().Throw<InvalidOperationException>()
			.WithMessage("*must not be null for the ':'*");
	}

	#endregion

	#region Comparator round trip

	[Theory]
	[InlineData(Comparator.Eq, ":")]
	[InlineData(Comparator.IsNull, ":::null")]
	[InlineData(Comparator.IsNullOrEmpty, ":::empty")]
	[InlineData(Comparator.Ge, ">:")]
	[InlineData(Comparator.Gt, ">")]
	[InlineData(Comparator.Includes, "~")]
	[InlineData(Comparator.Le, "<=")]
	[InlineData(Comparator.Lt, "<")]
	[InlineData(Comparator.Ne, "!:")]
	[InlineData(Comparator.IsNotNull, "!::null")]
	[InlineData(Comparator.IsNotNullOrEmpty, "!::empty")]
	[InlineData(Comparator.NotIncludes, "!~")]
	public void Comparator_SetsTheExpectedOperation(Comparator comparator, string expectedOperation)
	{
		var filterItem = new FilterItem<Alert> { Comparator = comparator };

		filterItem.Operation.Should().Be(expectedOperation);
	}

	[Theory]
	[InlineData(Comparator.Eq)]
	[InlineData(Comparator.IsNull)]
	[InlineData(Comparator.Ge)]
	[InlineData(Comparator.Gt)]
	[InlineData(Comparator.Includes)]
	[InlineData(Comparator.Lt)]
	[InlineData(Comparator.Ne)]
	[InlineData(Comparator.IsNotNull)]
	[InlineData(Comparator.NotIncludes)]
	public void Comparator_RoundTripsThroughOperation(Comparator comparator)
	{
		var filterItem = new FilterItem<Alert> { Comparator = comparator };

		filterItem.Comparator.Should().Be(comparator);
	}

	[Fact]
	public void Comparator_ForAnUnrecognisedOperation_Throws()
	{
		var filterItem = new FilterItem<Alert> { Operation = "??" };

		var act = () => filterItem.Comparator;

		act.Should().Throw<NotSupportedException>();
	}

	#endregion
}
