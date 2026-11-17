namespace LogicMonitor.Api;

/// <summary>
///    Websites Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///    Gets a website group by full path.
	///    Returns null if not found.
	/// </summary>
	/// <param name="websiteGroupFullPath">The website group full path.  The root is "/".</param>
	/// <param name="cancellationToken">Optional cancellation token</param>
	public async Task<WebsiteGroup?> GetWebsiteGroupByFullPathAsync(string websiteGroupFullPath, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(websiteGroupFullPath);

		if (string.IsNullOrWhiteSpace(websiteGroupFullPath) || websiteGroupFullPath == "/")
		{
			return await GetAsync<WebsiteGroup>(1, cancellationToken).ConfigureAwait(false);
		}
		// A non-root path has been specified.

		var fullpathsplit = websiteGroupFullPath.Split('/');
		var currentWebsiteGroupId = 1;
		foreach (var localPath in fullpathsplit)
		{
			var websiteGroupById = await GetAsync<WebsiteGroup>(currentWebsiteGroupId, cancellationToken).ConfigureAwait(false);
			var websiteOverview = websiteGroupById.ChildWebsiteGroups?.SingleOrDefault(sov => sov.Name == localPath);
			if (websiteOverview is null)
			{
				return null;
			}

			currentWebsiteGroupId = websiteOverview.Id;
		}

		return await GetBySubUrlAsync<WebsiteGroup>($"website/groups/{currentWebsiteGroupId}", cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	///     Set single website property
	/// </summary>
	/// <param name="websiteId">The Website Id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
	/// <param name="mode">How to set the property.
	/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
	/// If you know that the property is already set and you want to change the value, use Update.
	/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
	/// If you know that the property is NOT already set and you want to set the value, use Create.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetWebsiteCustomPropertyAsync(
		int websiteId,
		string name,
		string? value,
		[Optional, DefaultParameterValue(SetPropertyMode.Automatic)] SetPropertyMode mode,
		[Optional] CancellationToken cancellationToken)
	=>
		SetWebsiteOrWebsiteGroupPropertyAsync<Website>(websiteId, name, value, mode, cancellationToken);

	private async Task SetWebsiteOrWebsiteGroupPropertyAsync<T>(
		int id,
		string name,
		string? value,
		SetPropertyMode mode,
		CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint, IHasCustomProperties, new()
	{
		if (typeof(T) != typeof(Website) && typeof(T) != typeof(WebsiteGroup))
		{
			throw new InvalidOperationException($"Only to be used for {nameof(Website)} and {nameof(WebsiteGroup)}.");
		}

		ValidatePropertyValueForMode(mode, value);

		// NB the generic SetCustomPropertyAsync helper cannot be used for websites and website
		// groups - the portal does not support the sub-URL it builds, so the get/modify/put
		// sequence below is done by hand instead.
		var websiteOrGroup = await GetAsync<T>(id, cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		ApplyCustomPropertyChange(websiteOrGroup.CustomProperties, name, value, mode);

		await PutAsync(websiteOrGroup, cancellationToken).ConfigureAwait(false);
	}

	private static void ValidatePropertyValueForMode(SetPropertyMode mode, string? value)
	{
		// Every mode other than create, update and delete (e.g. CreateOrUpdate) permits both a null and a non-null value.
		if (mode is SetPropertyMode.Create or SetPropertyMode.Update && value is null)
		{
			throw new InvalidOperationException("Value cannot be null if the mode is create or update.");
		}

		if (mode == SetPropertyMode.Delete && value is not null)
		{
			throw new ArgumentException("If mode is delete, value must be set to null", nameof(value));
		}
	}

	private static void ApplyCustomPropertyChange(List<EntityProperty> customProperties, string name, string? value, SetPropertyMode mode)
	{
		var existingCustomProperty = customProperties.SingleOrDefault(cp => cp.Name == name);
		if (existingCustomProperty is null)
		{
			if (mode == SetPropertyMode.Delete)
			{
				throw new LogicMonitorApiException("Can't delete a custom property that is not there.");
			}

			if (value is not null)
			{
				customProperties.Add(new EntityProperty
				{
					Name = name,
					Value = value
				});
			}

			return;
		}

		if (value is null)
		{
			customProperties.Remove(existingCustomProperty);
		}
		else
		{
			existingCustomProperty.Value = value;
		}
	}

	/// <summary>
	///     Set single websiteGroup property
	/// </summary>
	/// <param name="websiteGroupId">The WebsiteGroup Id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
	/// <param name="mode">How to set the property.
	/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
	/// If you know that the property is already set and you want to change the value, use Update.
	/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
	/// If you know that the property is NOT already set and you want to set the value, use Create.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetWebsiteGroupCustomPropertyAsync(
		int websiteGroupId,
		string name,
		string? value,
		[Optional, DefaultParameterValue(SetPropertyMode.Automatic)] SetPropertyMode mode,
		[Optional] CancellationToken cancellationToken)
	=>
		SetWebsiteOrWebsiteGroupPropertyAsync<WebsiteGroup>(websiteGroupId, name, value, mode, cancellationToken);

	/// <summary>
	/// get a list of websites for a group (Response may contain extra fields depending upon the type of check { PingCheck | WebCheck} being added)
	/// </summary>
	/// <param name="id">The parent website group id.  If not specified, the root id (1) is used.</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	public async Task<Page<Website>> GetWebsitesByWebsiteGroupIdAsync(
		int id,
		Filter<Website> filter,
		CancellationToken cancellationToken)
		=> (await FilteredGetAsync($"website/groups/{id}/websites", filter, cancellationToken).ConfigureAwait(false));

	/// <summary>
	///     Get website properties, in the following order:
	///     - Custom
	///     - Auto
	///     - System
	///     - Inherit
	/// </summary>
	/// <param name="websiteId">The Website Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<EntityProperty>> GetWebsitePropertiesAsync(int websiteId, CancellationToken cancellationToken)
		=> GetAllAsync<EntityProperty>($"website/websites/{websiteId}/properties", cancellationToken);
}
