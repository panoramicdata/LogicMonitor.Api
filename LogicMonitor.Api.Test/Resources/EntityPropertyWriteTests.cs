using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Resources;

/// <summary>
/// Portal-free tests for <see cref="EntityPropertyWrite" /> - the config-as-code shape used to write
/// hidden (masked) property fields such as snmp.community / *.pass / *.key onto resources and groups.
/// No network or credentials required.
/// </summary>
public class EntityPropertyWriteTests
{
	[Fact]
	public void HiddenFieldWriteConfig_Deserializes()
	{
		// The exact config shape a caller supplies to write hidden fields.
		const string json = """
		[
		  { "type": "resourceGroup", "id": 1234, "name": "snmp.community", "value": "public" }
		]
		""";

		var writes = JsonConvert.DeserializeObject<List<EntityPropertyWrite>>(json);

		writes.Should().NotBeNull();
		writes!.Should().HaveCount(1);

		var write = writes[0];
		write.Type.Should().Be(EntityPropertyWriteTargetType.ResourceGroup);
		write.Id.Should().Be(1234);
		write.Name.Should().Be("snmp.community");
		write.Value.Should().Be("public");

		// The write targets the group's own properties collection (one field), never a full object PUT,
		// so a masked ******** value can never be sent back and clobber the real secret.
		write.PropertiesSubUrl().Should().Be("device/groups/1234/properties");
	}

	[Fact]
	public void ResourceTarget_MapsToDevicePropertiesEndpoint()
	{
		var write = new EntityPropertyWrite
		{
			Type = EntityPropertyWriteTargetType.Resource,
			Id = 42,
			Name = "esx.pass",
			Value = "s3cret"
		};

		write.PropertiesSubUrl().Should().Be("device/devices/42/properties");
	}

	[Fact]
	public void RoundTrips_WithCamelCaseTargetType()
	{
		var write = new EntityPropertyWrite
		{
			Type = EntityPropertyWriteTargetType.ResourceGroup,
			Id = 1234,
			Name = "snmp.community",
			Value = "public"
		};

		var json = JObject.Parse(JsonConvert.SerializeObject(write));

		json["type"]!.Value<string>().Should().Be("resourceGroup");
		json["id"]!.Value<int>().Should().Be(1234);
		json["name"]!.Value<string>().Should().Be("snmp.community");
		json["value"]!.Value<string>().Should().Be("public");
	}

	[Fact]
	public void UnknownTarget_Throws()
	{
		var write = new EntityPropertyWrite { Type = EntityPropertyWriteTargetType.Unknown, Id = 1, Name = "x" };

		var act = write.PropertiesSubUrl;

		act.Should().Throw<NotSupportedException>();
	}
}
