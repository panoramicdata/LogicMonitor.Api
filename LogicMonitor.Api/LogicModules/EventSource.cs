using LogicMonitor.Api.Alerts;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// An EventSource
	/// </summary>
	[DataContract]
	public class EventSource : NamedItem, IHasEndpoint
	{
		/// <summary>
		/// The alert subject template
		/// </summary>
		[DataMember(Name = "alertSubjectTemplate")]
		public string AlertBodyTemplate { get; set; }

		/// <summary>
		/// The Alert effective interval
		/// </summary>
		[DataMember(Name = "alertEffectiveIval")]
		public int AlertEffectiveIntervalMinutes { get; set; }

		/// <summary>
		/// The alert body template
		/// </summary>
		[DataMember(Name = "alertBodyTemplate")]
		public string AlertSubjectTemplate { get; set; }

		/// <summary>
		/// The alert level
		/// </summary>
		[DataMember(Name = "alertLevel")]
		public AlertLevel AlertLevel { get; set; }

		/// <summary>
		/// What this applies to
		/// </summary>
		[DataMember(Name = "appliesTo")]
		public string AppliesTo { get; set; }

		/// <summary>
		/// The audit version
		/// </summary>
		[DataMember(Name = "auditVersion")]
		public int? AuditVersion { get; set; }

		/// <summary>
		/// The check interval in seconds.  Applies to IPMI events
		/// </summary>
		[DataMember(Name = "checkInterval")]
		public int? CheckIntervalSeconds { get; set; }

		/// <summary>
		/// The checksum
		/// </summary>
		[DataMember(Name = "checksum")]
		public string CheckSum { get; set; }

		/// <summary>
		/// Whether to clear after acknowledgement
		/// </summary>
		[DataMember(Name = "clearAfterAck")]
		public bool ClearAfterAcknowledgement { get; set; }

		/// <summary>
		/// The collection method
		/// </summary>
		[DataMember(Name = "collector")]
		public EventSourceType EventSourceType { get; set; }

		/// <summary>
		/// The filters
		/// </summary>
		[DataMember(Name = "filters")]
		public List<EventSourceFilter> Filters { get; set; }

		/// <summary>
		/// GroovyScript
		/// </summary>
		[DataMember(Name = "groovyScript")]
		public string GroovyScript { get; set; }

		/// <summary>
		/// The Group name
		/// </summary>
		[DataMember(Name = "group")]
		public string Group { get; set; }

		/// <summary>
		/// Linux command line
		/// </summary>
		[DataMember(Name = "linuxCmdline")]
		public string LinuxCommandLine { get; set; }

		/// <summary>
		/// Log files
		/// </summary>
		[DataMember(Name = "logFiles")]
		public List<EventSourceLogFile> LogFiles { get; set; }

		/// <summary>
		/// Linux script
		/// </summary>
		[DataMember(Name = "linuxScript")]
		public string LinuxScript { get; set; }

		/// <summary>
		/// Linux script
		/// </summary>
		[DataMember(Name = "lineageId")]
		public string LineageId { get; set; }

		/// <summary>
		/// Published
		/// </summary>
		[DataMember(Name = "published")]
		public int Published { get; set; }

		/// <summary>
		/// The schedule
		/// </summary>
		[DataMember(Name = "schedule")]
		public int? Schedule { get; set; }

		/// <summary>
		/// Script type
		/// </summary>
		[DataMember(Name = "scriptType")]
		public EventSourceScriptType ScriptType { get; set; }

		/// <summary>
		/// Suppress duplicates
		/// </summary>
		[DataMember(Name = "suppressDuplicatesES")]
		public bool SuppressDuplicates { get; set; }

		/// <summary>
		/// Tags
		/// </summary>
		[DataMember(Name = "tags")]
		public string Tags { get; set; }

		/// <summary>
		/// Technology
		/// </summary>
		[DataMember(Name = "technology")]
		public string Technology { get; set; }

		/// <summary>
		/// The version
		/// </summary>
		[DataMember(Name = "version")]
		public int? Version { get; set; }

		/// <summary>
		/// Windows command line
		/// </summary>
		[DataMember(Name = "windowsCmdline")]
		public string WindowsCommandLine { get; set; }

		/// <summary>
		/// Windows script
		/// </summary>
		[DataMember(Name = "windowsScript")]
		public string WindowsScript { get; set; }

		/// <summary>
		/// ToString override
		/// </summary>
		/// <returns>'Id : Name - DisplayedAs'</returns>
		public override string ToString() => $"{Id} : {Name}";

		/// <summary>
		///    The endpoint
		/// </summary>
		public string Endpoint() => "setting/eventsources";
	}
}