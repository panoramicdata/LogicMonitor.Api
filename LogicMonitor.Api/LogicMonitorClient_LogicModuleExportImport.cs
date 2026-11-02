namespace LogicMonitor.Api;

/// <summary>
///     LogicModule Export/Import operations
/// </summary>
public partial class LogicMonitorClient
{
	#region Export Methods - JSON

	/// <summary>
	///     Gets the JSON export for a DataSource.
	/// </summary>
	/// <param name="dataSourceId">The DataSource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The JSON string representation of the DataSource</returns>
	public async Task<string> GetDataSourceJsonAsync(
		int dataSourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/datasources/{dataSourceId}?format=json", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///     Gets the JSON export for an EventSource.
	/// </summary>
	/// <param name="eventSourceId">The EventSource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The JSON string representation of the EventSource</returns>
	public async Task<string> GetEventSourceJsonAsync(
		int eventSourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/eventsources/{eventSourceId}?format=json", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///     Gets the JSON export for a ConfigSource.
	/// </summary>
	/// <param name="configSourceId">The ConfigSource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The JSON string representation of the ConfigSource</returns>
	public async Task<string> GetConfigSourceJsonAsync(
		int configSourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/configsources/{configSourceId}?format=json", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///     Gets the JSON export for a TopologySource.
	/// </summary>
	/// <param name="topologySourceId">The TopologySource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The JSON string representation of the TopologySource</returns>
	public async Task<string> GetTopologySourceJsonAsync(
		int topologySourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/topologysources/{topologySourceId}?format=json", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///     Gets the JSON export for a DiagnosticSource.
	/// </summary>
	/// <param name="diagnosticSourceId">The DiagnosticSource id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The JSON string representation of the DiagnosticSource</returns>
	public async Task<string> GetDiagnosticSourceJsonAsync(
		int diagnosticSourceId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/diagnosticsources/{diagnosticSourceId}?format=json", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///     Gets the JSON export for a JobMonitor (BatchJob).
	/// </summary>
	/// <param name="jobMonitorId">The JobMonitor id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The JSON string representation of the JobMonitor</returns>
	public async Task<string> GetJobMonitorJsonAsync(
		int jobMonitorId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/batchjobs/{jobMonitorId}?format=json", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///     Gets the JSON export for an AppliesToFunction.
	/// </summary>
	/// <param name="appliesToFunctionId">The AppliesToFunction id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The JSON string representation of the AppliesToFunction</returns>
	public async Task<string> GetAppliesToFunctionJsonAsync(
		int appliesToFunctionId,
		CancellationToken cancellationToken)
		=> (await GetBySubUrlAsync<XmlResponse>($"setting/functions/{appliesToFunctionId}?format=json", cancellationToken).ConfigureAwait(false)).Content;

	/// <summary>
	///     Gets the JSON export for a LogicModule by type and id.
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type</param>
	/// <param name="logicModuleId">The LogicModule id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The JSON string representation of the LogicModule</returns>
	/// <exception cref="NotSupportedException">Thrown when the LogicModule type is not supported for JSON export</exception>
	public Task<string> GetLogicModuleJsonAsync(
		LogicModuleType logicModuleType,
		int logicModuleId,
		CancellationToken cancellationToken)
		=> logicModuleType switch
		{
			LogicModuleType.DataSource => GetDataSourceJsonAsync(logicModuleId, cancellationToken),
			LogicModuleType.EventSource => GetEventSourceJsonAsync(logicModuleId, cancellationToken),
			LogicModuleType.ConfigSource => GetConfigSourceJsonAsync(logicModuleId, cancellationToken),
			LogicModuleType.PropertySource => GetPropertySourceJsonAsync(logicModuleId, cancellationToken),
			LogicModuleType.TopologySource => GetTopologySourceJsonAsync(logicModuleId, cancellationToken),
			LogicModuleType.DiagnosticSource => GetDiagnosticSourceJsonAsync(logicModuleId, cancellationToken),
			LogicModuleType.JobMonitor => GetJobMonitorJsonAsync(logicModuleId, cancellationToken),
			LogicModuleType.AppliesToFunction => GetAppliesToFunctionJsonAsync(logicModuleId, cancellationToken),
			_ => throw new NotSupportedException($"JSON export is not supported for LogicModule type: {logicModuleType}")
		};

	/// <summary>
	///     Gets the XML export for a LogicModule by type and id.
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type</param>
	/// <param name="logicModuleId">The LogicModule id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The XML string representation of the LogicModule</returns>
	/// <exception cref="NotSupportedException">Thrown when the LogicModule type is not supported for XML export</exception>
	public Task<string> GetLogicModuleXmlAsync(
		LogicModuleType logicModuleType,
		int logicModuleId,
		CancellationToken cancellationToken)
		=> logicModuleType switch
		{
			LogicModuleType.DataSource => GetDataSourceXmlAsync(logicModuleId, cancellationToken),
			LogicModuleType.EventSource => GetEventSourceXmlAsync(logicModuleId, cancellationToken),
			LogicModuleType.ConfigSource => GetConfigSourceXmlAsync(logicModuleId, cancellationToken),
			LogicModuleType.PropertySource => GetPropertySourceJsonAsync(logicModuleId, cancellationToken), // PropertySource returns JSON even with XML format
			_ => throw new NotSupportedException($"XML export is not supported for LogicModule type: {logicModuleType}")
		};

	#endregion Export Methods - JSON

	#region Import Methods - JSON

	/// <summary>
	///     Imports a DataSource from a JSON string.
	/// </summary>
	/// <param name="json">The JSON string representing the DataSource</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported DataSource</returns>
	public Task<DataSource> ImportDataSourceJsonAsync(
		string json,
		CancellationToken cancellationToken)
		=> ImportLogicModuleJsonAsync<DataSource>(json, "setting/datasources", cancellationToken);

	/// <summary>
	///     Imports an EventSource from a JSON string.
	/// </summary>
	/// <param name="json">The JSON string representing the EventSource</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported EventSource</returns>
	public Task<EventSource> ImportEventSourceJsonAsync(
		string json,
		CancellationToken cancellationToken)
		=> ImportLogicModuleJsonAsync<EventSource>(json, "setting/eventsources", cancellationToken);

	/// <summary>
	///     Imports a ConfigSource from a JSON string.
	/// </summary>
	/// <param name="json">The JSON string representing the ConfigSource</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported ConfigSource</returns>
	public Task<ConfigSource> ImportConfigSourceJsonAsync(
		string json,
		CancellationToken cancellationToken)
		=> ImportLogicModuleJsonAsync<ConfigSource>(json, "setting/configsources", cancellationToken);

	/// <summary>
	///     Imports a PropertySource from a JSON string.
	/// </summary>
	/// <param name="json">The JSON string representing the PropertySource</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported PropertySource</returns>
	public Task<PropertySource> ImportPropertySourceJsonAsync(
		string json,
		CancellationToken cancellationToken)
		=> ImportLogicModuleJsonAsync<PropertySource>(json, "setting/propertyrules", cancellationToken);

	/// <summary>
	///     Imports a TopologySource from a JSON string.
	/// </summary>
	/// <param name="json">The JSON string representing the TopologySource</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported TopologySource</returns>
	public Task<TopologySource> ImportTopologySourceJsonAsync(
		string json,
		CancellationToken cancellationToken)
		=> ImportLogicModuleJsonAsync<TopologySource>(json, "setting/topologysources", cancellationToken);

	/// <summary>
	///     Imports a DiagnosticSource from a JSON string.
	/// </summary>
	/// <param name="json">The JSON string representing the DiagnosticSource</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported DiagnosticSource</returns>
	public Task<LogicMonitor.Api.LogicModules.DiagnosticSource> ImportDiagnosticSourceJsonAsync(
		string json,
		CancellationToken cancellationToken)
		=> ImportLogicModuleJsonAsync<LogicMonitor.Api.LogicModules.DiagnosticSource>(json, "setting/diagnosticsources", cancellationToken);

	/// <summary>
	///     Imports a JobMonitor (BatchJob) from a JSON string.
	/// </summary>
	/// <param name="json">The JSON string representing the JobMonitor</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported JobMonitor</returns>
	public Task<JobMonitor> ImportJobMonitorJsonAsync(
		string json,
		CancellationToken cancellationToken)
		=> ImportLogicModuleJsonAsync<JobMonitor>(json, "setting/batchjobs", cancellationToken);

	/// <summary>
	///     Imports an AppliesToFunction from a JSON string.
	/// </summary>
	/// <param name="json">The JSON string representing the AppliesToFunction</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported AppliesToFunction</returns>
	public Task<AppliesToFunction> ImportAppliesToFunctionJsonAsync(
		string json,
		CancellationToken cancellationToken)
		=> ImportLogicModuleJsonAsync<AppliesToFunction>(json, "setting/functions", cancellationToken);

	/// <summary>
	///     Internal method to import a LogicModule from JSON.
	/// </summary>
	private async Task<T> ImportLogicModuleJsonAsync<T>(
		string json,
		string endpoint,
		CancellationToken cancellationToken)
		where T : class, new()
	{
		// Parse the JSON to a JObject for posting
		var jObject = JObject.Parse(json);

		// Remove the id field if present (for creating new modules)
		jObject.Remove("id");

		// Post the JSON to create the LogicModule
		var result = await PostJObjectAsync(jObject, endpoint, cancellationToken).ConfigureAwait(false);

		// Deserialize the response
		return result?.ToObject<T>() ?? new T();
	}

	#endregion Import Methods - JSON

	#region File-based Export/Import

	/// <summary>
	///     Exports a LogicModule to a JSON file.
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type</param>
	/// <param name="logicModuleId">The LogicModule id</param>
	/// <param name="filePath">The file path to save the JSON to</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task ExportLogicModuleToJsonFileAsync(
		LogicModuleType logicModuleType,
		int logicModuleId,
		string filePath,
		CancellationToken cancellationToken)
	{
		var json = await GetLogicModuleJsonAsync(logicModuleType, logicModuleId, cancellationToken).ConfigureAwait(false);

		// Pretty-print the JSON
		var jObject = JObject.Parse(json);
		var formattedJson = jObject.ToString(Formatting.Indented);

		// Write to file (.NET Standard 2.0 compatible)
		File.WriteAllText(filePath, formattedJson);
	}

	/// <summary>
	///     Exports a LogicModule to an XML file.
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type</param>
	/// <param name="logicModuleId">The LogicModule id</param>
	/// <param name="filePath">The file path to save the XML to</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task ExportLogicModuleToXmlFileAsync(
		LogicModuleType logicModuleType,
		int logicModuleId,
		string filePath,
		CancellationToken cancellationToken)
	{
		var xml = await GetLogicModuleXmlAsync(logicModuleType, logicModuleId, cancellationToken).ConfigureAwait(false);

		// Write to file (.NET Standard 2.0 compatible)
		File.WriteAllText(filePath, xml);
	}

	/// <summary>
	///     Imports a DataSource from a JSON file.
	/// </summary>
	/// <param name="filePath">The file path to read the JSON from</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported DataSource</returns>
	public Task<DataSource> ImportDataSourceFromJsonFileAsync(
		string filePath,
		CancellationToken cancellationToken)
	{
		var json = File.ReadAllText(filePath);
		return ImportDataSourceJsonAsync(json, cancellationToken);
	}

	/// <summary>
	///     Imports an EventSource from a JSON file.
	/// </summary>
	/// <param name="filePath">The file path to read the JSON from</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported EventSource</returns>
	public Task<EventSource> ImportEventSourceFromJsonFileAsync(
		string filePath,
		CancellationToken cancellationToken)
	{
		var json = File.ReadAllText(filePath);
		return ImportEventSourceJsonAsync(json, cancellationToken);
	}

	/// <summary>
	///     Imports a ConfigSource from a JSON file.
	/// </summary>
	/// <param name="filePath">The file path to read the JSON from</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported ConfigSource</returns>
	public Task<ConfigSource> ImportConfigSourceFromJsonFileAsync(
		string filePath,
		CancellationToken cancellationToken)
	{
		var json = File.ReadAllText(filePath);
		return ImportConfigSourceJsonAsync(json, cancellationToken);
	}

	/// <summary>
	///     Imports a PropertySource from a JSON file.
	/// </summary>
	/// <param name="filePath">The file path to read the JSON from</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported PropertySource</returns>
	public Task<PropertySource> ImportPropertySourceFromJsonFileAsync(
		string filePath,
		CancellationToken cancellationToken)
	{
		var json = File.ReadAllText(filePath);
		return ImportPropertySourceJsonAsync(json, cancellationToken);
	}

	/// <summary>
	///     Imports a TopologySource from a JSON file.
	/// </summary>
	/// <param name="filePath">The file path to read the JSON from</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported TopologySource</returns>
	public Task<TopologySource> ImportTopologySourceFromJsonFileAsync(
		string filePath,
		CancellationToken cancellationToken)
	{
		var json = File.ReadAllText(filePath);
		return ImportTopologySourceJsonAsync(json, cancellationToken);
	}

	/// <summary>
	///     Imports a DiagnosticSource from a JSON file.
	/// </summary>
	/// <param name="filePath">The file path to read the JSON from</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported DiagnosticSource</returns>
	public Task<LogicMonitor.Api.LogicModules.DiagnosticSource> ImportDiagnosticSourceFromJsonFileAsync(
		string filePath,
		CancellationToken cancellationToken)
	{
		var json = File.ReadAllText(filePath);
		return ImportDiagnosticSourceJsonAsync(json, cancellationToken);
	}

	/// <summary>
	///     Imports a JobMonitor from a JSON file.
	/// </summary>
	/// <param name="filePath">The file path to read the JSON from</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported JobMonitor</returns>
	public Task<JobMonitor> ImportJobMonitorFromJsonFileAsync(
		string filePath,
		CancellationToken cancellationToken)
	{
		var json = File.ReadAllText(filePath);
		return ImportJobMonitorJsonAsync(json, cancellationToken);
	}

	/// <summary>
	///     Imports an AppliesToFunction from a JSON file.
	/// </summary>
	/// <param name="filePath">The file path to read the JSON from</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The imported AppliesToFunction</returns>
	public Task<AppliesToFunction> ImportAppliesToFunctionFromJsonFileAsync(
		string filePath,
		CancellationToken cancellationToken)
	{
		var json = File.ReadAllText(filePath);
		return ImportAppliesToFunctionJsonAsync(json, cancellationToken);
	}

	#endregion File-based Export/Import

	#region Portal file import

	/// <summary>
	///     Imports a LogicModule file through the portal's importjson/importxml endpoint, which can resolve a conflict with an existing module
	/// </summary>
	/// <param name="logicModuleType">The LogicModule type</param>
	/// <param name="fileContent">The exported LogicModule JSON or XML</param>
	/// <param name="isXml">True for XML (DataSource, ConfigSource, EventSource and JobMonitor only), false for JSON</param>
	/// <param name="handleConflict">How the portal should handle a module that already exists, or null for the portal default</param>
	/// <param name="fieldsToPreserve">Fields of an existing module to keep when overwriting it, or null</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The portal's response, usually the imported module, or null if the body was empty</returns>
	public async Task<JObject?> ImportLogicModuleFileAsync(
		LogicModuleType logicModuleType,
		string fileContent,
		bool isXml,
		string? handleConflict,
		IReadOnlyCollection<string>? fieldsToPreserve,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(fileContent);
		var typeEndpoint = logicModuleType switch
		{
			LogicModuleType.DataSource => "datasources",
			LogicModuleType.ConfigSource => "configsources",
			LogicModuleType.EventSource => "eventsources",
			LogicModuleType.JobMonitor => "batchjobs",
			LogicModuleType.PropertySource when !isXml => "propertyrules",
			LogicModuleType.TopologySource when !isXml => "topologysources",
			LogicModuleType.DiagnosticSource when !isXml => "diagnosticsources",
			LogicModuleType.LogSource when !isXml => "logsources",
			LogicModuleType.AppliesToFunction when !isXml => "functions",
			LogicModuleType.SnmpSysOIDMap when !isXml => "oids",
			_ => throw new NotSupportedException($"LogicMonitor has no {(isXml ? "XML" : "JSON")} file import for {logicModuleType}."),
		};

		var query = new List<string>();
		if (!isXml && handleConflict is not null)
		{
			query.Add($"handleConflict={Uri.EscapeDataString(handleConflict)}");
		}

		if (!isXml && fieldsToPreserve is not null)
		{
			query.AddRange(fieldsToPreserve.Select(f => $"fieldsToPreserve={Uri.EscapeDataString(f)}"));
		}

		var subUrl = $"setting/{typeEndpoint}/import{(isXml ? "xml" : "json")}{(query.Count == 0 ? string.Empty : "?" + string.Join("&", query))}";
		var responseBody = await UploadFileAsync(
			subUrl,
			isXml ? "logicmodule.xml" : "logicmodule.json",
			fileContent,
			isXml ? "application/xml" : "application/json",
			cancellationToken).ConfigureAwait(false);

		return string.IsNullOrWhiteSpace(responseBody) ? null : JObject.Parse(responseBody);
	}

	#endregion Portal file import
}
