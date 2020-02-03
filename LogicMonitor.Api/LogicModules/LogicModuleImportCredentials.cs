using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// The LogicModule import credentials
	/// </summary>
	[DataContract]
	public class LogicModuleImportCredentials
	{
		/// <summary>
		/// The server address
		/// </summary>
		[DataMember(Name = "coreserver")]
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
