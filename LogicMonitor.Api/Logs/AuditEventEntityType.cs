namespace LogicMonitor.Api.Logs
{
	/// <summary>
	/// The entity type of an audit event
	/// </summary>
	public enum AuditEventEntityType
	{
		/// <summary>
		/// Unknown
		/// </summary>
		Unknown,

		/// <summary>
		/// Resource
		/// </summary>
		Resource,

		/// <summary>
		/// Devcie DataSource Instance
		/// </summary>
		DeviceDataSourceInstance,

		/// <summary>
		/// Scheduled Down Time
		/// </summary>
		ScheduledDownTime,
	}
}
