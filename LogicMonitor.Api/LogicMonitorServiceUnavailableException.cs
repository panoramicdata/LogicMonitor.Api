using System;
using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api
{
	/// <summary>
	/// This exception is thrown when the LogicMonitor portal is down, typically due to planned maintenance.
	/// </summary>
	[Serializable]
	public class LogicMonitorServiceUnavailableException : LogicMonitorApiException
	{
		internal const string MatchText = "Apologies, service is temporarily unavailable. Please try again later.";

		/// <summary>
		/// Constructor
		/// </summary>
		internal LogicMonitorServiceUnavailableException(string responseBody)
			: base(MatchText)
		{
			ResponseBody = responseBody;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public LogicMonitorServiceUnavailableException(HttpMethod method, string subUrl, HttpStatusCode httpStatusCode, string responseBody, string message = null) : base(method, subUrl, httpStatusCode, responseBody, message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		internal LogicMonitorServiceUnavailableException(Exception exception) : base(MatchText, exception)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public LogicMonitorServiceUnavailableException() : base()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public LogicMonitorServiceUnavailableException(HttpResponseMessage httpResponseMessage) : base(httpResponseMessage)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="responseBody"></param>
		/// <param name="exception"></param>
		protected LogicMonitorServiceUnavailableException(string responseBody, Exception exception) : base(responseBody, exception)
		{
		}
	}
}