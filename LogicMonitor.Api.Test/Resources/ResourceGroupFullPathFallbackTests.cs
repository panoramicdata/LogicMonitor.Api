namespace LogicMonitor.Api.Test.Resources;

/// <summary>
/// Cover for issue #40.
///
/// LogicMonitor's filter endpoint silently trims whitespace in filter values, so an Eq filter on a
/// path whose segment ends in a space matches nothing and the group appears not to exist. Verified
/// against a live portal: the filter returned 0 results for such a path and 1 for an otherwise
/// identical control, while the tree node free search returned the group with its whitespace intact.
///
/// One customer portal had 12 such groups out of 13,351, and a report iterating them died after
/// 4 hours 45 minutes on the first one it reached.
/// </summary>
public class ResourceGroupFullPathFallbackTests
{
	private static bool NeedsFallback(string fullPath)
		=> LogicMonitorClient.RequiresTreeNodeSearchFallback(fullPath, fullPath.Split('/'));

	[Theory]
	[InlineData("Devices by Type/Meraki APAC/AU 71116 Highpoint MW PRL ")]   // trailing space - issue #40
	[InlineData(" Leading/Normal")]                                          // leading space on an interior segment
	[InlineData("Normal/Interior /Leaf")]                                    // interior segment, whole path looks trimmed
	[InlineData("Trailing\t/Leaf")]                                          // tab counts as whitespace
	public void WhitespaceBearingSegments_UseTheFallback(string fullPath)
		=> NeedsFallback(fullPath).Should().BeTrue(
			"LogicMonitor's Eq filter trims whitespace, so this path cannot be resolved by filter");

	[Theory]
	[InlineData("Devices/Group (with parens)")]
	[InlineData("Devices/Group ) unbalanced")]
	public void Parentheses_StillUseTheFallback(string fullPath)
		=> NeedsFallback(fullPath).Should().BeTrue("the original MS-23720 behaviour must be preserved");

	[Theory]
	[InlineData("Devices by Type/Meraki APAC/AU 71116 Highpoint MW PRL External BOH")]
	[InlineData("Devices/Normal Group With Interior Spaces")]
	[InlineData("Single")]
	public void OrdinaryPaths_UseTheFilter(string fullPath)
		=> NeedsFallback(fullPath).Should().BeFalse(
			"the Eq filter is cheaper and must remain the default for paths it can carry");
}
