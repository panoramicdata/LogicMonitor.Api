namespace LogicMonitor.Api.Filters;

/// <summary>
///     A query filter
/// </summary>
/// <typeparam name="T"></typeparam>
public class Filter<T>
{
	/// <summary>
	///     The number to skip
	/// </summary>
	public int Skip { get; set; }

	/// <summary>
	///     The number to take
	/// </summary>
	public int Take { get; set; } = int.MaxValue;

	/// <summary>
	///     The order the results should come back
	/// </summary>
	public Order<T> Order { get; set; }

	/// <summary>
	///  The filter type (defaults to "And")
	/// </summary>
	public FilterType Type { get; set; }

	/// <summary>
	///     Filters
	/// </summary>
	public List<FilterItem<T>> FilterItems { get; set; } = new List<FilterItem<T>>();

	/// <summary>
	///     Extra filters
	/// </summary>
	public List<FilterItem<T>> ExtraFilters { get; set; }

	/// <summary>
	///     The properties to complete
	/// </summary>
	public List<string> Properties { get; set; }

	/// <summary>
	/// If present will be used as the query string on the request
	/// </summary>
	public string QueryString { get; set; }

	/// <inheritdoc />
	public override string ToString()
	{
		Validate();
		return QueryString != null
				  ? $"offset={Skip}&size={Take}&{QueryString}"
				  : $"offset={Skip}&size={Take}{(Order == null ? string.Empty : $"&{Order}")}{(FilterItems == null || FilterItems.Count == 0 ? string.Empty : $"&filter={HttpUtility.UrlEncode(string.Join(Type == FilterType.And ? "," : "||", FilterItems.Select(fi => fi.ToString())))}")}{(ExtraFilters == null || ExtraFilters.Count == 0 ? string.Empty : $"&extraFilters={HttpUtility.UrlEncode(string.Join(",", ExtraFilters.Select(fi => fi.ToJsonString())))}")}{(Properties == null ? string.Empty : $"&fields={HttpUtility.UrlEncode(string.Join(",", GetFields()))}")}";
	}

	private void Validate()
	{
		if (Skip < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(Skip), "must be greater than 0");
		}
	}

	private IEnumerable<string> GetFields()
	{
		var typeProperties = typeof(T).GetProperties();
		foreach (var propertyName in Properties)
		{
			var property = typeProperties.SingleOrDefault(p => p.Name == propertyName);
			if (property == null)
			{
				continue;
			}

			yield return LogicMonitorClient.GetSerializationName<T>(propertyName);
		}
	}

	internal void AppendFilterItemIfNotNull(string property, object value, string operation = ":")
	{
		if (value == null)
		{
			return;
		}

		FilterItems.Add(new FilterItem<T> { Property = property, Operation = operation, Value = value });
	}
}
