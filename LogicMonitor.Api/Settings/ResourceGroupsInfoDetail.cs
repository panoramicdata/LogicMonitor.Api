namespace LogicMonitor.Api.Settings;

/// <summary>
///     ResourceGroups info detail
/// </summary>
[DataContract]
public class ResourceGroupsInfoDetail
{
	/// <summary>
	///     The ResourceGroup count
	/// </summary>
	[DataMember(Name = "size")]
	public int Count { get; set; }

	/// <summary>
	///     The Resource Group property count
	/// </summary>
	[DataMember(Name = "props")]
	public int PropertyCount { get; set; }
}

