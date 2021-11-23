namespace LogicMonitor.Api;

/// <summary>
/// Custom properties
/// </summary>
public interface IHasCustomProperties
{
	/// <summary>
	/// Custom properties
	/// </summary>
	List<Property> CustomProperties { get; set; }
}
