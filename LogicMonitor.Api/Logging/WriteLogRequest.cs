using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Logging
{
	/// <summary>
	/// A request to write to a log against a resource id
	/// </summary>
	[DataContract]
	public class WriteLogRequest : Dictionary<string, object>
	{
		/// <summary>
		/// Parameterless constructor for self-assembly
		/// </summary>
		public WriteLogRequest()
		{
		}

		/// <summary>
		/// Construct a regular deviceId WriteLogRequest
		/// </summary>
		/// <param name="resourceId"></param>
		/// <param name="message"></param>
		public WriteLogRequest(int resourceId, string message)
		{
			this["_lm.resourceId"] = new Dictionary<string, string>
			{
				{ "deviceId", resourceId.ToString() }
			};

			this["message"] = message;
		}
	}
}
