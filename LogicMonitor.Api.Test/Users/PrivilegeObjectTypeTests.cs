using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Test.Users;

/// <summary>
/// A role privilege's object type arrives from the portal as a JSON string, so an object type
/// the enum does not declare fails the entire RoleList / UserList response, not just that one
/// privilege. These tests need no portal.
/// </summary>
public class PrivilegeObjectTypeTests
{
	[Fact]
	public void Deserialize_Agentic_ReturnsAgentic()
	{
		// MS-26522. The shape, property and wire value that failed in production, taken from the
		// Panoramic Data portal's own built-in administrator role.
		var privilege = JsonConvert.DeserializeObject<RolePrivilege>(
			"""{"objectType":"agentic","objectId":"*.*.read","operation":"read"}""");

		privilege.Should().NotBeNull();
		privilege.ObjectType.Should().Be(PrivilegeObjectType.Agentic);
	}

	[Theory]
	[MemberData(nameof(DeclaredWireValues))]
	public void Deserialize_EveryDeclaredWireValue_ReturnsItsOwnMember(
		string wireValue,
		PrivilegeObjectType expected)
	{
		var json = JsonConvert.SerializeObject(new { objectType = wireValue });

		var privilege = JsonConvert.DeserializeObject<RolePrivilege>(json);

		privilege.Should().NotBeNull();
		privilege.ObjectType.Should().Be(expected);
	}

	[Fact]
	public void Deserialize_UnrecognisedWireValue_Throws()
	{
		// Deliberate, not an oversight: an object type LogicMonitor has added and we have not
		// declared must fail loudly so that the package is fixed. Tolerating it would map a real
		// privilege onto the wrong member. Agreed on MS-26522, whose acceptance criterion 7
		// offered the tolerant alternative and was declined.
		var act = () => JsonConvert.DeserializeObject<RolePrivilege>(
			"""{"objectType":"aPrivilegeTypeThatDoesNotExist"}""");

		act.Should().Throw<JsonSerializationException>();
	}

	public static TheoryData<string, PrivilegeObjectType> DeclaredWireValues()
	{
		var data = new TheoryData<string, PrivilegeObjectType>();

		foreach (var value in Enum.GetValues<PrivilegeObjectType>())
		{
			data.Add(WireValueOf(value), value);
		}

		return data;
	}

	/// <summary>
	/// The string the portal sends for a member. Most members declare it with EnumMember; four
	/// historic ones use DataMember instead, which StringEnumConverter ignores, leaving them to
	/// match on the member name.
	/// </summary>
	private static string WireValueOf(PrivilegeObjectType value)
	{
		var field = typeof(PrivilegeObjectType).GetField(value.ToString())
			?? throw new InvalidOperationException($"No field for {value}");

		return field.GetCustomAttribute<EnumMemberAttribute>()?.Value
			?? field.GetCustomAttribute<DataMemberAttribute>()?.Name
			?? value.ToString();
	}
}
