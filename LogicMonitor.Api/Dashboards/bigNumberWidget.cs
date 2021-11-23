using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A big number widget
/// </summary>
[DataContract]
public class BigNumberWidget : Widget
{
	/// <summary>
	///     The alert filter
	/// </summary>
	[DataMember(Name = "bigNumberInfo")]
	public BigNumberInfo BigNumberInfo { get; set; }

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public object DisplaySettings { get; set; }
}
