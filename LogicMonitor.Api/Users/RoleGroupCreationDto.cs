using System.Runtime.Serialization;

namespace LogicMonitor.Api.Users
{
	/// <summary>
	/// RoleGroup creation DTO
	/// </summary>
	[DataContract]
	public class RoleGroupCreationDto : CreationDto<RoleGroup>
	{
		/// <summary>
		/// Name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Description
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }
	}
}