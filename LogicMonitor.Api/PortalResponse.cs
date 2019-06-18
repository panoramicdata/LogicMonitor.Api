using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api
{
	/// <summary>
	///    A LogicMonitor API Portal response
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PortalResponse<T> where T : new()
	{
		/// <summary>
		///    Construct a PortalResponse from an HttpResponseMessage
		/// </summary>
		/// <param name="httpResponseMessage"></param>
		public PortalResponse(HttpResponseMessage httpResponseMessage)
		{
			var responseBody = httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
			Init(responseBody);
			HttpStatusCode = httpResponseMessage.StatusCode;
			ErrorMessage = IsSuccessStatusCode ? null : httpResponseMessage.StatusCode.ToString();
		}

		private void Init(string jsonString)
		{
			// Determine the PortalResponse
			if (jsonString?.Length == 0 || jsonString == "{}\r\n")
			{
				return;
			}

			if (typeof(T) == typeof(List<string>) && jsonString.Replace(" ", "") == "[]")
			{
				Data = new JArray(new List<string>());
				return;
			}

			var jObject = JObject.Parse(jsonString);

			// Does the response contain the old wrapper?
			var status = jObject["status"]?.ToString();
			var errorMessage = ((JValue)jObject["errmsg"])?.ToString(CultureInfo.InvariantCulture);
			if (status != null && jObject["data"] is JContainer data && errorMessage != null)
			{
				// Yes
				HttpStatusCode = (HttpStatusCode)int.Parse(status);
				Data = data;
				ErrorMessage = errorMessage;
			}
			else
			{
				// No
				Data = jObject;
			}
		}

		/// <summary>
		///    The HTTPS Status code
		/// </summary>
		public HttpStatusCode HttpStatusCode { get; set; }

		/// <summary>
		///    A JContainer for the Data element
		/// </summary>
		public JContainer Data { get; set; }

		/// <summary>
		///    The error message
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		///    Whether the HttpStatusCode is of an error type
		/// </summary>
		public bool IsSuccessStatusCode
		{
			get
			{
				switch (HttpStatusCode)
				{
					case HttpStatusCode.Continue:
					case HttpStatusCode.SwitchingProtocols:
					case HttpStatusCode.OK:
					case HttpStatusCode.Created:
					case HttpStatusCode.Accepted:
					case HttpStatusCode.NonAuthoritativeInformation:
					case HttpStatusCode.NoContent:
					case HttpStatusCode.ResetContent:
					case HttpStatusCode.PartialContent:
					case HttpStatusCode.MultipleChoices:
					case HttpStatusCode.MovedPermanently:
					case HttpStatusCode.Found:
					case HttpStatusCode.SeeOther:
					case HttpStatusCode.NotModified:
					case HttpStatusCode.UseProxy:
					case HttpStatusCode.Unused:
					case HttpStatusCode.TemporaryRedirect:
						return true;

					default:
						return false;
				}
			}
		}

		/// <summary>
		///    Parse a JSON string into an object of type T.
		///    If T is EmptyResponse, the Data node will not be checked for
		/// </summary>
		/// <param name="converters"></param>
		/// <returns>The object of type T</returns>
		public T GetObject(JsonConverter[] converters = null)
		{
			// If no data was received, throw an exception
			if (Data == null)
			{
				if (typeof(T) == typeof(EmptyResponse))
				{
					return new T();
				}

				// If a success code was not received, throw an exception
				throw new LogicMonitorApiException("No data node present in response");
			}

			var dataString = Data.ToString();
			try
			{
				var deserializedObject = JsonConvert.DeserializeObject<T>(dataString, new JsonSerializerSettings
				{
#if DEBUG
					MissingMemberHandling = MissingMemberHandling.Error,
					ContractResolver = new RequireObjectPropertiesContractResolver(),
#endif
					TypeNameHandling = TypeNameHandling.Auto,
					Converters = converters
				});
				return deserializedObject;
			}
			catch (JsonSerializationException e)
			{
				if (dataString.Contains(LogicMonitorServiceUnavailableException.MatchText))
				{
					throw new LogicMonitorServiceUnavailableException(dataString);
				}
				throw new DeserializationException(dataString, e);
			}
			catch (JsonReaderException e)
			{
				if (dataString.Contains(LogicMonitorServiceUnavailableException.MatchText))
				{
					throw new LogicMonitorServiceUnavailableException(dataString);
				}
				throw new DeserializationException(dataString, e);
			}
		}
	}
}