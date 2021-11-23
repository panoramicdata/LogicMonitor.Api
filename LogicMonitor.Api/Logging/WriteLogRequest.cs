using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Logging;

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
	public WriteLogRequest(int resourceId, string message) : this(WriteLogLevel.Info, resourceId, message)
	{
	}

	/// <summary>
	/// Construct a regular deviceId WriteLogRequest
	/// </summary>
	/// <param name="level"></param>
	/// <param name="resourceId"></param>
	/// <param name="message"></param>
	public WriteLogRequest(WriteLogLevel level, int resourceId, string message)
	{
		this["_lm.resourceId"] = new Dictionary<string, string>
			{
				{ "system.deviceId", resourceId.ToString() }
			};
		this["message"] = GetPrefix(level) + message;
	}

	private string GetPrefix(WriteLogLevel level) => level switch
	{
		WriteLogLevel.Trace => "[TRCE] ",
		WriteLogLevel.Debug => "[DEBG] ",
		WriteLogLevel.Info => "[INFO] ",
		WriteLogLevel.Warning => "[WARN] ",
		WriteLogLevel.Error => "[EROR] ",
		WriteLogLevel.Critical => "[CRIT] ",
		WriteLogLevel.Fatal => "[FATL] ",
		_ => throw new NotSupportedException($"Error level {level} not supported.")
	};
}
