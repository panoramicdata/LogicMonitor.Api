namespace LogicMonitor.Api.Websites;

/// <summary>
/// Website method
/// </summary>
public enum WebsiteMethod
{
	/// <summary>
	/// Unknown website method
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	/// Table driven method
	/// </summary>
	[EnumMember(Value = "tabledriven")]
	TableDriven = 1,

	/// <summary>
	/// Script-driven method
	/// </summary>
	[EnumMember(Value = "script")]
	Script = 2
}
