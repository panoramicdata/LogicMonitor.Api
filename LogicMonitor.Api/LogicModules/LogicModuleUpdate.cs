using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A LogicModule Update
	/// </summary>
	[DataContract]
	public class LogicModuleUpdate : IHasEndpoint
	{
		/// <summary>
		/// The local ID
		/// </summary>
		[DataMember(Name = "localId")]
		public int LocalId { get; set; }

		/// <summary>
		/// The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// The appliesTo
		/// </summary>
		[DataMember(Name = "appliesTo")]
		public string AppliesTo { get; set; }

		/// <summary>
		/// The permission
		/// </summary>
		[DataMember(Name = "category")]
		public LogicModuleUpdateCategory Category { get; set; }

		/// <summary>
		/// The type
		/// </summary>
		[DataMember(Name = "type")]
		public LogicModuleType Type { get; set; }

		/// <summary>
		/// The collection method
		/// </summary>
		[DataMember(Name = "collectionMethod")]
		public string CollectionMethod { get; set; }

		/// <summary>
		/// The description
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		/// <summary>
		/// The group
		/// </summary>
		[DataMember(Name = "group")]
		public string Group { get; set; }

		/// <summary>
		/// The version
		/// </summary>
		[DataMember(Name = "version")]
		public long Version { get; set; }

		/// <summary>
		/// The local version
		/// </summary>
		[DataMember(Name = "localVersion")]
		public long LocalVersion { get; set; }

		/// <summary>
		/// The audit version
		/// </summary>
		[DataMember(Name = "auditVersion")]
		public long AuditVersion { get; set; }

		/// <summary>
		/// The rest LM (?)
		/// </summary>
		[DataMember(Name = "restLm")]
		public string RestLm { get; set; }

		/// <summary>
		/// The registryVersion
		/// </summary>
		[DataMember(Name = "registryVersion")]
		public string RegistryVersion { get; set; }

		/// <summary>
		/// The publish time
		/// </summary>
		[DataMember(Name = "publishedAt")]
		public long PublishedAtMilliseconds { get; set; }

		/// <summary>
		/// The quality
		/// </summary>
		[DataMember(Name = "quality")]
		public string Quality { get; set; }

		/// <summary>
		/// The locator
		/// </summary>
		[DataMember(Name = "locator")]
		public string Locator { get; set; }

		/// <summary>
		/// The currentUuid
		/// </summary>
		[DataMember(Name = "currentUuid")]
		public string CurrentUuid { get; set; }

		/// <summary>
		/// The namespace
		/// </summary>
		[DataMember(Name = "namespace")]
		public string Namespace { get; set; }

		/// <summary>
		/// The local version
		/// </summary>
		[DataMember(Name = "local")]
		public string Local { get; set; }

		/// <summary>
		/// The remote version
		/// </summary>
		[DataMember(Name = "remote")]
		public string Remote { get; set; }

		/// <summary>
		/// The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "setting/logicmodules/listcore";
	}
}