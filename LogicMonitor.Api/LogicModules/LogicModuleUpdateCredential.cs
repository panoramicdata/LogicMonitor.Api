using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// The credentials used to get LogicModule update details
	/// </summary>
	[DataContract]
	public class LogicModuleUpdateCredential
	{
		/// <summary>
		/// The server address
		/// </summary>
		[DataMember(Name = "coreServer")]
		public string CoreServer { get; set; }

		/// <summary>
		/// The username
		/// </summary>
		[DataMember(Name = "username")]

		public string Username { get; set; } = "anonymouse";

		/// <summary>
		/// The password
		/// </summary>
		[DataMember(Name = "password")]
		public string Password { get; set; } = "logicmonitor";
	}
}
