namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// ScheduledDownTime filter
/// The default value of Take is 300
/// </summary>
public class ScheduledDownTimeFilter
{
	/// <summary>
	/// Whether is effective
	/// </summary>
	public bool? IsEffective { get; set; }

	/// <summary>
	/// Number to skip
	/// </summary>
	public int? Skip { get; set; }

	/// <summary>
	/// The sort direction
	/// </summary>
	public OrderDirection OrderDirection { get; set; }

	/// <summary>
	/// The sort property
	/// </summary>
	public string OrderByProperty { get; set; } = string.Empty;

	/// <summary>
	/// Number to take
	/// </summary>
	public int? Take { get; set; } = 300;

	/// <summary>
	///    Type
	///    If omitted, brings back SDTS of all types
	/// </summary>
	public ScheduledDownTimeType? Type { get; set; }

	/// <summary>
	/// The ResourceId (only works for Type=Resource)
	/// </summary>
	public int? ResourceId { get; set; }

	/// <summary>
	/// The ResourceId (only works for Type=ResourceGroup)
	/// </summary>
	public int? ResourceGroupId { get; set; }

	/// <summary>
	/// The ResourceDataSourceInstanceId (only works for Type=ResourceDataSourceInstance)
	/// </summary>
	public int? DataSourceInstanceId { get; set; }

	/// <summary>
	/// The DataSourceInstanceGroup Id (only works for Type=ResourceDataSourceInstanceGroup)
	/// </summary>
	public int? DataSourceInstanceGroupId { get; set; }

	/// <summary>
	/// The start date time in seconds since the Epoch
	/// </summary>
	public long StartDateTimeEpoch { get; set; }

	/// <summary>
	/// Start DateTime in UTC
	/// </summary>
	public DateTime StartDateTimeUtc
	{
		get => StartDateTimeEpoch.ToDateTimeUtc();
		set => StartDateTimeEpoch = value.SecondsSinceTheEpoch();
	}

	/// <summary>
	/// The CollectorId
	/// </summary>
	public int? CollectorId { get; set; }

	internal Filter<ScheduledDownTime> GetFilter()
	{
		var filter = new Filter<ScheduledDownTime>();

		if (OrderByProperty is not null)
		{
			filter.Order = new Order<ScheduledDownTime> { Direction = OrderDirection, Property = OrderByProperty.LowerCaseFirst() };
		}

		if (Skip is not null)
		{
			filter.Skip = Skip.Value;
		}

		if (Take is not null)
		{
			filter.Take = Take.Value;
		}

		filter.AppendFilterItemIfNotNull(nameof(ScheduledDownTime.IsEffective), IsEffective);
		filter.AppendFilterItemIfNotNull(nameof(ScheduledDownTime.ResourceGroupId), ResourceGroupId);
		if (Type is not null)
		{
			filter.AppendFilterItemIfNotNull(nameof(ScheduledDownTime.Type), $"{Type}SDT");
		}

		filter.AppendFilterItemIfNotNull(nameof(ScheduledDownTime.ResourceId), ResourceId);
		filter.AppendFilterItemIfNotNull(nameof(ScheduledDownTime.DataSourceInstanceId), DataSourceInstanceId);
		filter.AppendFilterItemIfNotNull(nameof(ScheduledDownTime.DataSourceInstanceGroupId), DataSourceInstanceGroupId);
		filter.AppendFilterItemIfNotNull(nameof(ScheduledDownTime.CollectorId), CollectorId);
		filter.AppendFilterItemIfNotNull(nameof(ScheduledDownTime.StartDateTimeMs), StartDateTimeEpoch, ">");

		return filter;
	}
}
