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
		/// name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// description
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		/// <summary>
		/// requireEULA
		/// </summary>
		[DataMember(Name = "requireEULA")]
		public bool RequireEULA { get; set; }

		/// <summary>
		/// twoFARequired
		/// </summary>
		[DataMember(Name = "twoFARequired")]
		public bool TwoFactorAuthenticationRequired { get; set; }

		/// <summary>
		/// customHelpLabel
		/// </summary>
		[DataMember(Name = "customHelpLabel")]
		public string CustomHelpLabel { get; set; }

		/// <summary>
		/// customHelpURL
		/// </summary>
		[DataMember(Name = "customHelpURL")]
		public string CustomHelpUrl { get; set; }

		/// <summary>
		/// privileges
		/// </summary>
		[DataMember(Name = "privileges")]
		public List<RolePrivilege> Privileges { get; set; }
	}
}