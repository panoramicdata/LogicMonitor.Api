namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An AppliesTo Function
/// </summary>
[DataContract]
public class SnmpSysOidMap : LogicModule, IHasEndpoint
{
	/// <summary>
	/// The categories
	/// </summary>
	[DataMember(Name = "categories")]
	public string Categories { get; set; } = string.Empty;

	/// <summary>
	/// The parameters
	/// </summary>
	[DataMember(Name = "oid")]
	public string Oid { get; set; } = string.Empty;


	/// <summary>
	/// Published
	/// </summary>
	[DataMember(Name = "published")]
	public int Published { get; set; }

	/// <summary>
	/// ToString override
	/// </summary>
	/// <returns>'Id : Oid'</returns>
	public override string ToString() => $"{Id} : {Oid}";

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/oids";
}
