using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// Used to serialise the version for a POST to update a logic module, with the audited version
	/// </summary>
	[DataContract]
	public class LogicModuleUpdateVersion
	{
		/// <summary>
		/// The version. This is an epoch timestamp
		/// </summary>
		[DataMember(Name = "version")]
		public long Version { get; set; }
	}
}
