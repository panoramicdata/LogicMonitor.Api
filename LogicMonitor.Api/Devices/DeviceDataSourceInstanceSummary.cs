using System.Runtime.Serialization;

namespace LogicMonitor.Api.Devices
{
	/// <summary>
	/// A Device DataSource instance
	/// </summary>
	[DataContract(Name = "deviceDataSourceInstance")]
	public class DeviceDataSourceInstanceSummary : NamedItem
	{
		/// <summary>
		/// The alias
		/// </summary>
		[DataMember]
		public string Alias { get; set; }
	}
}