using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports;

/// <summary>
/// A report recipient
/// </summary>
[DataContract]
public class Recipient
{
	/// <summary>
	/// The recipient type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	/// The receipt method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; }

	/// <summary>
	/// The address
	/// </summary>
	[DataMember(Name = "addr")]
	public string Address { get; set; }

	/// <summary>
	/// The additional information
	/// </summary>
	[DataMember(Name = "additionInfo")]
	public string AdditionInfo { get; set; }
}
