using System;
using System.IO;

namespace LogicMonitor.Api.Backup
{
	/// <summary>
	///     A configuration backup specification
	/// </summary>
	public class ConfigurationBackupSpecification
	{
		/// <summary>
		///     Constructor
		/// </summary>
		/// <param name="includeAllByDefault"></param>
		public ConfigurationBackupSpecification(bool includeAllByDefault)
		{
			AccountSettings = includeAllByDefault;
			Alerting = includeAllByDefault;
			Collectors = includeAllByDefault;
			Dashboards = includeAllByDefault;
#pragma warning disable CS0618 // Type or member is obsolete
			LogicModules = includeAllByDefault;
#pragma warning restore CS0618 // Type or member is obsolete
			Devices = includeAllByDefault;
			Integrations = includeAllByDefault;
			Logs = includeAllByDefault;
			Netscans = includeAllByDefault;
			OpsNotes = includeAllByDefault;
			ScheduledDownTimes = includeAllByDefault;
			Websites = includeAllByDefault;
			Users = includeAllByDefault;
		}

		/// <summary>
		///     Whether to back up AccountSettings
		/// </summary>
		public bool AccountSettings { get; set; }

		/// <summary>
		///     Whether to back up AlertRules and EscalationChains
		/// </summary>
		public bool Alerting { get; set; }

		/// <summary>
		///     Whether to back up Collectors
		/// </summary>
		public bool Collectors { get; set; }

		/// <summary>
		///     Whether to back up Dashboards
		/// </summary>
		public bool Dashboards { get; set; }

		/// <summary>
		///     Whether to back up all LogicModules
		/// </summary>
		[Obsolete("Select individual LogicModules instead.")]
		public bool LogicModules
		{
			get =>
				AppliesToFunctions
				&& ConfigSources
				&& DataSources
				&& EventSources
				&& JobMonitors
				&& PropertySources
				&& SnmpSysOidMaps;
			set
			{
				AppliesToFunctions = value;
				ConfigSources = value;
				DataSources = value;
				EventSources = value;
				JobMonitors = value;
				PropertySources = value;
				SnmpSysOidMaps = value;
			}
		}

		/// <summary>
		///     Whether to back up AppliesToFunctions
		/// </summary>
		public bool AppliesToFunctions { get; set; }

		/// <summary>
		///     Whether to back up ConfigSources
		/// </summary>
		public bool ConfigSources { get; set; }

		/// <summary>
		///     Whether to back up DataSources
		/// </summary>
		public bool DataSources { get; set; }

		/// <summary>
		///     Whether to back up EventSources
		/// </summary>
		public bool EventSources { get; set; }

		/// <summary>
		///     Whether to back up JobMonitors
		/// </summary>
		public bool JobMonitors { get; set; }

		/// <summary>
		///     Whether to back up PropertySources
		/// </summary>
		public bool PropertySources { get; set; }

		/// <summary>
		///     Whether to back up SnmpSysOidMaps
		/// </summary>
		public bool SnmpSysOidMaps { get; set; }

		/// <summary>
		///     Whether to back up Devices
		/// </summary>
		public bool Devices { get; set; }

		/// <summary>
		///     Whether to back up Integrations
		/// </summary>
		public bool Integrations { get; set; }

		/// <summary>
		///     Whether to back up Logs
		/// </summary>
		public bool Logs { get; set; }

		/// <summary>
		///     Whether to back up NetscanPolicies
		/// </summary>
		public bool Netscans { get; set; }

		/// <summary>
		///     Whether to back up OpsNotes
		/// </summary>
		public bool OpsNotes { get; set; }

		/// <summary>
		///     Whether to back up ScheduledDownTimes
		/// </summary>
		public bool ScheduledDownTimes { get; set; }

		/// <summary>
		///     Whether to back up Websites
		/// </summary>
		public bool Websites { get; set; }

		/// <summary>
		///     Whether to back up Users
		/// </summary>
		public bool Users { get; set; }

		/// <summary>
		///     The Gzip file info.  If null, no file is written
		/// </summary>
		public FileInfo GzipFileInfo { get; set; }
	}
}