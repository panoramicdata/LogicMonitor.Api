using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A big number widget creation DTO
/// </summary>
[DataContract]
public class BigNumberWidgetCreationDto : WidgetCreationDto<BigNumberWidget>
{
	/// <inheritdoc />
	public override string Type { get; } = "bigNumber";

	/// <summary>
	/// The Big Number info
	/// </summary>
	[DataMember(Name = "bigNumberInfo")]
	public BigNumberInfo BigNumberInfo { get; set; }
}
