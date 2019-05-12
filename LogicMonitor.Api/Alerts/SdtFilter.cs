namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// Filter used when retrieving alerts
	/// </summary>
	public enum SdtFilter : byte
	{
		/// <summary>
		/// All alerts
		/// </summary>
		All = 0,

		/// <summary>
		/// Only devices and DataSources experiencing SDT
		/// </summary>
		Sdt = 1,

		/// <summary>
		/// Only devices and DataSources not experiencing SDT
		/// </summary>
		NonSdt = 2,
	}
}