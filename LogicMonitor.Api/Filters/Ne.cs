namespace LogicMonitor.Api.Filters
{
	/// <summary>
	/// An inequality filter item
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Ne<T> : FilterItem<T>
	{
		/// <summary>
		/// An inequality filter item
		/// </summary>
		/// <param name="property">The property e.g. nameof(ScheduledDownTime.Id)</param>
		/// <param name="value">The value (e.g. D123)</param>
		public Ne(string property, object value)
		{
			Property = property;
			Comparator = Comparator.Ne;
			Value = value;
		}
	}
}