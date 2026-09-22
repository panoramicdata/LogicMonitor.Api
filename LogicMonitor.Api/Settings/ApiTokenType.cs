namespace LogicMonitor.Api.Settings;

/// <summary>
/// An API token type
/// </summary>
[DataContract]
public enum ApiTokenType
{
	/// <summary>
	/// Unknown
	/// </summary>
	Unknown = 0,

	/// <summary>
	/// Disabled
	/// </summary>
	[EnumMember(Value = "bearer")]
	Bearer = 1,

	/// <summary>
	/// Agentic (AI agent access, e.g. the MCP gateway)
	/// </summary>
	[EnumMember(Value = "agentic")]
	Agentic = 2,
}
