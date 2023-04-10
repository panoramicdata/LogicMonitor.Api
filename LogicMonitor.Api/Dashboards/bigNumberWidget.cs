namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A big number widget
/// </summary>
[DataContract]
public class BigNumberWidget : Widget, IWidget
{
	/// <summary>
	///     The alert filter
	/// </summary>
	[DataMember(Name = "bigNumberInfo")]
	public BigNumberInfo BigNumberInfo { get; set; } = new();

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
