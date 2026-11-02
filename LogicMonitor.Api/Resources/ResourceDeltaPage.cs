namespace LogicMonitor.Api.Resources;

/// <summary>
/// A page of resources changed since a delta was started
/// </summary>
[DataContract]
public class ResourceDeltaPage : Page<Resource>
{
	/// <summary>
	/// The delta id to pass to the next delta request
	/// </summary>
	[DataMember(Name = "deltaId")]
	public string DeltaId { get; set; } = string.Empty;
}
