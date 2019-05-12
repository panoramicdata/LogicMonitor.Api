namespace LogicMonitor.Api.Filters
{
	/// <summary>
	/// A less than filter item
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Lt<T> : FilterItem<T>
	{
		/// <summary>
		/// A less than filter item
		/// </summary>
		/// <param name="property">The property e.g. nameof(ScheduledDownTime.Id)</param>
		/// <param name="value">The value (e.g. D123)</param>
		public Lt(string property, object value)
		{
			Property = property;
			Comparator = Comparator.Lt;
			Value = value;
		}
	}
}