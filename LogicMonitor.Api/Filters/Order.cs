namespace LogicMonitor.Api.Filters;

/// <summary>
///     An order
/// </summary>
/// <typeparam name="T"></typeparam>
public class Order<T>
{
	/// <summary>
	///     The direction
	/// </summary>
	public OrderDirection Direction { get; set; }

	/// <summary>
	///     The class property name
	/// </summary>
	public string Property { get; set; }

	/// <inheritdoc />
	public override string ToString()
	{
		if (string.IsNullOrWhiteSpace(Property))
		{
			throw new InvalidOperationException("Field cannot be empty.");
		}

		var orderField = LogicMonitorClient.GetSerializationName<T>(Property);
		return $"sort={(Direction == OrderDirection.Desc ? "-" : string.Empty)}{orderField}";
	}
}
