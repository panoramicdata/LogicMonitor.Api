namespace LogicMonitor.Api.Filters
{
	/// <summary>
	/// An includes filter item
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Includes<T> : FilterItem<T>
	{
		/// <summary>
		/// An includes item
		/// </summary>
		/// <param name="property">The property e.g. nameof(ScheduledDownTime.Id)</param>
		/// <param name="value">The value (e.g. D123)</param>
		public Includes(string property, object value)
		{
			Property = property;
			Operation = "~";
			Value = value;
		}
	}
}