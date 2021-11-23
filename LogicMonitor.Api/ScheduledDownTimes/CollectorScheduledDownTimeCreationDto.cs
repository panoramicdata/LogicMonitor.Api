using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
///    Collector SDT creation DTO
/// </summary>
public class CollectorScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
{
	/// <summary>
	///    Constructor
	/// </summary>
	/// <param name="collectorId"></param>
	public CollectorScheduledDownTimeCreationDto(int collectorId) : base(ScheduledDownTimeType.Collector)
	{
		CollectorId = collectorId;
	}

	/// <summary>
	///    The collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int CollectorId { get; set; }
}
