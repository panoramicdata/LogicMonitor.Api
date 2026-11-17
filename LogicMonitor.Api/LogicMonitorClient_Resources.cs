namespace LogicMonitor.Api;

/// <summary>
///     Resource Portal interaction
/// </summary>
public partial class LogicMonitorClient
{

	/// <summary>
	/// Get ResourceGroup alerts
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="customColumns"></param>
	/// <param name="needMessage"></param>
	/// <param name="fields"></param>
	/// <param name="size"></param>
	/// <param name="offset"></param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<Page<Alert>> GetResourceGroupAlertsAsync(
		int resourceGroupId,
		[Optional, DefaultParameterValue("")] string customColumns,
		[Optional] bool needMessage,
		[Optional, DefaultParameterValue("")] string fields,
		[Optional, DefaultParameterValue(50)] int size,
		[Optional] int offset,
		[Optional, DefaultParameterValue("")] string filter,
		[Optional] CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<Alert>>($"device/groups/{resourceGroupId}/alerts?customColumns={customColumns}&needMessage={needMessage}&fields={fields}&size={size}&offset={offset}&filter={filter}", cancellationToken);
	private async Task<List<ResourceGroup>> GetAllSubResourceGroups(
		int resourceGroupId,
		CancellationToken cancellationToken)
	{
		var allResourceGroups = new List<ResourceGroup>();

		var resourceGroups =
			await GetAllAsync(
				new Filter<ResourceGroup>
				{
					FilterItems =
					[
						new Eq<ResourceGroup>(nameof(ResourceGroup.ParentId), resourceGroupId)
					]
				},
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (resourceGroups.Count != 0)
		{
			foreach (var resourceGroup in resourceGroups)
			{
				allResourceGroups.Add(resourceGroup);
				allResourceGroups.AddRange(await GetAllSubResourceGroups(resourceGroup.Id, cancellationToken).ConfigureAwait(false));
			}
		}

		return allResourceGroups;
	}

	/// <summary>
	///     Gets Resources by HostName
	/// </summary>
	/// <param name="hostName">The Resource HostName</param>
	/// <param name="maxResultCount">Max result count.  May not exceed 100</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<Resource>> GetResourcesByHostNameAsync
	(string hostName, int maxResultCount, CancellationToken cancellationToken)
		=> [.. (await GetResourcesByNameAsync(
			hostName,
			maxResultCount,
			cancellationToken).ConfigureAwait(false))
			.Where(d => string.Equals(d.Name, hostName, StringComparison.OrdinalIgnoreCase))];

	/// <summary>
	///     Gets Resource by DisplayName
	/// </summary>
	/// <param name="displayName">The Resource DisplayName</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<Resource?> GetResourceByDisplayNameAsync(
		string displayName,
		CancellationToken cancellationToken)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

		// The LM API Eq filter cannot handle parentheses in values - fall back to tree node search
		if (displayName.Contains('(') || displayName.Contains(')'))
		{
			return (await GetResourcesByNameAsync(
				displayName,
				100,
				cancellationToken)
				.ConfigureAwait(false))
				.SingleOrDefault(r => r.DisplayName == displayName);
		}

		return (await GetAllAsync(new Filter<Resource>
		{
			FilterItems =
			[
				new Eq<Resource>(nameof(Resource.DisplayName), displayName
					.EscapeSlashes()
					.EscapePlusCharacter())
			]
		},
		cancellationToken)
		.ConfigureAwait(false))
		.SingleOrDefault(r => r.DisplayName == displayName);
	}

	/// <summary>
	///     Get device properties, in the following order:
	///     - Custom
	///     - Auto
	///     - System
	///     - Inherit
	/// </summary>
	/// <param name="resourceId">The Resource Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<EntityProperty>> GetResourcePropertiesAsync(
		int resourceId,
		CancellationToken cancellationToken)
		=> GetAllAsync<EntityProperty>($"device/devices/{resourceId}/properties", cancellationToken);

	/// <summary>
	/// Schedule active discovery for a Resource
	/// </summary>
	/// <param name="resourceId">The Resource Id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task ScheduleActiveDiscovery(
		int resourceId,
		CancellationToken cancellationToken)
		=> PostAsync<object, object>(
			new object(),
			$"device/devices/{resourceId}/scheduleAutoDiscovery",
			cancellationToken);

	/// <summary>
	///     Get Resource alerts
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="skip">The number to skip</param>
	/// <param name="take">The number to take</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<Alert>> GetResourceAlertsPageAsync(
		int resourceId,
		int skip,
		int take,
		CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<Alert>>($"device/devices/{resourceId}/alerts?size={take}&offset={skip}", cancellationToken);

	/// <summary>
	///     Set single resource property
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
	/// <param name="mode">How to set the property.
	/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
	/// If you know that the property is already set and you want to change the value, use Update.
	/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
	/// If you know that the property is NOT already set and you want to set the value, use Create.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceCustomPropertyAsync(
		int resourceId,
		string name,
		string? value,
		SetPropertyMode mode,
		CancellationToken cancellationToken)
	=>
		SetCustomPropertyAsync(
			resourceId,
			name,
			value,
			mode,
			"device/devices",
			cancellationToken
			);

	/// <summary>
	///     Set single Resource property, using automatic mode
	/// </summary>
	/// <param name="resourceId">The Resource id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceCustomPropertyAsync(
		int resourceId,
		string name,
		string? value,
		CancellationToken cancellationToken)
		=> SetCustomPropertyAsync(
			resourceId,
			name,
			value,
			SetPropertyMode.Automatic,
			"device/devices",
			cancellationToken
			);

	/// <summary>
	///     Set single ResourceGroup property
	/// </summary>
	/// <param name="resourceGroupId">The ResourceGroup Id</param>
	/// <param name="name">The property name</param>
	/// <param name="value">The property value.  If set to null and the property exists, it will be removed.</param>
	/// <param name="mode">How to set the property.
	/// If you are unsure, use CreateOrUpdate (the default).  As this checks to see if the property is set first, this option is slower.
	/// If you know that the property is already set and you want to change the value, use Update.
	/// If you know that the property is already set and you want to delete the value, use Delete (value must also be set to null).
	/// If you know that the property is NOT already set and you want to set the value, use Create.
	/// </param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task SetResourceGroupCustomPropertyAsync(
		int resourceGroupId,
		string name,
		string? value,
		SetPropertyMode mode,
		CancellationToken cancellationToken)
	=>
		SetCustomPropertyAsync(
			resourceGroupId,
			name,
			value,
			mode,
			"device/groups",
			cancellationToken);

	/// <summary>
	///     Applies a single <see cref="EntityPropertyWrite" />, writing one custom property onto the
	///     target <see cref="Resource" /> or <see cref="ResourceGroup" />.
	/// </summary>
	/// <remarks>
	///     This writes only the named property (never a full object), so you always supply the real value
	///     rather than round-tripping a masked <c>********</c>. It is the admin-gated way to set or rotate
	///     hidden fields such as <c>snmp.community</c>, <c>*.pass</c> and <c>*.key</c>. (For reference: a
	///     full-object PUT does not clobber a Resource/ResourceGroup secret either - the server treats the
	///     <c>********</c> mask as "unchanged" - but this method is how you change the stored value.)
	/// </remarks>
	/// <param name="write">The property write directive.</param>
	/// <param name="mode">How to set the property (Create, Update, Delete or Automatic).</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <exception cref="NotSupportedException">Thrown when the write targets an unsupported entity type.</exception>
	public Task SetCustomPropertyAsync(
		EntityPropertyWrite write,
		SetPropertyMode mode,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(write);
		return write.Type switch
		{
			EntityPropertyWriteTargetType.Resource
				=> SetResourceCustomPropertyAsync(write.Id, write.Name, write.Value, mode, cancellationToken),
			EntityPropertyWriteTargetType.ResourceGroup
				=> SetResourceGroupCustomPropertyAsync(write.Id, write.Name, write.Value, mode, cancellationToken),
			_ => throw new NotSupportedException($"Unsupported {nameof(EntityPropertyWriteTargetType)}: {write.Type}")
		};
	}

	/// <summary>
	///     Applies a batch of <see cref="EntityPropertyWrite" /> directives in order. This is the
	///     config-as-code entry point for setting hidden property fields across resources and groups.
	/// </summary>
	/// <param name="writes">The property write directives to apply, in order.</param>
	/// <param name="mode">How to set each property (Create, Update, Delete or Automatic).</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	public async Task SetCustomPropertiesAsync(
		IEnumerable<EntityPropertyWrite> writes,
		SetPropertyMode mode,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(writes);
		foreach (var write in writes)
		{
			await SetCustomPropertyAsync(write, mode, cancellationToken).ConfigureAwait(false);
		}
	}

	/// <summary>
	///     Gets Resources
	/// </summary>
	/// <param name="filter">The Resource filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<Resource>> GetResourcesPageAsync(
		Filter<Resource> filter,
		CancellationToken cancellationToken)
	{
		if (filter is not null && filter.Order is null)
		{
			filter.Order = new Order<Resource> { Property = nameof(Resource.Id), Direction = OrderDirection.Asc };
		}

		return GetBySubUrlAsync<Page<Resource>>($"device/devices?{filter}", cancellationToken);
	}

	/// <summary>
	///     Gets Resources by ResourceGroup Id
	/// </summary>
	/// <param name="resourceGroupId"></param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<Resource>> GetResourcesByResourceGroupIdAsync(
		int resourceGroupId,
		Filter<Resource>? filter,
		CancellationToken cancellationToken)
		=> GetAllAsync(filter, $"device/groups/{resourceGroupId}/devices", cancellationToken);
}
