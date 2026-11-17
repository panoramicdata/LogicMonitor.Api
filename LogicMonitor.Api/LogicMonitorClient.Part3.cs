using System.Security.Cryptography;

namespace LogicMonitor.Api;

/// <summary>
///     The main client for querying the portal.
/// </summary>
public partial class LogicMonitorClient
{

	// A page that is empty, one that completes the total, and OpsNoteTags (which do not page) all end the loop.
	private static bool IsLastPage<T>(int pageItemCount, int totalItemCount, int expectedTotal)
		=> pageItemCount == 0
			|| totalItemCount >= expectedTotal
			|| typeof(T) == typeof(OpsNoteTag);

	/// <summary>
	///     Gets an item singleton
	/// </summary>
	/// <typeparam name="T">The item to get</typeparam>
	/// <param name="cancellationToken"></param>
	public virtual Task<T> GetAsync<T>(CancellationToken cancellationToken) where T : class, IHasSingletonEndpoint, new()
		=> GetBySubUrlAsync<T>(new T().Endpoint(), cancellationToken);

	/// <summary>
	///     Gets an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The item to get</typeparam>
	public virtual Task<T> GetAsync<T>(int id, CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint, new()
		=> GetBySubUrlAsync<T>($"{new T().Endpoint()}/{id}", cancellationToken);

	/// <summary>
	///     Gets a single item by id, bringing back only specific properties as specified in a filter
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The item to get</typeparam>
	public virtual Task<T> GetAsync<T>(int id, Filter<T>? filter, CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint, new()
		=> GetBySubUrlAsync<T>($"{new T().Endpoint()}/{id}?{filter}", cancellationToken);

	/// <summary>
	///    Gets something by its name
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="name"></param>
	/// <param name="cancellationToken"></param>
	public virtual async Task<T?> GetByNameAsync<T>(string name, CancellationToken cancellationToken)
	where T : NamedItem, IHasEndpoint, new()
	{
		ArgumentNullException.ThrowIfNull(name);

		var items = await GetAllAsync(new Filter<T>
		{
			FilterItems =
				[
					new Eq<T>(nameof(NamedEntity.Name), name)
				]
		}, cancellationToken: cancellationToken).ConfigureAwait(false);

		// If only one, return that.
		return items.Count switch
		{
			0 => null,
			1 => items[0],
			_ => throw new InvalidOperationException($"An unexpected number of {typeof(T).Name} have name: {name}"),
		};
	}

	/// <summary>
	///     Gets an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The item to get</typeparam>
	public virtual Task<T> GetAsync<T>(string id, CancellationToken cancellationToken) where T : StringIdentifiedItem, IHasEndpoint, new()
	=> GetBySubUrlAsync<T>($"{new T().Endpoint()}/{id}", cancellationToken);

	/// <summary>
	///     Executes an item by id
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="item"></param>
	/// <param name="cancellationToken"></param>
	public virtual Task<T> ExecuteAsync<T>(T @item, CancellationToken cancellationToken) where T : IdentifiedItem, IExecutable, new()
	=> GetBySubUrlAsync<T>($"{@item.Endpoint()}/{@item.Id}/executenow", cancellationToken);

	/// <summary>
	///     Executes an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The item to get</typeparam>
	public virtual Task<T> ExecuteAsync<T>(int id, CancellationToken cancellationToken) where T : IdentifiedItem, IExecutable, new()
	=> GetBySubUrlAsync<T>($"{new T().Endpoint()}/{id}/executenow", cancellationToken);

	/// <summary>
	///     Patch a single IPatchable entity's property.
	/// </summary>
	/// <param name="entity">The entity</param>
	/// <param name="propertyName">The name of the property</param>
	/// <param name="value">The value</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The entity type</typeparam>
	/// <exception cref="ArgumentOutOfRangeException">Throw if the property does not exist on the entity.</exception>
	public virtual async Task PatchPropertyAsync<T>(T entity, string propertyName, object value, CancellationToken cancellationToken) where T : IPatchable =>
		await PatchAsync(entity, new Dictionary<string, object>
		{
				{ GetSerializationName<T>(propertyName), value }
		}, cancellationToken)
	  .ConfigureAwait(false);

	/// <summary>
	/// Get the serialization name for a specified type
	/// </summary>
	/// <typeparam name="T">The type</typeparam>
	/// <param name="propertyName">The property</param>
	public static string GetSerializationName<T>(string propertyName)
	{
		var propertyInfos = typeof(T).GetProperties();
		var property = propertyInfos.SingleOrDefault(p => p.Name == propertyName)
			?? throw new ArgumentOutOfRangeException(nameof(propertyName), $"{propertyName} is not a property of {typeof(T).Name}.");

		// Use reflection to find the DataMember Name
		return property.GetCustomAttributes(typeof(DataMemberAttribute), true).Cast<DataMemberAttribute>().SingleOrDefault()!.Name!;
	}

	/// <summary>
	/// Get the serialization name for a specified enum type
	/// </summary>
	/// <param name="enumObject">The property</param>
	public static string GetSerializationNameFromEnumMember(object enumObject)
	{
		var type = enumObject.GetType();
		if (!type.IsEnum)
		{
			throw new ArgumentException($"{enumObject} is not an enum", nameof(enumObject));
		}

		var fieldInfos = type.GetFields();
		var field = fieldInfos.SingleOrDefault(f => f.Name == enumObject.ToString())
			?? throw new ArgumentOutOfRangeException(nameof(enumObject), $"{@enumObject} is not a member of enum {type.Name}.");

		// Use reflection to find the DataMember Name
		var list = field.GetCustomAttributes(typeof(EnumMemberAttribute), true).Cast<EnumMemberAttribute>().ToList();
		return list.SingleOrDefault()!.Value!;
	}

	/// <summary>
	///     Update an identified item
	/// </summary>
	/// <param name="item">The object to update</param>
	/// <typeparam name="T"></typeparam>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="AggregateException"></exception>
	public virtual Task PutAsync<T>(T @item, CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint
		// The ignoreReference permits forcing appliesTo functions and is ignored for other types
		=> PutAsync($"{@item.Endpoint()}/{@item.Id}?ignoreReference=true", @item, cancellationToken);

	/// <summary>
	///     Update a string identified item
	/// </summary>
	/// <param name="item">The object to update</param>
	/// <typeparam name="T"></typeparam>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="AggregateException"></exception>
	public virtual Task PutStringIdentifiedItemAsync<T>(T @item, CancellationToken cancellationToken) where T : StringIdentifiedItem, IHasEndpoint
		// The ignoreReference permits forcing appliesTo functions and is ignored for other types
		=> PutAsync($"{@item.Endpoint()}/{@item.Id}?ignoreReference=true", @item, cancellationToken);

	/// <summary>
	/// Update an item
	/// </summary>
	/// <param name="subUrl">The subURL</param>
	/// <param name="item">The updated object</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	public async Task PutAsync(string subUrl, object @item, CancellationToken cancellationToken)
	{
		var httpMethod = HttpMethod.Put;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		var jsonString = JsonConvert.SerializeObject(@item);
		_logger.LogTrace("{Prefix} jsonString:\r\n{JsonString}", prefix, jsonString);

		var httpResponseMessage = await SendWithRetryAsync(httpMethod, subUrl, jsonString, prefix, false, cancellationToken).ConfigureAwait(false);

		var portalResponse = new PortalResponse<EmptyResponse>(httpResponseMessage);
		if (!portalResponse.IsSuccessStatusCode)
		{
			throw new LogicMonitorApiException(portalResponse.ErrorMessage) { HttpStatusCode = portalResponse.HttpStatusCode };
		}
	}

	/// <summary>
	///     Deletes an item
	/// </summary>
	/// <param name="item">The object to delete</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		T @item,
		bool hardDelete,
		CancellationToken cancellationToken)
		where T : IdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{@item.Id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}", cancellationToken);

	/// <summary>
	///     Hard deletes an item
	/// </summary>
	/// <param name="item">The object to delete</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		T @item,
		CancellationToken cancellationToken)
		where T : IdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{@item.Id}", cancellationToken);
}
