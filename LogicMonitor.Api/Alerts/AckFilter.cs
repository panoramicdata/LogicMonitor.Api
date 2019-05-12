namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// Filter used when retrieving alerts
	/// </summary>
	public enum AckFilter : byte
	{
		/// <summary>
		/// All alerts
		/// </summary>
		All = 0,

		/// <summary>
		/// Acknowledged alerts only
		/// </summary>
		Acked = 1,

		/// <summary>
		/// Unacknowledged alerts only
		/// </summary>
		Nonacked = 2,
	}
}