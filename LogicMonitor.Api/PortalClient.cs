﻿using LogicMonitor.Api.Attributes;
using LogicMonitor.Api.Collectors;
using LogicMonitor.Api.Converters;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.LogicModules;
using LogicMonitor.Api.OpsNotes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
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
}