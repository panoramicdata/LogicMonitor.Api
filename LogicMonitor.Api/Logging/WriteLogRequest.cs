namespace LogicMonitor.Api.Logging;

/// <summary>
/// A request to write to a log against a resource
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
	/// <param name="level"></param>
	/// <param name="resourceId"></param>
	/// <param name="message"></param>
	public WriteLogRequest(WriteLogLevel level, int resourceId, string message)
	{
		this["_lm.resourceId"] = new Dictionary<string, string>
			{
				{ "system.deviceId", resourceId.ToString(CultureInfo.InvariantCulture) }
			};
		this["message"] = GetPrefix(level) + message;
	}

	/// <summary>
	/// Construct a regular resourceDisplayName WriteLogRequest
	/// </summary>
	/// <param name="level"></param>
	/// <param name="resourceDisplayName"></param>
	/// <param name="message"></param>
	public WriteLogRequest(WriteLogLevel level, string resourceDisplayName, string message)
	{
		this["_lm.resourceId"] = new Dictionary<string, string>
			{
				{ "system.displayname", resourceDisplayName }
			};
		this["message"] = GetPrefix(level) + message;
	}

	/// <summary>
	/// Construct a regular custom property WriteLogRequest
	/// </summary>
	/// <param name="level"></param>
	/// <param name="customPropertyName"></param>
	/// <param name="customPropertyValue"></param>
	/// <param name="message"></param>
	public WriteLogRequest(WriteLogLevel level, string customPropertyName, string customPropertyValue, string message)
	{
		this["_lm.resourceId"] = new Dictionary<string, string>
			{
				{ customPropertyName, customPropertyValue }
			};
		this["message"] = GetPrefix(level) + message;
	}

	/// <summary>
	/// Construct a regular custom property WriteLogRequest
	/// </summary>
	/// <param name="level"></param>
	/// <param name="propertyDictionary">
	///		The property dictionary, the combination of which will be used to identify the resource.
	///		This must be unique across the specified custom property values for the entire LM portal.
	/// </param>
	/// <param name="message"></param>
	public WriteLogRequest(WriteLogLevel level, Dictionary<string, string> propertyDictionary, string message)
	{
		this["_lm.resourceId"] = propertyDictionary;
		this["message"] = GetPrefix(level) + message;
	}

	private static string GetPrefix(WriteLogLevel level) => level switch
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
