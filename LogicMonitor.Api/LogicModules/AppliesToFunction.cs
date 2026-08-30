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
		Code = BuildCode(cidr, networkParts, bitsInteger);
	}

	/// <summary>
	/// Builds the AppliesTo expression matching the fourth-octet pattern for the given netmask.
	/// </summary>
	private static string BuildCode(string cidr, List<int> networkParts, int bitsInteger)
	{
		var hostPattern = BuildHostPattern(cidr, networkParts, bitsInteger);
		return $"join(system.ips, \",\") =~ \"(^|,){networkParts[0]}\\\\.{networkParts[1]}\\\\.{hostPattern}(,|$)\"";
	}

	/// <summary>
	/// Builds the part of the expression from the third octet onwards, which is what varies
	/// between the supported netmasks.
	/// </summary>
	private static string BuildHostPattern(string cidr, List<int> networkParts, int bitsInteger)
	{
		// A /23 spans two adjacent third octets, each with any host
		if (bitsInteger == 23)
		{
			return $"({networkParts[2]}|{networkParts[2] + 1})\\\\.\\\\d+";
		}

		// A /24 is a single third octet with any host
		if (bitsInteger == 24)
		{
			return $"{networkParts[2]}\\\\.\\\\d+";
		}

		// A /32 is one exact address
		if (bitsInteger == 32)
		{
			return $"{networkParts[2]}\\\\.{networkParts[3]}";
		}

		if (bitsInteger is >= 25 and <= 31)
		{
			// NB the range size is (2 << (32 - bits)) - 1, which is twice the number of addresses
			// the netmask actually covers. Preserved as-is: changing it would change which
			// resources existing AppliesTo functions match.
			var numbers = Enumerable.Range(networkParts[3], (2 << (32 - bitsInteger)) - 1).ToList();
			return $"{networkParts[2]}\\\\.({string.Join("|", numbers)})";
		}

		// TODO - support other netmasks
		throw new NotSupportedException($"Network: {cidr} netmask {bitsInteger} not supported.");
	}
}
