namespace LogicMonitor.Api;

/// <summary>
///     LogicModule Export/Import operations
/// </summary>
public partial class LogicMonitorClient
{

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
		var typeEndpoint = GetFileImportEndpoint(logicModuleType, isXml);
		var query = isXml ? [] : GetFileImportQuery(handleConflict, fieldsToPreserve);
		var subUrl = $"setting/{typeEndpoint}/import{(isXml ? "xml" : "json")}{(query.Count == 0 ? string.Empty : "?" + string.Join("&", query))}";
		var responseBody = await UploadFileAsync(
			subUrl,
			isXml ? "logicmodule.xml" : "logicmodule.json",
			fileContent,
			isXml ? "application/xml" : "application/json",
			cancellationToken).ConfigureAwait(false);

		return string.IsNullOrWhiteSpace(responseBody) ? null : JObject.Parse(responseBody);
	}

	private static string GetFileImportEndpoint(LogicModuleType logicModuleType, bool isXml)
		=> logicModuleType switch
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

	private static List<string> GetFileImportQuery(string? handleConflict, IReadOnlyCollection<string>? fieldsToPreserve)
	{
		var query = new List<string>();
		if (handleConflict is not null)
		{
			query.Add($"handleConflict={Uri.EscapeDataString(handleConflict)}");
		}

		if (fieldsToPreserve is not null)
		{
			query.AddRange(fieldsToPreserve.Select(f => $"fieldsToPreserve={Uri.EscapeDataString(f)}"));
		}

		return query;
	}
}
