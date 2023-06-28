namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A collector status
/// </summary>
[DataContract]
public class CollectorStatus
{
	/// <summary>
	///    The status
	/// </summary>
	[DataMember(Name = "status")]
	public int Status { get; set; }

	/// <summary>
	///    Whether isDown
	/// </summary>
	[DataMember(Name = "isDown")]
	public bool IsDown { get; set; }

	/// <summary>
	///    Whether acked
	/// </summary>
	[DataMember(Name = "acked")]
	public bool Acked { get; set; }

	/// <summary>
	///    Whether inSDT
	/// </summary>
	[DataMember(Name = "inSDT")]
	public bool InSDT { get; set; }
}
