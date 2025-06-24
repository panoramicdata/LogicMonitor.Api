namespace LogicMonitor.Api.Time;

/// <summary>
/// This enum represents the time periods available for raw data of a Website Checkpoint
/// </summary>
[DataContract]
public enum WebsiteCheckpointRawDataTimePeriod
{
	/// <summary>
	/// One hour
	/// </summary>
	OneHour = 1,

	/// <summary>
	/// Two hours
	/// </summary>
	TwoHours = 2,

	/// <summary>
	/// Three hours
	/// </summary>
	ThreeHours = 3,

	/// <summary>
	/// Four hours
	/// </summary>
	FourHours = 4,

	/// <summary>
	/// Five hours
	/// </summary>
	FiveHours = 5,

	/// <summary>
	/// Six hours
	/// </summary>
	SixHours = 6,

	/// <summary>
	/// Seven hours
	/// </summary>
	SevenHours = 7,

	/// <summary>
	/// Eight hours
	/// </summary>
	EightHours = 8
}