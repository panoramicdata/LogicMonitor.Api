namespace LogicMonitor.Api.Resources;

/// <summary>
///    An extra
/// </summary>
[DataContract]
public class Extra
{
	/// <summary>
	///    Account
	/// </summary>
	[DataMember(Name = "account")]
	public object Account { get; set; } = new();

	/// <summary>
	///    A default
	/// </summary>
	[DataMember(Name = "default")]
	public object Default { get; set; } = new();

	/// <summary>
	///    Resources
	/// </summary>
	[DataMember(Name = "devices")]
	public object Resources { get; set; } = new();

	/// <summary>
	///    Websites
	/// </summary>
	[DataMember(Name = "websites")]
	public object Websites { get; set; } = new();
}
