using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	/// An external alert destination
	/// </summary>
	[DataContract]
	public class ExternalAlertDestination : IdentifiedItem, IHasEndpoint
	{
		/// <summary>
		/// The groups
		/// </summary>
		[DataMember(Name = "groups")]
		public string Groups { get; set; }

		/// <summary>
		/// The Collector Id
		/// </summary>
		[DataMember(Name = "collectorId")]
		public int CollectorId { get; set; }

		/// <summary>
		/// The collectorDescription
		/// </summary>
		[DataMember(Name = "collectorDescription")]
		public string CollectorDescription { get; set; }

		/// <summary>
		/// The mechanism
		/// </summary>
		[DataMember(Name = "mechanism")]
		public string Mechanism { get; set; }

		/// <summary>
		/// The script path
		/// </summary>
		[DataMember(Name = "scriptPath")]
		public string ScriptPath { get; set; }

		/// <summary>
		/// The script command line
		/// </summary>
		[DataMember(Name = "scriptCmdline")]
		public string ScriptCommandLine { get; set; }

		/// <summary>
		/// The snmpTrapVersion
		/// </summary>
		[DataMember(Name = "snmpTrapVersion")]
		public string SnmpTrapVersion { get; set; }

		/// <summary>
		/// The SNMP Trap server
		/// </summary>
		[DataMember(Name = "snmpTrapServer")]
		public string SnmpTrapServer { get; set; }

		/// <summary>
		/// The SNMP community string
		/// </summary>
		[DataMember(Name = "snmpCommunity")]
		public string SnmpCommunityString { get; set; }

		/// <summary>
		/// The Syslog server
		/// </summary>
		[DataMember(Name = "syslogServer")]
		public string SyslogServer { get; set; }

		/// <summary>
		///    The endpoint
		/// </summary>
		public string Endpoint() => "setting/alert/internalalerts";
	}
}