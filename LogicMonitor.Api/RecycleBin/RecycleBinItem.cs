using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.RecycleBin
{
	/// <summary>
	/// A recycle bin item
	/// </summary>
	[DataContract]
	public class RecycleBinItem : IHasEndpoint
	{
		/// <summary>
		/// The ID
		/// </summary>
		[DataMember(Name = "id")]
		public string Id { get; set; }

		/// <summary>
		/// The Resource Type
		/// </summary>
		[DataMember(Name = "resourceType")]
		public string ResourceType { get; set; }

		/// <summary>
		/// The deleted resource's name
		/// </summary>
		[DataMember(Name = "resourceName")]
		public string ResourceName { get; set; }

		/// <summary>
		/// When it was deleted in milliseconds since the Epoch
		/// </summary>
		[DataMember(Name = "deletedOn")]
		public long DeletedOnMs { get; set; }

		/// <summary>
		/// When it was deleted in seconds since the Epoch
		/// </summary>
		public long DeletedOnSeconds => DeletedOnMs / 1000;

		/// <summary>
		/// Who deleted it
		/// </summary>
		[DataMember(Name = "deletedBy")]
		public string DeletedBy { get; set; }

		/// <summary>
		/// The Resource ID
		/// </summary>
		[DataMember(Name = "resourceId")]
		public int ResourceId { get; set; }

		/// <summary>
		/// The paths
		/// </summary>
		[DataMember(Name = "paths")]
		public List<string> Paths { get; set; }

		/// <summary>
		/// The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "recyclebin/recycles";
	}
}