using LogicMonitor.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	/// API Token
	/// </summary>
	[DataContract]
	public class ApiToken : IdentifiedItem
	{
		/// <summary>
		/// The access ID
		/// </summary>
		[DataMember(Name = "accessId")]
		public string AccessId { get; set; }

		/// <summary>
		/// The access key
		/// </summary>
		[DataMember(Name = "accessKey")]
		public string AccessKey { get; set; }

		/// <summary>
		/// The user id
		/// </summary>
		[DataMember(Name = "adminId")]
		public int UserId { get; set; }

		/// <summary>
		/// The username
		/// </summary>
		[DataMember(Name = "adminName")]
		public string UserName { get; set; }

		/// <summary>
		/// The roles
		/// </summary>
		[DataMember(Name = "roles")]
		public List<string> Roles { get; set; }

		/// <summary>
		/// The status
		/// </summary>
		[DataMember(Name = "status")]
		public ApiTokenStatus Status { get; set; }

		/// <summary>
		/// A note
		/// </summary>
		[DataMember(Name = "note")]
		public string Note { get; set; }

		/// <summary>
		/// Who created it
		/// </summary>
		[DataMember(Name = "createdBy")]
		public string CreatedBy { get; set; }

		/// <summary>
		/// The status
		/// </summary>
		[DataMember(Name = "createdOn")]
		public long CreatedOnSeconds { get; set; }

		/// <summary>
		/// The number of seconds since the Epoch that it was last sued
		/// </summary>
		[DataMember(Name = "lastUsedOn")]
		public long LastUsedOnSeconds { get; set; }

		/// <summary>
		/// The user permission
		/// </summary>
		[DataMember(Name = "userPermission")]
		public string UserPermission { get; set; }

		/// <summary>
		///    The DateTime the user accepted the EULA UTC
		/// </summary>
		[IgnoreDataMember]
		public DateTime CreatedOnUtc => CreatedOnSeconds.ToDateTimeUtc();

		/// <summary>
		///    The last user action time in UTC
		/// </summary>
		[IgnoreDataMember]
		public DateTime? LastActionOnUtc => LastUsedOnSeconds.ToNullableDateTimeUtc();
	}
}