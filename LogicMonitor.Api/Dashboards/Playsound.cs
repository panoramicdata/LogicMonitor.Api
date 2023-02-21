namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A PlaySound
/// </summary>
[DataContract]
public class PlaySound
{
	/// <summary>
	/// Whether alerts should play a sound
	/// </summary>
	[DataMember(Name = "shouldPlay")]
	public bool ShouldPlay { get; set; }

	/// <summary>
	/// The critical Alert audio fileName
	/// </summary>
	[DataMember(Name = "criticalAlertAudioFileName")]
	public string CriticalAlertAudioFileName { get; set; }

	/// <summary>
	/// The error Alert audio fileName
	/// </summary>
	[DataMember(Name = "errorAlertAudioFileName")]
	public string ErrorAlertAudioFileName { get; set; }

	/// <summary>
	/// The warning Alert audio fileName
	/// </summary>
	[DataMember(Name = "warningAlertAudioFileName")]
	public string WarningAlertAudioFileName { get; set; }
}
