using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Alerts;

/// <summary>
/// Serialisation cover for issue #28.
///
/// EscalationChainCreationDto used to carry four destination properties - "destination" and
/// "ccdestination" (the names the READ model returns) alongside "destinations" and "ccDestinations"
/// (the names the API honours on WRITE). All four defaulted to an empty list rather than null, and
/// the client's NullValueHandling.Ignore only skips nulls, so every create serialised all four.
///
/// A caller that populated the read-shaped property therefore sent its recipients under
/// "ccdestination" AND an empty "ccDestinations". The API read the empty one and returned
/// HTTP 200 with a fully-formed chain and no CC recipients: silent data loss, visible only on a
/// re-read. The expected values below are the wire shapes observed against a live portal, not
/// values derived from the model.
/// </summary>
public class EscalationChainCreationDtoSerializationTests
{
	private static string Serialize(EscalationChainCreationDto dto)
		=> JsonConvert.SerializeObject(dto, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

	private static EscalationChainCreationDto BuildDto() => new()
	{
		Name = "Test chain",
		Description = "Test",
	};

	[Fact]
	public void WhenCcDestinationsSet_SerializesUnderTheWriteName()
	{
		var dto = BuildDto();
		dto.CcDestinations.Add(new Destination { Method = "email", Address = "someone@example.com" });

		var json = Serialize(dto);

		json.Should().Contain("\"ccDestinations\"");
		json.Should().Contain("someone@example.com");
	}

	[Fact]
	public void ReadShapedAliases_AreNotSerialized()
	{
		// The whole defect: an unset alias must not go out as an empty array that the API then
		// prefers over the populated canonical field.
		var dto = BuildDto();
		dto.Destinations.Add(new Destination { Method = "email", Address = "someone@example.com" });

		var json = Serialize(dto);

		json.Should().NotContain("\"ccdestination\"");
		json.Should().NotContain("\"destination\":");
	}

	[Fact]
	public void SettingTheReadShapedAlias_PopulatesTheCanonicalField()
	{
		// Code written against the read shape now works rather than silently losing its recipients.
		var dto = BuildDto();

#pragma warning disable CS0618 // Testing the obsolete alias is the point of this test
		dto.CcDestination = [new Destination { Method = "email", Address = "cc@example.com" }];
		dto.Destination = [new Destination { Method = "email", Address = "to@example.com" }];
#pragma warning restore CS0618

		dto.CcDestinations.Should().ContainSingle(d => d.Address == "cc@example.com");
		dto.Destinations.Should().ContainSingle(d => d.Address == "to@example.com");

		var json = Serialize(dto);

		json.Should().Contain("\"ccDestinations\"");
		json.Should().Contain("cc@example.com");
		json.Should().Contain("\"destinations\"");
		json.Should().Contain("to@example.com");
		json.Should().NotContain("\"ccdestination\"");
	}

	[Fact]
	public void ReadShapedAlias_ReflectsTheCanonicalField()
	{
		var dto = BuildDto();
		dto.CcDestinations.Add(new Destination { Method = "email", Address = "cc@example.com" });

#pragma warning disable CS0618 // Testing the obsolete alias is the point of this test
		dto.CcDestination.Should().BeSameAs(dto.CcDestinations);
#pragma warning restore CS0618
	}
}
