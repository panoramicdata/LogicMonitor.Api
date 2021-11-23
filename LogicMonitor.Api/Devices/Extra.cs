using System.Runtime.Serialization;

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
	public object Account { get; set; }

	/// <summary>
	///    A default
	/// </summary>
	[DataMember(Name = "default")]
	public object Default { get; set; }

	/// <summary>
	///    Devices
	/// </summary>
	[DataMember(Name = "devices")]
	public object Devices { get; set; }

	/// <summary>
	///    Devices
	/// </summary>
	[DataMember(Name = "websites")]
	public object Websites { get; set; }
}
