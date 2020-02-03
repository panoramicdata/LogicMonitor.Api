using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A wdiget type
	/// </summary>
	[DataContract]
	public enum WidgetType
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// Noc widget
		/// </summary>
		[EnumMember(Value = "noc")]
		Noc,

		/// <summary>
		/// Noc widget
		/// </summary>
		[EnumMember(Value = "deviceNOC")]
		DeviceNoc,

		/// <summary>
		/// Complex graph widget
		/// </summary>
		[EnumMember(Value = "cgraph")]
		CGraph,

		/// <summary>
		/// Netflow Graph widget
		/// </summary>
		[EnumMember(Value = "ngraph")]
		NGraph,

		/// <summary>
		/// Simple graph widget
		/// </summary>
		[EnumMember(Value = "sgraph")]
		SGraph,

		/// <summary>
		/// Google map widget
		/// </summary>
		[EnumMember(Value = "gmap")]
		GMap,

		/// <summary>
		/// Flash widget
		/// </summary>
		[EnumMember(Value = "flash")]
		Flash,

		/// <summary>
		/// Alert widget
		/// </summary>
		[EnumMember(Value = "alert")]
		Alert,

		/// <summary>
		/// Website individual status widget
		/// </summary>
		[EnumMember(Value = "websiteIndividualStatus")]
		WebsiteIndividualStatus,

		/// <summary>
		/// Website overall status widget
		/// </summary>
		[EnumMember(Value = "websiteOverallStatus")]
		WebsiteOverallStatus,

		/// <summary>
		/// Netflow widget
		/// </summary>
		[EnumMember(Value = "netflow")]
		Netflow,

		/// <summary>
		/// Netflow graph widget
		/// </summary>
		[EnumMember(Value = "netflowGraph")]
		NetflowGraph,

		/// <summary>
		/// Big number widget
		/// </summary>
		[EnumMember(Value = "bigNumber")]
		BigNumber,

		/// <summary>
		/// Dynamic table
		/// </summary>
		[EnumMember(Value = "dynamicTable")]
		DynamicTable,
	}
}