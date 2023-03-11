namespace LogicMonitor.Api;

/// <summary>
/// Used to provide deprecation information
/// </summary>
[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class DeprecatedAttribute : Attribute
{
	/// <summary>
	/// The preferred alternative
	/// </summary>
	public string Preferred { get; set; } = string.Empty;

	/// <summary>
	/// Notes
	/// </summary>
	public string Notes { get; set; } = string.Empty;
}
