using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Topologies;

/// <summary>
/// Offline deserialization tests for /topology/data payloads.
/// Regression cover for PanoramicData.Skills issue #701: a "Wireless" vertex (any access point)
/// failed the whole GetTopologyDataAsync call.
/// </summary>
public class TopologyDataDeserializationTests
{
	private const string WirelessVertexJson = """
		{
			"vertices": [
				{
					"id": "device.224942",
					"type": "Wireless",
					"managedEdgeTypes": [ { "type": "Network", "direction": "in", "count": 1 } ],
					"LMResources": [ { "id": 224942, "type": "device", "name": "store-ap-01" } ],
					"alerts": []
				}
			],
			"edges": [ { "from": "device.224941", "to": "device.224942", "type": "Network", "subType": "LLDP" } ]
		}
		""";

	[Fact]
	public void Deserialize_WirelessVertexType_DoesNotThrow()
	{
		var data = JsonConvert.DeserializeObject<TopologyData>(WirelessVertexJson);

		data.Should().NotBeNull();
		data!.Vertices.Should().HaveCount(1);
		data.Vertices[0].Type.Should().Be(DataVertexType.Wireless);
		data.Edges.Should().HaveCount(1);
	}

	[Theory]
	[InlineData("Switch", DataVertexType.Switch)]
	[InlineData("Router", DataVertexType.Router)]
	[InlineData("Firewall", DataVertexType.Firewall)]
	[InlineData("Wireless", DataVertexType.Wireless)]
	[InlineData("Service", DataVertexType.Service)]
	[InlineData("AccessPoint", DataVertexType.AccessPoint)]
	[InlineData("Undiscovered", DataVertexType.Undiscovered)]
	public void Deserialize_VertexTypesSeenInTheWild_DeserializeCorrectly(string apiValue, DataVertexType expected)
	{
		var json = $$"""{"id": "device.1", "type": "{{apiValue}}"}""";

		var vertex = JsonConvert.DeserializeObject<DataVertex>(json);

		vertex.Should().NotBeNull();
		vertex!.Type.Should().Be(expected);
	}

	[Theory]
	[InlineData("Unknown")]
	[InlineData("unknown")]
	public void Deserialize_UnknownVertexTypeInEitherCase_DeserializesToUnknown(string apiValue)
	{
		// The endpoint has been observed returning "Unknown" (capitalised); the enum originally
		// declared only the lower-case form.
		var json = $$"""{"id": "device.1", "type": "{{apiValue}}"}""";

		var vertex = JsonConvert.DeserializeObject<DataVertex>(json);

		vertex.Should().NotBeNull();
		vertex!.Type.Should().Be(DataVertexType.Unknown);
	}

	[Fact]
	public void Deserialize_UnmodelledVertexType_InDebugThrowsOtherwiseFallsBackToUnknown()
	{
		// LogicMonitor can introduce vertex types at any time. Released (Release-built) packages
		// must degrade gracefully rather than failing the whole topology call; DEBUG builds throw
		// so that library developers notice the gap.
		var json = """{"id": "device.1", "type": "SomeBrandNewVertexType"}""";

		var act = () => JsonConvert.DeserializeObject<DataVertex>(json);

#if DEBUG
		act.Should().Throw<NotImplementedException>()
			.WithMessage("*missing an enum member*");
#else
		var vertex = act();
		vertex.Should().NotBeNull();
		vertex!.Type.Should().Be(DataVertexType.Unknown);
#endif
	}

	[Fact]
	public void TopologyDataRequest_GetQueryString_UsesResourceDeviceForm()
	{
		// deviceId=/resourceId=/resourceIds= all return {"errorMessage":"No resource Specified"}.
		var queryString = new TopologyDataRequest
		{
			ResourceId = 224942,
			Algorithm = TopologyAlgorithm.FirstDegreeAway,
		}.GetQueryString();

		queryString.Should().Be("resource=device.224942&algorithm=Neighbours-1");
	}
}
