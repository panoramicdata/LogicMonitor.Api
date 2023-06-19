namespace LogicMonitor.Api.Settings;

/// <summary>
///     An integration
/// </summary>
[DataContract]
[JsonConverter(typeof(IntegrationsConverter))]
[DebuggerDisplay("{Type}:{Name}")]
public class Integration : NamedItem, IHasEndpoint
{
	/// <summary>
	///     The integration type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	///     Extra configuration
	/// </summary>
	[DataMember(Name = "extra")]
	public string Extra { get; set; } = string.Empty;

	/// <summary>
	///     The endpoint
	/// </summary>
	public string Endpoint() => "setting/integrations";
}
