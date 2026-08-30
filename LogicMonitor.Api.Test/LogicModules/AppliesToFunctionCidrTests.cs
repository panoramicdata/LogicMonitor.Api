namespace LogicMonitor.Api.Test.LogicModules;

/// <summary>
/// Portal-free characterisation tests for <see cref="AppliesToFunction.SetCodeFromCidr"/>.
/// </summary>
/// <remarks>
/// The generated code is an AppliesTo expression evaluated by the portal against every resource,
/// so an off-by-one in the host range silently changes which resources a LogicModule applies to.
/// These tests pin the exact generated expression for every supported netmask.
/// </remarks>
public class AppliesToFunctionCidrTests
{
	private static string CodeFor(string cidr)
	{
		var appliesToFunction = new AppliesToFunction();
		appliesToFunction.SetCodeFromCidr(cidr);
		return appliesToFunction.Code;
	}

	#region Rejected input

	[Theory]
	[InlineData("10.2.3.0")]
	[InlineData("10.2.3.0/24/8")]
	public void SetCodeFromCidr_WithoutExactlyOneSlash_Throws(string cidr)
		=> ((Action)(() => CodeFor(cidr))).Should().Throw<FormatException>()
			.WithMessage("*one slash*");

	[Theory]
	[InlineData("not-an-ip/24")]
	[InlineData("999.999.999.999/24")]
	public void SetCodeFromCidr_WithAnInvalidNetworkAddress_Throws(string cidr)
		=> ((Action)(() => CodeFor(cidr))).Should().Throw<FormatException>()
			.WithMessage("*Invalid IPv4 network address*");

	[Theory]
	[InlineData("10.2.3.0/abc")]
	[InlineData("10.2.3.0/-1")]
	[InlineData("10.2.3.0/33")]
	public void SetCodeFromCidr_WithInvalidBits_Throws(string cidr)
		=> ((Action)(() => CodeFor(cidr))).Should().Throw<FormatException>()
			.WithMessage("*Invalid value after the slash*");

	[Theory]
	[InlineData("10.2.3.0/0")]
	[InlineData("10.2.0.0/16")]
	[InlineData("10.2.3.0/22")]
	public void SetCodeFromCidr_WithAnUnsupportedNetmask_Throws(string cidr)
		=> ((Action)(() => CodeFor(cidr))).Should().Throw<NotSupportedException>()
			.WithMessage("*not supported*");

	#endregion

	#region Supported netmasks

	[Fact]
	public void SetCodeFromCidr_WithSlash23_MatchesBothAdjacentThirdOctets()
		=> CodeFor("10.2.4.0/23")
			.Should().Be(@"join(system.ips, "","") =~ ""(^|,)10\\.2\\.(4|5)\\.\\d+(,|$)""");

	[Fact]
	public void SetCodeFromCidr_WithSlash24_MatchesAnyHostInTheThirdOctet()
		=> CodeFor("10.2.3.0/24")
			.Should().Be(@"join(system.ips, "","") =~ ""(^|,)10\\.2\\.3\\.\\d+(,|$)""");

	[Fact]
	public void SetCodeFromCidr_WithSlash32_MatchesTheSingleHostExactly()
		=> CodeFor("10.2.3.7/32")
			.Should().Be(@"join(system.ips, "","") =~ ""(^|,)10\\.2\\.3\\.7(,|$)""");

	[Fact]
	public void SetCodeFromCidr_WithSlash31_EnumeratesTheHosts()
		=> CodeFor("10.2.3.0/31")
			.Should().Be(@"join(system.ips, "","") =~ ""(^|,)10\\.2\\.3\\.(0|1|2)(,|$)""");

	[Fact]
	public void SetCodeFromCidr_WithSlash30_EnumeratesTheHosts()
		=> CodeFor("10.2.3.0/30")
			.Should().Be(@"join(system.ips, "","") =~ ""(^|,)10\\.2\\.3\\.(0|1|2|3|4|5|6)(,|$)""");

	[Fact]
	public void SetCodeFromCidr_WithSlash29_EnumeratesTheHostsStartingAtTheGivenOctet()
		=> CodeFor("10.2.3.8/29")
			.Should().Be(@"join(system.ips, "","") =~ ""(^|,)10\\.2\\.3\\.(8|9|10|11|12|13|14|15|16|17|18|19|20|21|22)(,|$)""");

	/// <summary>
	/// Pins the CURRENT host-range size for the enumerated netmasks (25-31).
	/// </summary>
	/// <remarks>
	/// NOTE: these counts are twice what the netmask actually covers, because the range size is
	/// computed as (2 &lt;&lt; (32 - bits)) - 1 rather than 1 &lt;&lt; (32 - bits). A /30 covers 4
	/// addresses but generates 7; a /25 covers 128 but generates 255, spilling into the adjacent
	/// subnet. That is a pre-existing behaviour, pinned here deliberately rather than corrected,
	/// because changing it would change which resources existing AppliesTo functions match in
	/// live portals. See the note raised alongside these tests.
	/// </remarks>
	[Theory]
	[InlineData(25, 255)]
	[InlineData(26, 127)]
	[InlineData(27, 63)]
	[InlineData(28, 31)]
	[InlineData(29, 15)]
	[InlineData(30, 7)]
	[InlineData(31, 3)]
	public void SetCodeFromCidr_WithEnumeratedNetmask_ProducesTheCurrentNumberOfHosts(int bits, int expectedHostCount)
	{
		var code = CodeFor($"10.2.3.0/{bits}");

		var alternation = code.Split(@"\\.(")[1].Split(')')[0];
		alternation.Split('|').Should().HaveCount(expectedHostCount);
	}

	#endregion
}
