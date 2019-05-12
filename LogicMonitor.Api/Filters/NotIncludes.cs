namespace LogicMonitor.Api.Filters
{
	/// <summary>
	/// A not includes filter item
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class NotIncludes<T> : FilterItem<T>
	{
		/// <summary>
		/// An not includes filter item
		/// </summary>
		/// <param name="property">The property e.g. nameof(ScheduledDownTime.Id)</param>
		/// <param name="value">The value (e.g. D123)</param>
		public NotIncludes(string property, object value)
		{
			Property = property;
			Comparator = Comparator.NotIncludes;
			Value = value;
		}
	}
}