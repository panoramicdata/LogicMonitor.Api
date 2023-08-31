﻿namespace LogicMonitor.Api.Logs;

/// <summary>
/// The action type of an audit event
/// </summary>
public enum AuditEventActionType
{
	/// <summary>
	/// Unknown / not set
	/// </summary>
	None,

	/// <summary>
	/// Create
	/// </summary>
	Create,

	/// <summary>
	/// Read
	/// </summary>
	Read,

	/// <summary>
	/// Update
	/// </summary>
	Update,

	/// <summary>
	/// Delete
	/// </summary>
	Delete,

	/// <summary>
	/// Scheduled health check script
	/// </summary>
	ScheduledHealthCheckScript,

	/// <summary>
	/// A login event
	/// </summary>
	Login,

	/// <summary>
	/// A general API event
	/// </summary>
	GeneralApi,

	/// <summary>
	/// An event alert was discarded
	/// </summary>
	DiscardedEventAlert,

	/// <summary>
	/// Run
	/// </summary>
	Run,

	/// <summary>
	/// Enable
	/// </summary>
	Enable,

	/// <summary>
	/// Disable
	/// </summary>
	Disable,

	/// <summary>
	/// Request Remote Session
	/// </summary>
	RequestRemoteSession,

	/// <summary>
	/// Download
	/// </summary>
	Download,

	/// <summary>
	/// Request Password Reset
	/// </summary>
	RequestPasswordReset
}
