namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An AppliesTo Function
/// </summary>
[DataContract]
public class AppliesToFunction : LogicModule, IHasEndpoint
{
	/// <summary>
	/// The AppliesTo Function code. Note that special characters may need to be escaped.
	/// </summary>
	[DataMember(Name = "code")]
	public string Code { get; set; } = string.Empty;

	/// <summary>
	/// The AppliesTo Function parameters
	/// </summary>
	[DataMember(Name = "params")]
	public string Parameters { get; set; } = string.Empty;

	/// <summary>
	/// Published
	/// </summary>
	[DataMember(Name = "published")]
	public int Published { get; set; }

	/// <summary>
	/// ToString override
	/// </summary>
	/// <returns>'Id : Name - DisplayedAs'</returns>
	public override string ToString() => $"{Id} : {Name}";

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/functions";

	/// <summary>
	/// Set the Code given a CIDR network address (e.g. 10.2.3.0/25)
	/// </summary>
	/// <param name="cidr"></param>
	public void SetCodeFromCidr(string cidr)
	{
		// Get network and mask parts
		var subnetParts = cidr.Split('/');
		if (subnetParts.Length != 2)
		{
			throw new FormatException($"Network: {cidr} is badly formed.  Should contain one slash.");
		}

		var networkPart = subnetParts[0];
		var bitsPart = subnetParts[1];
		// We have the network and bits parts

		// Ensure they are valid
		if (!IPAddress.TryParse(networkPart, out var ipAddress))
		{
			throw new FormatException($"Network: {cidr} is badly formed.  Invalid IPv4 network address before the slash.");
		}

		var networkParts = networkPart.Split('.').Select(int.Parse).ToList();

		if (!int.TryParse(bitsPart, out var bitsInteger) || bitsInteger < 0 || bitsInteger > 32)
		{
			throw new FormatException($"Network: {cidr} is badly formed.  Invalid value after the slash.");
		}

		// Determine the applies function
		switch (bitsInteger)
		{
			case 23:
				Code = $"join(system.ips, \",\") =~ \"(^|,){networkParts[0]}\\\\.{networkParts[1]}\\\\.({networkParts[2]}|{networkParts[2] + 1})\\\\.\\\\d+(,|$)\"";
				return;
			case 24:
				Code = $"join(system.ips, \",\") =~ \"(^|,){networkParts[0]}\\\\.{networkParts[1]}\\\\.{networkParts[2]}\\\\.\\\\d+(,|$)\"";
				return;
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
				var numbers = Enumerable.Range(networkParts[3], (2 << (32 - bitsInteger)) - 1).ToList();
				Code = $"join(system.ips, \",\") =~ \"(^|,){networkParts[0]}\\\\.{networkParts[1]}\\\\.{networkParts[2]}\\\\.({string.Join("|", numbers)})(,|$)\"";
				return;
			case 32:
				Code = $"join(system.ips, \",\") =~ \"(^|,){networkParts[0]}\\\\.{networkParts[1]}\\\\.{networkParts[2]}\\\\.{networkParts[3]}(,|$)\"";
				return;
			default:
				// TODO - support other netmasks
				throw new NotSupportedException($"Network: {cidr} netmask {bitsInteger} not supported.");
		}
	}
}
