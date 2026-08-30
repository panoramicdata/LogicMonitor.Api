using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api.Test;

/// <summary>
/// Portal-free characterisation tests for <see cref="PortalResponse{T}"/>.
/// </summary>
/// <remarks>
/// PortalResponse decides, for every single API call, whether the portal returned the legacy
/// status/data/errmsg wrapper or a bare object, and how an absent data node is treated. Those
/// decisions are pinned here so that Init and GetObject can be restructured safely.
/// </remarks>
public class PortalResponseTests
{
	private static PortalResponse<T> Respond<T>(string body, HttpStatusCode statusCode = HttpStatusCode.OK)
		where T : new()
	{
		using var httpResponseMessage = new HttpResponseMessage(statusCode)
		{
			Content = new StringContent(body)
		};
		return new PortalResponse<T>(httpResponseMessage);
	}

	#region Init - response shape detection

	[Theory]
	[InlineData("")]
	[InlineData("{}\r\n")]
	public void Init_WithAnEmptyBody_LeavesDataNull(string body)
		=> Respond<EmptyResponse>(body).Data.Should().BeNull();

	[Fact]
	public void Init_WithTheLegacyWrapper_UnwrapsTheDataNode()
	{
		var response = Respond<Alert>("""
			{ "status": 200, "errmsg": "OK", "data": { "id": "DS123" } }
			""");

		response.Data.Should().NotBeNull();
		response.Data!["id"]!.ToString().Should().Be("DS123");
	}

	/// <summary>
	/// Pins the CURRENT precedence: the HTTP status wins over the wrapper's status.
	/// </summary>
	/// <remarks>
	/// NOTE: Init does parse the wrapper's "status" and "errmsg" into HttpStatusCode and
	/// ErrorMessage, but the constructor overwrites both immediately afterwards with the values
	/// from the HttpResponseMessage. So an application-level failure reported by the portal in
	/// the wrapper on an HTTP 200 is currently invisible - the response reads as a success and
	/// the wrapper's errmsg is discarded. Pinned rather than corrected, because changing it
	/// would start surfacing errors that callers do not currently see. See the note raised
	/// alongside these tests.
	/// </remarks>
	[Fact]
	public void Init_WithAFailureInTheLegacyWrapper_IsCurrentlyMaskedByTheHttpStatus()
	{
		var response = Respond<Alert>("""
			{ "status": 404, "errmsg": "Not found", "data": { "id": "x" } }
			""");

		response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
		response.ErrorMessage.Should().BeEmpty();
		response.IsSuccessStatusCode.Should().BeTrue();
	}

	[Fact]
	public void Init_WithoutTheLegacyWrapper_UsesTheWholeBodyAsData()
	{
		var response = Respond<Alert>("""
			{ "id": "DS456", "severity": 4 }
			""");

		response.Data.Should().NotBeNull();
		response.Data!["id"]!.ToString().Should().Be("DS456");
	}

	/// <summary>
	/// A body carrying status and errmsg but a non-container data node (here, null) is not
	/// treated as the wrapper - the whole object becomes the data.
	/// </summary>
	[Fact]
	public void Init_WithWrapperKeysButANonContainerDataNode_TreatsTheWholeBodyAsData()
	{
		var response = Respond<Alert>("""
			{ "status": 200, "errmsg": "OK", "data": null }
			""");

		response.Data.Should().NotBeNull();
		response.Data!["status"]!.ToString().Should().Be("200");
	}

	[Fact]
	public void Init_WithAnEmptyStringListBody_ProducesAnEmptyJArray()
	{
		var response = Respond<List<string>>("[]");

		response.Data.Should().BeOfType<JArray>();
		response.Data!.Should().BeEmpty();
	}

	[Fact]
	public void Init_WithAnEmptyStringListBodyContainingSpaces_ProducesAnEmptyJArray()
	{
		var response = Respond<List<string>>("[ ]");

		response.Data.Should().BeOfType<JArray>();
		response.Data!.Should().BeEmpty();
	}

	#endregion

	#region IsSuccessStatusCode

	[Theory]
	[InlineData(HttpStatusCode.OK)]
	[InlineData(HttpStatusCode.Created)]
	[InlineData(HttpStatusCode.Accepted)]
	[InlineData(HttpStatusCode.NoContent)]
	[InlineData(HttpStatusCode.NotModified)]
	[InlineData(HttpStatusCode.TemporaryRedirect)]
	public void IsSuccessStatusCode_ForASuccessCode_IsTrue(HttpStatusCode statusCode)
		=> Respond<EmptyResponse>("", statusCode).IsSuccessStatusCode.Should().BeTrue();

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.Unauthorized)]
	[InlineData(HttpStatusCode.Forbidden)]
	[InlineData(HttpStatusCode.NotFound)]
	[InlineData(HttpStatusCode.InternalServerError)]
	[InlineData(HttpStatusCode.ServiceUnavailable)]
	public void IsSuccessStatusCode_ForAFailureCode_IsFalse(HttpStatusCode statusCode)
		=> Respond<EmptyResponse>("", statusCode).IsSuccessStatusCode.Should().BeFalse();

	/// <summary>
	/// 207 MultiStatus is handled separately from the enum list, so it is pinned separately.
	/// </summary>
	[Fact]
	public void IsSuccessStatusCode_ForMultiStatus_IsTrue()
		=> Respond<EmptyResponse>("", (HttpStatusCode)207).IsSuccessStatusCode.Should().BeTrue();

	[Fact]
	public void ErrorMessage_ForAFailureCodeWithNoWrapper_IsTheStatusCodeName()
		=> Respond<EmptyResponse>("", HttpStatusCode.NotFound)
			.ErrorMessage.Should().Be(nameof(HttpStatusCode.NotFound));

	#endregion

	#region GetObject

	[Fact]
	public void GetObject_WithNoDataAndAnEmptyResponseType_ReturnsANewInstance()
		=> Respond<EmptyResponse>("").GetObject().Should().NotBeNull();

	[Fact]
	public void GetObject_WithNoDataAndNoContentStatus_ReturnsNull()
		=> Respond<Alert>("", HttpStatusCode.NoContent).GetObject().Should().BeNull();

	[Fact]
	public void GetObject_WithNoDataAndAnOrdinaryStatus_Throws()
	{
		var response = Respond<Alert>("");

		var act = () => response.GetObject();

		act.Should().Throw<LogicMonitorApiException>()
			.WithMessage("*No data node present*");
	}

	[Fact]
	public void GetObject_WithValidData_Deserialises()
	{
		var response = Respond<WebsiteGroup>("""
			{ "id": 42, "name": "Test group" }
			""");

		var result = response.GetObject();

		result.Should().NotBeNull();
		result!.Id.Should().Be(42);
		result.Name.Should().Be("Test group");
	}

	[Fact]
	public void GetObject_WithUndeserialisableData_ThrowsDeserializationException()
	{
		var response = Respond<WebsiteGroup>("""
			{ "id": "not-an-integer" }
			""");

		var act = () => response.GetObject();

		act.Should().Throw<DeserializationException>();
	}

	#endregion
}
