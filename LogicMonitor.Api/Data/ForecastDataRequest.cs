using LogicMonitor.Api.Extensions;
using System;

namespace LogicMonitor.Api.Data
{
	/// <summary>
	///    A request for forecast data
	/// </summary>
	public class ForecastDataRequest : GraphDataRequestBase
	{
		/// <summary>
		///    The graph Id
		/// </summary>
		public int GraphId { get; set; }

		/// <summary>
		///    The DataSource instance Id
		/// </summary>
		public int DataSourceInstanceId { get; set; }

		/// <summary>
		///    The selected DataPoint name
		/// </summary>
		public string DataPointLabel { get; set; }

		/// <summary>
		///    The forecast time period
		/// </summary>
		public ForecastTimePeriod ForecastTimePeriod { get; set; }

		/// <summary>
		/// Training time period
		/// </summary>
		public TrainingTimePeriod TrainingTimePeriod { get; set; }

		internal string TrainingSubUrl => $"device/devicedatasourceinstances/{DataSourceInstanceId}/graphs/{GraphId}/data/training?selectedDataPointLabel={DataPointLabel}&time={EnumHelper.ToEnumString(TrainingTimePeriod)}&start={StartTime(TrainingTimePeriod).SecondsSinceTheEpoch()}&end={DateTime.UtcNow.SecondsSinceTheEpoch()}";

		internal string ForecastedSubUrl => $"device/devicedatasourceinstances/{DataSourceInstanceId}/graphs/{GraphId}/data/forecasting?selectedDataPointLabel={DataPointLabel}&time={EnumHelper.ToEnumString(TrainingTimePeriod)}&forecastTime={EnumHelper.ToEnumString(ForecastTimePeriod)}&start={StartTime(TrainingTimePeriod).SecondsSinceTheEpoch()}&end={DateTime.UtcNow.SecondsSinceTheEpoch()}";

		/// <summary>
		///    Returns a string that represents the current object.
		/// </summary>
		public override string ToString() => $"{TrainingTimePeriod}/{ForecastTimePeriod} for DataSourceInstance {DataSourceInstanceId}, Graph {GraphId} and DataPoint {DataPointLabel}";

		/// <inheritdoc />
		public override void Validate()
		{
			if (GraphId <= 0)
			{
				throw new InvalidOperationException("GraphId must be specified");
			}
			if (DataSourceInstanceId <= 0)
			{
				throw new InvalidOperationException("DataSourceInstanceId must be specified");
			}
			if (ForecastTimePeriod == ForecastTimePeriod.Unknown)
			{
				throw new InvalidOperationException("ForecastTimePeriod must be specified");
			}
			if (TrainingTimePeriod == TrainingTimePeriod.Unknown)
			{
				throw new InvalidOperationException("TraingingTimePeriod must be specified");
			}
		}

		private DateTime StartTime(TrainingTimePeriod trainingTime)
			=> trainingTime switch
			{
				TrainingTimePeriod.ThreeMonths => DateTime.UtcNow.AddMonths(-3),
				TrainingTimePeriod.SixMonths => DateTime.UtcNow.AddMonths(-6),
				TrainingTimePeriod.OneYear => DateTime.UtcNow.AddYears(-1),
				_ => DateTime.UtcNow.AddDays(-30),
			};
	}
}