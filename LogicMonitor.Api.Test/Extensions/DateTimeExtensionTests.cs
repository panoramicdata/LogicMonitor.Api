using LogicMonitor.Api.Extensions;
using System;
using System.Linq;
using Xunit;

namespace LogicMonitor.Api.Test.Extensions
{
	public class DateTimeExtensionTests
	{
		[Fact]
		public void InvalidValuesThrowsException() => Assert.Throws<ArgumentException>(() => new DateTime(1976, 02, 18).GetChunkedTimeRangeList(new DateTime(1975, 02, 17), TimeSpan.FromHours(1)));

		[Fact]
		public void RangeToLargeThrowsException() => Assert.Throws<ArgumentException>(() => new DateTime(1975, 02, 17).GetChunkedTimeRangeList(new DateTime(1978, 04, 25), TimeSpan.FromHours(1)));

		[Fact]
		public void ValuesEqualBringsBackSingleItem()
		{
			var dateTime = new DateTime(1975, 02, 17);
			var list = dateTime.GetChunkedTimeRangeList(dateTime, TimeSpan.FromHours(1));
			Assert.NotNull(list);
			Assert.Single(list);
			Assert.Equal(list.Single().Item1, dateTime);
			Assert.Equal(list.Single().Item1, list.Single().Item2);
		}

		[Fact]
		public void ValuesOneHourApartBringsBackSingleItem()
		{
			var startDateTime = new DateTime(1975, 02, 17);
			var endDateTime = startDateTime.AddHours(1);
			var list = startDateTime.GetChunkedTimeRangeList(endDateTime, TimeSpan.FromHours(1));
			Assert.NotNull(list);
			Assert.Single(list);
			Assert.Equal(list.Single().Item1, startDateTime);
			Assert.Equal(list.Single().Item2, endDateTime);
		}

		[Fact]
		public void ValuesOneHourAndOneMinuteApartBringsBackTwoItems()
		{
			var startDateTime = new DateTime(1975, 02, 17);
			var endDateTime = startDateTime.AddHours(1).AddMinutes(1);
			var list = startDateTime.GetChunkedTimeRangeList(endDateTime, TimeSpan.FromHours(1));
			Assert.NotNull(list);
			Assert.Equal(2, list.Count);

			// First range
			Assert.Equal(list[0].Item1, startDateTime);
			Assert.Equal(list[0].Item2, startDateTime.AddHours(1));

			// Second range
			Assert.Equal(list.Last().Item1, startDateTime.AddHours(1));
			Assert.Equal(list.Last().Item2, endDateTime);
		}

		[Fact]
		public void ValuesTwoHoursAndOneMinuteApartBringsBackTwoItemsWithAChunkSizeOf2Hours()
		{
			var startDateTime = new DateTime(1975, 02, 17);
			var endDateTime = startDateTime.AddHours(2).AddMinutes(1);
			var list = startDateTime.GetChunkedTimeRangeList(endDateTime, TimeSpan.FromHours(2));
			Assert.NotNull(list);
			Assert.Equal(2, list.Count);

			// First range
			Assert.Equal(list[0].Item1, startDateTime);
			Assert.Equal(list[0].Item2, startDateTime.AddHours(2));

			// Second range
			Assert.Equal(list.Last().Item1, startDateTime.AddHours(2));
			Assert.Equal(list.Last().Item2, endDateTime);
		}

		[Fact]
		public void ValuesOneHourTwoHoursAndOneMinuteApartBringsBackThreeItems()
		{
			var startDateTime = new DateTime(1975, 02, 17);
			var endDateTime = startDateTime.AddHours(2).AddMinutes(1);
			var list = startDateTime.GetChunkedTimeRangeList(endDateTime, TimeSpan.FromHours(1));
			Assert.NotNull(list);
			Assert.Equal(3, list.Count);

			// First range
			Assert.Equal(startDateTime, list[0].Item1);
			Assert.Equal(startDateTime.AddHours(1), list[0].Item2);

			// Second range
			Assert.Equal(startDateTime.AddHours(1), list.Skip(1).First().Item1);
			Assert.Equal(startDateTime.AddHours(2), list.Skip(1).First().Item2);

			// Last range
			Assert.Equal(startDateTime.AddHours(2), list.Last().Item1);
			Assert.Equal(endDateTime, list.Last().Item2);
		}
	}
}