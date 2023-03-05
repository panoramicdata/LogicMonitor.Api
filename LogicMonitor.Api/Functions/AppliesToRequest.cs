namespace LogicMonitor.Api.Functions;

[DataContract]
internal class AppliesToRequest
{
	[DataMember(Name = "type")]
	public string Type { get; set; } = "testAppliesTo";

	[DataMember(Name = "originalAppliesTo")]
	public string OriginalAppliesTo { get; set; } = string.Empty;

	[DataMember(Name = "currentAppliesTo")]
	public string CurrentAppliesTo { get; set; } = string.Empty;
}
