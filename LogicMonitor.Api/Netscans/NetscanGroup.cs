namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A Netscan Policy
/// </summary>
[DataContract]
public class NetscanGroup : NamedItem, IHasEndpoint
{
	/// <summary>
	///    The subUrl for setting by id
	/// </summary>
	public string Endpoint() => "setting/netscans/groups";
}
