namespace LogicMonitor.Api.Filters
{
	/// <summary>
	/// An greater than filter item
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Gt<T> : FilterItem<T>
	{
		/// <summary>
		/// An greater than filter item
		/// </summary>
		/// <param name="property">The property e.g. nameof(ScheduledDownTime.Id)</param>
		/// <param name="value">The value (e.g. D123)</param>
		public Gt(string property, object value)
		{
			Property = property;
			Comparator = Comparator.Gt;
			Value = value;
		}
	}
}