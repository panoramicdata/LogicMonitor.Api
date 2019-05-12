using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports
{
	/// <summary>
	/// User report user filter
	/// </summary>
	[DataContract]
	public class UserReportUserFilter
	{
		/// <summary>
		/// The status
		/// </summary>
		[DataMember(Name = "status")]
		public string Status { get; set; }

		/// <summary>
		/// The enable2fa
		/// </summary>
		[DataMember(Name = "enable2fa")]
		public string Enable2fa { get; set; }

		/// <summary>
		/// The apiOnlyUser
		/// </summary>
		[DataMember(Name = "apiOnlyUser")]
		public string ApiOnlyUser { get; set; }

		/// <summary>
		/// The roleAssignment
		/// </summary>
		[DataMember(Name = "roleAssignment")]
		public string RoleAssignment { get; set; }

		/// <summary>
		/// The firstName
		/// </summary>
		[DataMember(Name = "firstName")]
		public string FirstName { get; set; }

		/// <summary>
		/// The lastName
		/// </summary>
		[DataMember(Name = "lastName")]
		public string LastName { get; set; }

		/// <summary>
		/// The email
		/// </summary>
		[DataMember(Name = "email")]
		public string Email { get; set; }

		/// <summary>
		/// The username
		/// </summary>
		[DataMember(Name = "username")]
		public string Username { get; set; }
	}
}