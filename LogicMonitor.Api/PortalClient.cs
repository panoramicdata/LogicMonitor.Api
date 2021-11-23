using Microsoft.Extensions.Logging;
using System;

namespace LogicMonitor.Api;

/// <summary>
///     The main client for querying the portal.
/// </summary>
public class PortalClient : LogicMonitorClient
{
	/// <summary>
	/// Create a LogicMonitor client using subdomain, access id and access key, with an optional logger
	/// </summary>
	/// <param name="account">The account subdomain</param>
	/// <param name="accessId">The access id</param>
	/// <param name="accessKey">The access key</param>
	/// <param name="iLogger">An optional ILogger</param>
	[Obsolete("Use LogicMonitorClient instead.")]
	public PortalClient(
		string account,
		string accessId,
		string accessKey,
		ILogger iLogger = null) : base(new LogicMonitorClientOptions { Account = account, AccessId = accessId, AccessKey = accessKey, Logger = iLogger })
	{
	}
}
