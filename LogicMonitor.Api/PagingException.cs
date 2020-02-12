using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace LogicMonitor.Api
{
	/// <summary>
	/// An exception ocurring during REST API Paging
	/// </summary>
	[Serializable]
	public class PagingException : LogicMonitorApiException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public PagingException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message"></param>
		public PagingException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public PagingException(
			string message,
			Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected PagingException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="method"></param>
		/// <param name="subUrl"></param>
		/// <param name="httpStatusCode"></param>
		/// <param name="responseBody"></param>
		/// <param name="message"></param>
		public PagingException(
			HttpMethod method,
			string subUrl,
			System.Net.HttpStatusCode httpStatusCode,
			string responseBody,
			string message = null) : base(method, subUrl, httpStatusCode, responseBody, message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="httpResponseMessage"></param>
		public PagingException(HttpResponseMessage httpResponseMessage) : base(httpResponseMessage)
		{
		}
	}
}