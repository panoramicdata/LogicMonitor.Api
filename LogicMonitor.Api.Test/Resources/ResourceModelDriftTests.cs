using System.Reflection;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Test.Resources;

public class ResourceModelDriftTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	/// <summary>
	/// Every field the portal returns for a Resource should be modelled.
	/// Read-only, one request, and it reports every gap at once rather than
	/// failing on whichever unmodelled field happens to appear first.
	/// </summary>
	[Fact]
	public async Task Resource_ModelsEveryFieldThePortalReturns()
	{
		// A page rather than a single Resource: LogicMonitor omits fields that do not
		// apply to a given device, so one sample cannot show everything the model needs.
		var raw = await LogicMonitorClient
			.GetJObjectAsync("device/devices?size=300", CancellationToken);

		var items = raw["items"] as JArray;
		items.Should().NotBeNullOrEmpty("the portal must return at least one Resource for this to mean anything");

		var portalFieldNames = items!
			.OfType<JObject>()
			.SelectMany(item => item.Properties().Select(property => property.Name))
			.Distinct(StringComparer.Ordinal)
			.OrderBy(name => name, StringComparer.Ordinal)
			.ToList();

		var modelledNames = typeof(Resource)
			.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Select(property => property.GetCustomAttribute<DataMemberAttribute>()?.Name ?? property.Name)
			.ToHashSet(StringComparer.Ordinal);

		var missing = portalFieldNames
			.Where(name => !modelledNames.Contains(name))
			.ToList();

		TestOutputHelper.WriteLine($"Sampled {items.Count} Resources: {portalFieldNames.Count} distinct fields; Resource models {modelledNames.Count}.");
		// A field can be absent from the model, or present under a different casing,
		// which is a very different fix: the second means correcting an existing
		// DataMember name rather than adding a property that duplicates it.
		var caseInsensitive = modelledNames.ToDictionary(name => name, StringComparer.OrdinalIgnoreCase);

		TestOutputHelper.WriteLine($"MISSING ({missing.Count}):");
		foreach (var name in missing)
		{
			var note = caseInsensitive.TryGetValue(name, out var modelled)
				? $"  <-- CASING: the model spells this '{modelled}'"
				: string.Empty;
			TestOutputHelper.WriteLine($"    {name}{note}");
		}

		// Also report whether the portal sent the model's spelling too, which decides
		// whether the two names are one field or genuinely two.
		foreach (var name in missing.Where(caseInsensitive.ContainsKey))
		{
			var modelled = caseInsensitive[name];
			var alsoSent = portalFieldNames.Contains(modelled, StringComparer.Ordinal);
			TestOutputHelper.WriteLine(
				$"    portal also sent '{modelled}': {alsoSent}");
		}

		missing.Should().BeEmpty("every field the portal returns should be modelled on Resource");
	}
}
