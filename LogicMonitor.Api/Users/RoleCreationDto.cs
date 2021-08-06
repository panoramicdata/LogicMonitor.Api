using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Users
{
	/// <summary>
	/// Role creation DTO
	/// </summary>
	[DataContract]
	public class RoleCreationDto : CreationDto<Role>
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

		/// <summary>
		/// Whether EULA acceptance is required
		/// </summary>
		[DataMember(Name = "requireEULA")]
		public bool RequireEULA { get; set; }

		/// <summary>
		/// Whether two factor authentication is required
		/// </summary>
		[DataMember(Name = "twoFARequired")]
		public bool TwoFactorAuthenticationRequired { get; set; }

		/// <summary>
		/// Custom Help label
		/// </summary>
		[DataMember(Name = "customHelpLabel")]
		public string CustomHelpLabel { get; set; }

		/// <summary>
		/// Custom Help URL
		/// </summary>
		[DataMember(Name = "customHelpURL")]
		public string CustomHelpUrl { get; set; }

		/// <summary>
		/// Privileges
		/// </summary>
		[DataMember(Name = "privileges")]
		public List<RolePrivilege> Privileges { get; set; }

		/// <summary>
		/// Role Group ID
		/// </summary>
		[DataMember(Name = "roleGroupId")]
		public int RoleGroupId { get; set; } = 1;
	}
}