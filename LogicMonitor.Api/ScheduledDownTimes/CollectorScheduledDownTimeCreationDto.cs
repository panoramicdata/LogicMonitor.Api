namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Collector SDT creation DTO
/// </summary>
/// <remarks>
///    Constructor
/// </remarks>
/// <param name="collectorId"></param>
public class CollectorScheduledDownTimeCreationDto(int collectorId) : ScheduledDownTimeCreationDto(ScheduledDownTimeType.Collector)
{

	/// <summary>
	///    The collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int CollectorId { get; set; } = collectorId;
}
