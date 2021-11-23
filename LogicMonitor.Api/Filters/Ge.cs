namespace LogicMonitor.Api.Filters;

/// <summary>
/// A greater than or equals filter item
/// </summary>
/// <typeparam name="T"></typeparam>
public class Ge<T> : FilterItem<T>
{
	/// <summary>
	/// A greater than or equals filter item
	/// </summary>
	/// <param name="property">The property e.g. nameof(ScheduledDownTime.Id)</param>
	/// <param name="value">The value (e.g. D123)</param>
	public Ge(string property, object value)
	{
		Property = property;
		Comparator = Comparator.Ge;
		Value = value;
	}
}
