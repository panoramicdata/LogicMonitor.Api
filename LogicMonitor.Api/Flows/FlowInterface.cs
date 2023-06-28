namespace LogicMonitor.Api.Flows;

/// <summary>
/// A flow interface
/// </summary>
[DataContract]
public class FlowInterface
{
	/// <summary>
	/// Time the last data was observed UTC in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "lastDataTime")]
	public long LastDataDateTimeUtcSeconds { get; set; }

	/// <summary>
	/// Time the last data was observed UTC in seconds since the Epoch
	/// </summary>
	[IgnoreDataMember]
	public DateTime LastDataDateTimeUtc => LastDataDateTimeUtcSeconds.ToDateTimeUtc();

	/// <summary>
	/// The index of the interface on the device with ID DeviceId.
	/// </summary>
	[DataMember(Name = "index")]
	public int Index { get; set; }

	/// <summary>
	/// Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;
}
