using LogicMonitor.Api.Devices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A DeviceDataSourceInstance
	/// </summary>
	[DataContract]
	[DebuggerDisplay("{Name}={Value}")]
	public class DeviceDataSourceInstanceProperty : StringIdentifiedItem
	{
		/// <summary>
		/// Name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Type
		/// </summary>
		[DataMember(Name = "type")]
		public PropertyType Type { get; set; }

		/// <summary>
		/// Value
		/// </summary>
		[DataMember(Name = "value")]
		public string Value { get; set; }

		/// <summary>
		///    The list of inherited items
		/// </summary>
		[DataMember(Name = "inheritList")]
		public List<InheritedItem> InheritList { get; set; }
	}
}