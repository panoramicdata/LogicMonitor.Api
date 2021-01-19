namespace LogicMonitor.Api.Logs
{
	/// <summary>
	/// The action type of an audit event
	/// </summary>
	public enum AuditEventActionType
	{
		/// <summary>
		/// Unknown / not set
		/// </summary>
		Unknown,

		/// <summary>
		/// Create
		/// </summary>
		Create,

		/// <summary>
		/// Update
		/// </summary>
		Update,

		/// <summary>
		/// Delete
		/// </summary>
		Delete,
	}
}
