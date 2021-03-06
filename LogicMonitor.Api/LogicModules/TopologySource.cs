using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A TopologySource
	/// </summary>
	[DataContract]
	public class TopologySource : NamedItem, IHasEndpoint
	{
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
		/// The collection interval in seconds
		/// </summary>
		[DataMember(Name = "collectInterval")]
		public int CollectionIntervalSeconds { get; set; }

		/// <summary>
		/// The collector attribute
		/// </summary>
		[DataMember(Name = "collectorAttribute")]
		public CollectorAttribute CollectorAttribute { get; set; }

		/// <summary>
		/// The Group name
		/// </summary>
		[DataMember(Name = "group")]
		public string Group { get; set; }

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
		/// ToString override
		/// </summary>
		/// <returns>'Id : Name - DisplayedAs'</returns>
		public override string ToString() => $"{Id} : {Name}";

		/// <summary>
		///    The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "setting/topologysources";
	}
}