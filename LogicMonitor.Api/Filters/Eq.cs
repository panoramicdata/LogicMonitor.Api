namespace LogicMonitor.Api.Filters;

/// <summary>
/// An equality filter item
/// </summary>
/// <typeparam name="T"></typeparam>
public class Eq<T> : FilterItem<T>
{
	/// <summary>
	/// An equality filter item
	/// </summary>
	/// <param name="property">The property e.g. nameof(ScheduledDownTime.Id)</param>
	/// <param name="value">The value (e.g. D123)</param>
	public Eq(string property, object value)
	{
		Property = property;
		Comparator = Comparator.Eq;
		Value = value;
	}
}
