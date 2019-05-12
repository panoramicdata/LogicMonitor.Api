using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// An Alerting Disabled on class
	/// </summary>
	[DataContract]
	public class AlertingDisabledOn : IdentifiedItem
	{
		/// <summary>
		/// The display name
		/// </summary>
		[DataMember(Name = "displayName")]
		public string DisplayName { get; set; }

		/// <summary>
		/// The type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; }

		/// <summary>
		/// The user permission
		/// </summary>
		[DataMember(Name = "userPermission")]
		public UserPermission UserPermission { get; set; }
	}
}