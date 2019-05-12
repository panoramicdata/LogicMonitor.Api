using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// An instance level property
	/// </summary>
	[DataContract]
	public class InstanceLevelProperty
	{
		/// <summary>
		/// The LM name
		/// </summary>
		[DataMember(Name = "lmName")]
		public string LogicMonitorName { get; set; }

		/// <summary>
		/// The method
		/// </summary>
		[DataMember(Name = "method")]
		public string Method { get; set; }

		/// <summary>
		/// The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// The OID
		/// </summary>
		[DataMember(Name = "oid")]
		public string Oid { get; set; }

		/// <summary>
		/// The WMI name
		/// </summary>
		[DataMember(Name = "wmiName")]
		public string WmiName { get; set; }
	}
}