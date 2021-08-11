using System.Runtime.Serialization;

namespace LogicMonitor.Api.Devices
{
	/// <summary>
	/// A TreeNodeFreeSearchResult
	/// </summary>
	[DataContract]
	public class TreeNodeFreeSearchResult
	{
		/// <summary>
		/// The Entity Id
		/// </summary>
		[DataMember(Name = "entityId")]
		public int EntityId { get; set; }

		/// <summary>
		/// The group Id
		/// </summary>
		[DataMember(Name = "groupId")]
		public int GroupId { get; set; }

		/// <summary>
		/// The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// The description
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		/// <summary>
		/// The result type
		/// </summary>
		[DataMember(Name = "type")]
		public TreeNodeFreeSearchResultType Type { get; set; }

		/// <summary>
		/// The location
		/// </summary>
		[DataMember(Name = "location")]
		public string Location { get; set; }

		/// <summary>
		/// The location
		/// </summary>
		[DataMember(Name = "extraVal")]
		public string ExtraValue { get; set; }

		/// <summary>
		/// The device count
		/// </summary>
		[DataMember(Name = "numOfHosts")]
		public string DeviceCount { get; set; }

		/// <summary>
		/// ToString override
		/// </summary>
		public override string ToString() => $"{Type} \"{Name}\" ({EntityId}){(string.IsNullOrWhiteSpace(Location) ? string.Empty : $" on {Location}")}";
	}
}