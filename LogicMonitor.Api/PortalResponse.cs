namespace LogicMonitor.Api;

/// <summary>
///    A LogicMonitor API Portal response
/// </summary>
/// <typeparam name="T"></typeparam>
internal class PortalResponse<T> where T : new()
{
	/// <summary>
	///    Construct a PortalResponse from an HttpResponseMessage
	/// </summary>
	/// <param name="httpResponseMessage"></param>
	public PortalResponse(HttpResponseMessage httpResponseMessage)
	{
		var responseBody = httpResponseMessage
			.Content
			.ReadAsStringAsync()
			.ConfigureAwait(false)
			.GetAwaiter()
			.GetResult();
		Init(responseBody);
		HttpStatusCode = httpResponseMessage.StatusCode;
		ErrorMessage = IsSuccessStatusCode ? "" : httpResponseMessage.StatusCode.ToString();
	}

	private void Init(string jsonString)
	{
		// Determine the PortalResponse
		if (IsEmptyBody(jsonString))
		{
			return;
		}

		if (IsEmptyStringList(jsonString))
		{
			Data = new JArray(new List<string>());
			return;
		}

		var jObject = JObject.Parse(jsonString);

		// Does the response contain the old wrapper?
		if (TryReadLegacyWrapper(jObject, out var data, out var statusCode, out var errorMessage))
		{
			// Yes
			HttpStatusCode = statusCode;
			Data = data;
			ErrorMessage = errorMessage;
			return;
		}

		// No
		Data = jObject;
	}

	private static bool IsEmptyBody([NotNullWhen(false)] string? jsonString)
		=> jsonString is null || jsonString.Length == 0 || jsonString == "{}\r\n";

	private static bool IsEmptyStringList(string jsonString)
		=> typeof(T) == typeof(List<string>) && jsonString.Replace(" ", "") == "[]";

	/// <summary>
	/// Recognises the legacy status/data/errmsg envelope that older portal endpoints return.
	/// </summary>
	private static bool TryReadLegacyWrapper(
		JObject jObject,
		[NotNullWhen(true)] out JContainer? data,
		out HttpStatusCode statusCode,
		[NotNullWhen(true)] out string? errorMessage)
	{
		data = null;
		statusCode = default;
		errorMessage = null;

		var status = jObject["status"]?.ToString();
		var wrapperErrorMessage = ((JValue?)jObject["errmsg"])?.ToString(CultureInfo.InvariantCulture);
		if (status is null || jObject["data"] is not JContainer wrapperData || wrapperErrorMessage is null)
		{
			return false;
		}

		data = wrapperData;
		statusCode = (HttpStatusCode)int.Parse(status, CultureInfo.InvariantCulture);
		errorMessage = wrapperErrorMessage;
		return true;
	}

	/// <summary>
	///    The HTTPS Status code
	/// </summary>
	public HttpStatusCode HttpStatusCode { get; set; }

	/// <summary>
	///    A JContainer for the Data element
	/// </summary>
	public JContainer? Data { get; set; }

	/// <summary>
	///    The error message
	/// </summary>
	public string ErrorMessage { get; set; } = string.Empty;

	/// <summary>
	///    Whether the HttpStatusCode is of an error type
	/// </summary>
	public bool IsSuccessStatusCode =>
		(int)HttpStatusCode == 207 // MultiStatus
		|| HttpStatusCode is HttpStatusCode.Continue
			or HttpStatusCode.SwitchingProtocols
			or HttpStatusCode.OK
			or HttpStatusCode.Created
			or HttpStatusCode.Accepted
			or HttpStatusCode.NonAuthoritativeInformation
			or HttpStatusCode.NoContent
			or HttpStatusCode.ResetContent
			or HttpStatusCode.PartialContent
			or HttpStatusCode.MultipleChoices
			or HttpStatusCode.MovedPermanently
			or HttpStatusCode.Found
			or HttpStatusCode.SeeOther
			or HttpStatusCode.NotModified
			or HttpStatusCode.UseProxy
			or HttpStatusCode.Unused
			or HttpStatusCode.TemporaryRedirect;

	/// <summary>
	///    Parse a JSON string into an object of type T.
	///    If T is EmptyResponse, the Data node will not be checked for
	/// </summary>
	/// <param name="converters"></param>
	/// <returns>The object of type T</returns>
	public T? GetObject(JsonConverter[]? converters = null)
	{
		// If no data was received, throw an exception
		if (Data is null)
		{
			return GetObjectWithoutData();
		}

		var dataString = Data.ToString();
		converters ??= [];
		try
		{
			return JsonConvert.DeserializeObject<T>(dataString, new JsonSerializerSettings
			{
#if DEBUG
				MissingMemberHandling = MissingMemberHandling.Error,
				ContractResolver = new RequireObjectPropertiesContractResolver(),
#endif
				TypeNameHandling = TypeNameHandling.Auto,
				Converters = converters
			});
		}
		catch (JsonSerializationException e)
		{
			throw BuildDeserializationFailure(dataString, e);
		}
		catch (JsonReaderException e)
		{
			throw BuildDeserializationFailure(dataString, e);
		}
	}

	private T? GetObjectWithoutData()
	{
		if (typeof(T) == typeof(EmptyResponse))
		{
			return new T();
		}

		// If this is a "NoContent" response, return null.
		if (HttpStatusCode == HttpStatusCode.NoContent)
		{
			return default;
		}

		// If a success code was not received, throw an exception
		throw new LogicMonitorApiException("No data node present in response");
	}

	/// <summary>
	/// A portal that is temporarily unavailable reports it in the body rather than the status
	/// code, so that case is distinguished from a genuine deserialization failure.
	/// </summary>
	private static Exception BuildDeserializationFailure(string dataString, Exception e)
		=> dataString.Contains(LogicMonitorServiceUnavailableException.MatchText)
			? new LogicMonitorServiceUnavailableException(dataString)
			: new DeserializationException(dataString, e);
}
