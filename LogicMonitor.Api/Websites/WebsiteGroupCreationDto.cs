using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites
{
	/// <summary>
	/// A ServiceGroup creation DTO
	/// </summary>
	[DataContract]
	public class WebsiteGroupCreationDto : CreationDto<WebsiteGroup>
	{
		/// <summary>
		/// User permission
		/// </summary>
		[DataMember(Name = "parentId")]
		public string ParentId { get; set; }

		/// <summary>
		/// Whether monitoring is disabled
		/// </summary>
		[DataMember(Name = "stopMonitoring")]
		public bool IsMonitoringDisabled { get; set; }

		/// <summary>
		/// Whether alerting is disabled.
		/// </summary>
		[DataMember(Name = "disableAlerting")]
		public bool IsAlertingDisabled { get; set; }

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
		/// The parent group
		/// </summary>
		[DataMember(Name = "parentGroup")]
		public string ParentGroupFullPath { get; set; }

		/// <summary>
		/// The properties
		/// </summary>
		[DataMember(Name = "properties")]
		public List<Property> Properties { get; set; }
	}
}