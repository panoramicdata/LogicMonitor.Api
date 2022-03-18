namespace LogicMonitor.Api.Test.Extensions;

public class DateTimeExtensionTests
{
	[Fact]
	public void InvalidValuesThrowsException()
		=> new DateTime(1976, 02, 18)
		.Invoking(x => x.GetChunkedTimeRangeList(new DateTime(1975, 02, 17), TimeSpan.FromHours(1)))
		.Should()
		.Throw<ArgumentException>();

	[Fact]
	public void RangeToLargeThrowsException()
		 => new DateTime(1975, 02, 17)
		.Invoking(x => x.GetChunkedTimeRangeList(new DateTime(1978, 04, 25), TimeSpan.FromHours(1)))
		.Should()
		.Throw<ArgumentException>();

	[Fact]
	public void ValuesEqualBringsBackSingleItem()
	{
		var dateTime = new DateTime(1975, 02, 17);
		var list = dateTime.GetChunkedTimeRangeList(dateTime, TimeSpan.FromHours(1));
		list.Should().NotBeNull();
		list.Should().ContainSingle();
		dateTime.Should().Be(list.Single().Item1);
		list.Single().Item2.Should().Be(list.Single().Item1);
	}

	[Fact]
	public void ValuesOneHourApartBringsBackSingleItem()
	{
		var startDateTime = new DateTime(1975, 02, 17);
		var endDateTime = startDateTime.AddHours(1);
		var list = startDateTime.GetChunkedTimeRangeList(endDateTime, TimeSpan.FromHours(1));
		list.Should().NotBeNull();
		list.Should().ContainSingle();
		startDateTime.Should().Be(list.Single().Item1);
		endDateTime.Should().Be(list.Single().Item2);
	}

	[Fact]
	public void ValuesOneHourAndOneMinuteApartBringsBackTwoItems()
	{
		var startDateTime = new DateTime(1975, 02, 17);
		var endDateTime = startDateTime.AddHours(1).AddMinutes(1);
		var list = startDateTime.GetChunkedTimeRangeList(endDateTime, TimeSpan.FromHours(1));
		list.Should().NotBeNull();
		list.Should().HaveCount(2);

		// First range
		startDateTime.Should().Be(list[0].Item1);
		startDateTime.AddHours(1).Should().Be(list[0].Item2);

		// Second range
		startDateTime.AddHours(1).Should().Be(list.Last().Item1);
		endDateTime.Should().Be(list.Last().Item2);
	}

	[Fact]
	public void ValuesTwoHoursAndOneMinuteApartBringsBackTwoItemsWithAChunkSizeOf2Hours()
	{
		var startDateTime = new DateTime(1975, 02, 17);
		var endDateTime = startDateTime.AddHours(2).AddMinutes(1);
		var list = startDateTime.GetChunkedTimeRangeList(endDateTime, TimeSpan.FromHours(2));
		list.Should().NotBeNull();
		list.Should().HaveCount(2);

		// First range
		startDateTime.Should().Be(list[0].Item1);
		startDateTime.AddHours(2).Should().Be(list[0].Item2);

		// Second range
		startDateTime.AddHours(2).Should().Be(list.Last().Item1);
		endDateTime.Should().Be(list.Last().Item2);
	}

	[Fact]
	public void ValuesOneHourTwoHoursAndOneMinuteApartBringsBackThreeItems()
	{
		var startDateTime = new DateTime(1975, 02, 17);
		var endDateTime = startDateTime.AddHours(2).AddMinutes(1);
		var list = startDateTime.GetChunkedTimeRangeList(endDateTime, TimeSpan.FromHours(1));
		list.Should().NotBeNull();
		list.Should().HaveCount(3);

		// First range
		list[0].Item1.Should().Be(startDateTime);
		list[0].Item2.Should().Be(startDateTime.AddHours(1));

		// Second range
		list.Skip(1).First().Item1.Should().Be(startDateTime.AddHours(1));
		list.Skip(1).First().Item2.Should().Be(startDateTime.AddHours(2));

		// Last range
		list.Last().Item1.Should().Be(startDateTime.AddHours(2));
		list.Last().Item2.Should().Be(endDateTime);
	}
}
