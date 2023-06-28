namespace LogicMonitor.Api.Devices;

/// <summary>
/// An inherited item
/// </summary>
[DataContract]
public class InheritedItem
{
	/// <summary>
	///    The type
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	///    The type
	/// </summary>
	[DataMember(Name = "type")]
	public MonitoredObjectType Type { get; set; }

	/// <summary>
	///    The id
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	///    The full path
	/// </summary>
	[DataMember(Name = "fullPath")]
	public string FullPath { get; set; } = string.Empty;

	/// <summary>
	///    The value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;
}
