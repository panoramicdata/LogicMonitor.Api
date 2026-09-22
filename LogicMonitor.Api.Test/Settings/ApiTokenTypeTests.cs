using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Test.Settings;

/// <summary>
/// An API token's type arrives from the portal as a JSON string, so a type the enum does not
/// declare fails the entire users response rather than that one token. These tests need no portal.
/// </summary>
public class ApiTokenTypeTests
{
	[Fact]
	public void Deserialize_Agentic_ReturnsAgentic()
	{
		// MS-26522. The second enum LogicMonitor started returning "agentic" on, found only by
		// running [LogicMonitor.UserList:] after PrivilegeObjectType was fixed. Path in the live
		// failure was items[4].apiTokens[0].type.
		var token = JsonConvert.DeserializeObject<ApiToken>(
			"""{"id":1,"type":"agentic","status":2}""");

		token.Should().NotBeNull();
		token.Type.Should().Be(ApiTokenType.Agentic);
	}

	[Theory]
	[MemberData(nameof(DeclaredWireValues))]
	public void Deserialize_EveryDeclaredWireValue_ReturnsItsOwnMember(
		string wireValue,
		ApiTokenType expected)
	{
		var json = JsonConvert.SerializeObject(new { type = wireValue });

		var token = JsonConvert.DeserializeObject<ApiToken>(json);

		token.Should().NotBeNull();
		token.Type.Should().Be(expected);
	}

	public static TheoryData<string, ApiTokenType> DeclaredWireValues()
	{
		var data = new TheoryData<string, ApiTokenType>();

		foreach (var value in Enum.GetValues<ApiTokenType>())
		{
			var wire = WireValueOf(value);
			if (wire is not null)
			{
				data.Add(wire, value);
			}
		}

		return data;
	}

	/// <summary>
	/// The string the portal sends for a member, or null for Unknown, which has no wire value and
	/// exists only as the zero default.
	/// </summary>
	private static string? WireValueOf(ApiTokenType value)
	{
		var field = typeof(ApiTokenType).GetField(value.ToString())
			?? throw new InvalidOperationException($"No field for {value}");

		return field.GetCustomAttribute<EnumMemberAttribute>()?.Value;
	}
}
