using System.Runtime.Serialization;

namespace LogicMonitor.Api.Logging;

/// <summary>
/// A WriteLog error
/// </summary>
[DataContract]
public class WriteLogError
{
	/// <summary>
	/// The error code
	/// </summary>
	[DataMember(Name = "code")]
	public int Code { get; set; }

	/// <summary>
	/// The error message
	/// </summary>
	[DataMember(Name = "error")]
	public string ErrorMessage { get; set; }

	/// <summary>
	/// The originating request
	/// </summary>
	[DataMember(Name = "event")]
	public WriteLogRequest Event { get; set; }
}
