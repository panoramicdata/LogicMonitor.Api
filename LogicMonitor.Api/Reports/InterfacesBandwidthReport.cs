using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports;

/// <summary>
/// An interfaces bandwidth report
/// </summary>
[DataContract]
public class InterfacesBandwidthReport : DateRangeReport
{
	/// <summary>
	/// The hostsVal
	/// </summary>
	[DataMember(Name = "hostsVal")]
	public string HostsVal { get; set; }

	/// <summary>
	/// The hostsValType
	/// </summary>
	[DataMember(Name = "hostsValType")]
	public string HostsValType { get; set; }

	/// <summary>
	/// The rowFormat
	/// </summary>
	[DataMember(Name = "rowFormat")]
	public int RowFormat { get; set; }
}
