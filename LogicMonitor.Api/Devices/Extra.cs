namespace LogicMonitor.Api.Devices;

/// <summary>
///    An extra
/// </summary>
[DataContract]
public class Extra
{
	/// <summary>
	///    Devices
	/// </summary>
	[DataMember(Name = "account")]
	public object Account { get; set; } = new();

	/// <summary>
	///    A default
	/// </summary>
	[DataMember(Name = "default")]
	public object Default { get; set; } = new();

	/// <summary>
	///    Devices
	/// </summary>
	[DataMember(Name = "devices")]
	public object Devices { get; set; } = new();

	/// <summary>
	///    Devices
	/// </summary>
	[DataMember(Name = "websites")]
	public object Websites { get; set; } = new();
}
