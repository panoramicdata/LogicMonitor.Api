using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites
{
	/// <summary>
	/// A website step authentication
	/// </summary>
	[DataContract]
	public class WebsiteStepAuthentication
	{
		/// <summary>
		/// The type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; }

		/// <summary>
		/// The user name
		/// </summary>
		[DataMember(Name = "userName")]
		public string UserName { get; set; }

		/// <summary>
		/// The password
		/// </summary>
		[DataMember(Name = "password")]
		public string Password { get; set; }
	}
}