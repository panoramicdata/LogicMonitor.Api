namespace LogicMonitor.Api.Filters
{
	/// <summary>
	/// A less than or equal to filter item
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Le<T> : FilterItem<T>
	{
		/// <summary>
		/// A less than or equal to filter item
		/// </summary>
		/// <param name="property">The property e.g. nameof(ScheduledDownTime.Id)</param>
		/// <param name="value">The value (e.g. D123)</param>
		public Le(string property, object value)
		{
			Property = property;
			Comparator = Comparator.Le;
			Value = value;
		}
	}
}