using System.Security.Cryptography;

namespace LogicMonitor.Api;

/// <summary>
///     The main client for querying the portal.
/// </summary>
public partial class LogicMonitorClient
{

	/// <summary>
	///     Deletes an item
	/// </summary>
	/// <param name="item">The object to delete</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteStringIdentifiedAsync<T>(
		T @item,
		bool hardDelete,
		CancellationToken cancellationToken
	) where T : StringIdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync(
			$"{new T().Endpoint()}/{@item.Id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}",
			cancellationToken
		);

	/// <summary>
	///     Hard deletes an item
	/// </summary>
	/// <param name="item">The object to delete</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteStringIdentifiedAsync<T>(
		T @item,
		CancellationToken cancellationToken
	) where T : StringIdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync(
			$"{new T().Endpoint()}/{@item.Id}",
			cancellationToken
		);

	/// <summary>
	///     Deletes an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		int id,
		bool hardDelete,
		CancellationToken cancellationToken
	) where T : IdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}", cancellationToken);

	/// <summary>
	///     Hard deletes an item by integer id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		int id,
		CancellationToken cancellationToken
	) where T : IdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{id}", cancellationToken);

	/// <summary>
	///     Deletes an item by string id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		string id,
		bool hardDelete,
		CancellationToken cancellationToken
	) where T : StringIdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync(
			$"{new T().Endpoint()}/{id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}",
			cancellationToken);


	/// <summary>
	///     Hard deletes an item by string id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		 string id,
		 CancellationToken cancellationToken
	) where T : StringIdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{id}", cancellationToken);

	/// <summary>
	///     Deletes a Resource DataSource Instance
	/// </summary>
	/// <param name="resourceDataSourceInstance">The resourceDataSourceInstance to delete</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	public virtual Task DeleteAsync(
		ResourceDataSourceInstance resourceDataSourceInstance,
		bool hardDelete,
		CancellationToken cancellationToken)
		=> DeleteAsync(
			$"device/devices/{resourceDataSourceInstance.ResourceId}/devicedatasources/{resourceDataSourceInstance.ResourceDataSourceId}/instances/{resourceDataSourceInstance.Id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}",
			cancellationToken);

	/// <summary>
	///     Hard deletes a ResourceDataSourceInstance
	/// </summary>
	/// <param name="resourceDataSourceInstance">The resourceDataSourceInstance to delete</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	public virtual Task DeleteAsync(
		ResourceDataSourceInstance resourceDataSourceInstance,
		CancellationToken cancellationToken)
		=> DeleteAsync(
			$"device/devices/{resourceDataSourceInstance.ResourceId}/devicedatasources/{resourceDataSourceInstance.ResourceDataSourceId}/instances/{resourceDataSourceInstance.Id}",
			cancellationToken);

	/// <summary>
	///     Create an item
	/// </summary>
	/// <param name="creationDto"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T"></typeparam>
	public virtual Task<T> CreateAsync<T>(CreationDto<T> creationDto, CancellationToken cancellationToken) where T : IHasEndpoint, new()
	{
		var instance = new T();
		var endpoint = instance is IHasCreationEndpoint hasCreationEndpoint
			? hasCreationEndpoint.CreationEndpoint()
			: instance.Endpoint();
		// Uptime checks are provisioned entirely by the v3 creation body (see UptimeResourceWireMapper): the
		// structured payload is what makes the server register the device as an Uptime resource and apply the
		// Ping_Check_*/Web_Check_* DataSources. No post-create property patching is required.
		return PostAsync<CreationDto<T>, T>(creationDto, endpoint, cancellationToken);
	}

	/// <summary>
	///     Patch specific fields
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="entity"></param>
	/// <param name="fieldsToUpdate">The fields to update</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="AggregateException"></exception>
	public virtual Task PatchAsync<T>(T entity, Dictionary<string, object> fieldsToUpdate, CancellationToken cancellationToken) where T : IPatchable
		=> PatchAsync($"{entity.Endpoint()}/{entity.Id}", fieldsToUpdate, cancellationToken);

	/// <summary>
	///    Patch specific fields
	/// </summary>
	/// <param name="subUrl"></param>
	/// <param name="fieldsToUpdate"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="LogicMonitorApiException"></exception>
	public async Task PatchAsync(string subUrl, Dictionary<string, object> fieldsToUpdate, CancellationToken cancellationToken)
	{
		var prefix = GetPrefix(_patchHttpMethod);
		var jsonString = JsonConvert.SerializeObject(fieldsToUpdate);

		_logger.LogDebug("{Prefix}", prefix);
		HttpResponseMessage httpResponseMessage;
		while (true)
		{
			using (var content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
			{
				var patchSpec = $"?patchFields={string.Join(",", fieldsToUpdate.Keys)}";
				using var requestMessage = new HttpRequestMessage(_patchHttpMethod, RequestUri(subUrl) + patchSpec) { Content = content };
				httpResponseMessage = await GetHttpResponseMessage(requestMessage, subUrl, jsonString, cancellationToken).ConfigureAwait(false);
			}

			_logger.LogDebug("{Prefix} complete", prefix);

			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
				{
					var body = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
					throw new LogicMonitorApiException(HttpMethod.Post, subUrl, HttpStatusCode.Forbidden, body, $"{prefix} failed ({httpResponseMessage.StatusCode}): {body}");
				}

				if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
				{
					// TODO: could also check the reason phrase, and / or the RESPONSE (which contains "Service Temporarily Unavailable")
					_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
					await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
					continue;
				}
				// If a success code was not received, throw an exception
				var responseBody = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
				_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}",
					prefix,
					httpResponseMessage.StatusCode,
					responseBody);
				throw new LogicMonitorApiException(HttpMethod.Get, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
			}

			// Success, so finish
			break;
		}
	}

	/// <summary>
	///     Gets a page of items
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>A list of Collectors</returns>
	public Task<Page<T>> GetPageAsync<T>(Filter<T>? filter, string subUrl, CancellationToken cancellationToken) where T : new()
		=> GetPageInternalAsync(filter, subUrl, cancellationToken);

	/// <summary>
	///     Gets a page of items
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>A list of Collectors</returns>
	public Task<Page<T>> GetPageAsync<T>(Filter<T> filter, CancellationToken cancellationToken) where T : IHasEndpoint, new()
		=> GetPageInternalAsync(filter, new T().Endpoint(), cancellationToken);

	/// <summary>
	///     Gets a page of items
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>A list of Collectors</returns>
	private Task<Page<T>> GetPageInternalAsync<T>(Filter<T>? filter, string subUrl, CancellationToken cancellationToken) where T : new()
		=> GetBySubUrlAsync<Page<T>>(subUrl.Contains('?')
			? $"{subUrl}&{filter}"
			: $"{subUrl}?{filter}"
			, cancellationToken);
	/// <summary>
	///     Starts the execution of a debug command
	/// </summary>
	/// <param name="collectorId">The ID of the collector on which to execute the command</param>
	/// <param name="commandText">The command to execute</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The ExecuteDebugCommandResponse containing the sessionId</returns>
	public Task<ExecuteDebugCommandResponse> ExecuteDebugCommandAsync(int collectorId, string commandText, CancellationToken cancellationToken)
	=> PostAsync<ExecuteDebugCommandRequest, ExecuteDebugCommandResponse>(new ExecuteDebugCommandRequest { Command = commandText }, $"debug?collectorId={collectorId}", cancellationToken);
}
