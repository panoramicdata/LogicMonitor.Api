namespace LogicMonitor.Api.Logs
{
	/// <summary>
	/// The entity type of an audit event
	/// </summary>
	public enum AuditEventEntityType
	{
		Unknown,
		Resource,
		DeviceDataSourceInstance,
		ScheduledDownTime,
	}
}
