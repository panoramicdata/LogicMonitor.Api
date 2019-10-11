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
		/// The permission
		/// </summary>
		[DataMember(Name = "category")]
		public LogicModuleUpdateCategory Category { get; set; }

		/// <summary>
		/// The active instance count
		/// </summary>
		[DataMember(Name = "activeInstances")]
		public int? ActiveInstanceCount { get; set; }

		/// <summary>
		/// The associated device count
		/// </summary>
		[DataMember(Name = "associatedDevices")]
		public int? AssociatedDeviceCount { get; set; }

		/// <summary>
		/// The description
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		/// <summary>
		/// The locator
		/// </summary>
		[DataMember(Name = "locator")]
		public string Locator { get; set; }

		/// <summary>
		/// The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// The displayName
		/// </summary>
		[DataMember(Name = "displayName")]
		public string DisplayName { get; set; }

		/// <summary>
		/// The lineage id
		/// </summary>
		[DataMember(Name = "lineageid")]
		public string LineageId { get; set; }

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
		/// The registryVersion
		/// </summary>
		[DataMember(Name = "registryVersion")]
		public string RegistryVersion { get; set; }

		/// <summary>
		/// The type
		/// </summary>
		[DataMember(Name = "type")]
		public LogicModuleType Type { get; set; }

		/// <summary>
		/// The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "setting/registry/listcore";
	}
}