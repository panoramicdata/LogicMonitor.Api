namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Widget data for the NOC widget type
/// </summary>
[DataContract]
public class NOCWidgetData : WidgetData
{
	/// <summary>
	/// Whether the widget is ack checked
	/// </summary>
	[DataMember(Name = "ackChecked")]
	public bool AckChecked { get; set; }

	/// <summary>
	/// Whether scheduled downtime has been checked
	/// </summary>
	[DataMember(Name = "sdtChecked")]
	public bool SdtChecked { get; set; }

	/// <summary>
	/// The data items to be reported on
	/// </summary>
	[DataMember(Name = "items")]
	public List<NOCWidgetDataItem> Items { get; set; } = [];
}

