using LogicMonitor.Api.Extensions;
using LogicMonitor.Api.Time;
using System;

namespace LogicMonitor.Api;

/// <summary>
/// A time-based request
/// </summary>
public abstract class TimeBasedRequest : IRequest
{
	/// <summary>
	/// The time period
	/// </summary>
	public TimePeriod TimePeriod { get; set; }

	/// <summary>
	/// The start date time
	/// </summary>
	public DateTime StartDateTime { get; set; }

	/// <summary>
	/// The start date time
	/// </summary>
	public DateTime EndDateTime { get; set; }

	///// <summary>
	///// Partial query string
	///// </summary>
	///// <returns></returns>
	//protected string GetTimePartialQueryString()
	//{
	//	switch (TimePeriod)
	//	{
	//		case TimePeriod.Unknown:
	//			throw new ArgumentException("TimePeriod not set.");
	//		case TimePeriod.Zoom:
	//			return $"&time=zoom&startTime={StartDateTime.SecondsSinceTheEpoch()}&endTime={EndDateTime.SecondsSinceTheEpoch()}";
	//		default:
	//			return $"&time={TimePeriod.ToString().LowerCaseFirst()}";
	//	}
	//}

	/// <summary>
	/// Used for flow queries
	/// </summary>
	/// <exception cref="ArgumentException"></exception>
	protected string GetTimePartialQueryStringNew()
	{
		if (StartDateTime > EndDateTime)
		{
			throw new InvalidOperationException("StartDateTime must be before EndDateTime");
		}

		return TimePeriod switch
		{
			TimePeriod.Unknown => throw new ArgumentException("TimePeriod not set."),
			TimePeriod.Zoom => $"&start={StartDateTime.SecondsSinceTheEpoch()}&end={EndDateTime.SecondsSinceTheEpoch()}",

			// NO: the below 'startEpochSec' appears not used, it's just 'start' in the portal...same for end...
			//TimePeriod.Zoom => $"&startEpochSec={StartDateTime.SecondsSinceTheEpoch()}&endEpochSec={EndDateTime.SecondsSinceTheEpoch()}",

			// NO: the below uses e.g. "fiveHours" but LogicMonitor API uses 5hour...
			//_ => $"&time={TimePeriod.ToString().LowerCaseFirst()}",
			_ => $"&time={EnumHelper.ToEnumString(TimePeriod)}",
		};
	}

	/// <summary>
	/// IRequest abstract method
	/// </summary>
	public abstract string GetQueryString();
}
