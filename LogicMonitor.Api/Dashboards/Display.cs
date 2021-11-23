namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A display
/// </summary>
[DataContract]
public class Display
{
	/// <summary>
	/// The option
	/// </summary>
	[DataMember(Name = "option")]
	public string Option { get; set; }

	/// <summary>
	/// The legend
	/// </summary>
	[DataMember(Name = "legend")]
	public string Legend { get; set; }

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public DisplayType Type { get; set; }

	/// <summary>
	/// The color
	/// </summary>
	[DataMember(Name = "color")]
	public string Color { get; set; }
}
