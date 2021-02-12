using LogicMonitor.Api.Websites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
	/// <summary>
	///    Websites Portal interaction
	/// </summary>
	public partial class LogicMonitorClient
	{
		/// <summary>
		///    Gets a website group by Id.
		/// </summary>
		/// <param name="websiteGroupId">The parent website group id.  The root website group is 1.</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns></returns>
		[Obsolete("Use GetAsync<WebsiteGroup> instead", true)]
		public Task<WebsiteGroup> GetWebsiteGroupByIdAsync(int websiteGroupId, CancellationToken cancellationToken = default)
			=> GetAsync<WebsiteGroup>(websiteGroupId, cancellationToken);

		/// <summary>
		///    Gets a website group by full path.
		///    Returns null if not found.
		/// </summary>
		/// <param name="websiteGroupFullPath">The website group full path.  The root is "/".</param>
		/// <param name="cancellationToken">Optional cancellation token</param>
		/// <returns></returns>
		public async Task<WebsiteGroup> GetWebsiteGroupByFullPathAsync(string websiteGroupFullPath, CancellationToken cancellationToken = default)
		{
			if (websiteGroupFullPath == null)
			{
				throw new ArgumentNullException(nameof(websiteGroupFullPath));
			}
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
				if (websiteOverview == null)
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
		/// <returns></returns>
		public Task SetWebsiteCustomPropertyAsync(
			int websiteId,
			string name,
			string value,
			SetPropertyMode mode = SetPropertyMode.Automatic,
			CancellationToken cancellationToken = default)
		=>
			SetWebsiteOrWebsiteGroupPropertyAsync<Website>(websiteId, name, value, mode, cancellationToken);

		private async Task SetWebsiteOrWebsiteGroupPropertyAsync<T>(
			int id,
			string name,
			string value,
			SetPropertyMode mode,
			CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint, IHasCustomProperties, new()
		{
			// Input checking
			switch (typeof(T).Name)
			{
				case nameof(WebsiteGroup):
				case nameof(Website):
					// OK
					break;
				default:
					throw new InvalidOperationException($"Only to be used for {nameof(Website)} and {nameof(WebsiteGroup)}.");
			}
			switch (mode)
			{
				case SetPropertyMode.Create:
				case SetPropertyMode.Update:
					if (value == null)
					{
						throw new InvalidOperationException("Value cannot be null if the mode is create or update.");
					}
					break;
				case SetPropertyMode.Delete:
					if (value != null)
					{
						throw new ArgumentException("If mode is delete, value must be set to null", nameof(value));
					}
					break;
			}

			// NB it is not yet possible to do the following:
			//return SetCustomPropertyAsync(
			//		  websiteId,
			//		  name,
			//		  value,
			//		  mode,
			//		  "website/websites",
			//		  cancellationToken);

			var websiteOrGroup = await GetAsync<T>(id, cancellationToken: cancellationToken)
				.ConfigureAwait(false);
			var existingCustomProperty = websiteOrGroup.CustomProperties.SingleOrDefault(cp => cp.Name == name);
			if (existingCustomProperty != null)
			{
				switch (value)
				{
					case null:
						websiteOrGroup.CustomProperties.Remove(existingCustomProperty);
						break;
					default:
						existingCustomProperty.Value = value;
						break;
				}
			}
			else
			{
				switch (mode)
				{
					case SetPropertyMode.Delete:
						throw new LogicMonitorApiException("Can't delete a custom property that is not there.");
				}

				switch (value)
				{
					case null:
						break;
					default:
						websiteOrGroup.CustomProperties.Add(new Property
						{
							Name = name,
							Value = value
						});
						break;
				}
			}
			await PutAsync(websiteOrGroup, cancellationToken).ConfigureAwait(false);
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
		/// <returns></returns>
		public Task SetWebsiteGroupCustomPropertyAsync(
			int websiteGroupId,
			string name,
			string value,
			SetPropertyMode mode = SetPropertyMode.Automatic,
			CancellationToken cancellationToken = default)
		=>
			SetWebsiteOrWebsiteGroupPropertyAsync<WebsiteGroup>(websiteGroupId, name, value, mode, cancellationToken);

		/// <summary>
		///    Set website overviews for the specified website group
		/// </summary>
		/// <param name="websiteGroupId">The parent website group id.  If not specified, the root id (1) is used.</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		public async Task<List<Website>> GetWebsitesByWebsiteGroupIdAsync(
			int websiteGroupId = 1,
			CancellationToken cancellationToken = default)
			=> (await GetBySubUrlAsync<Page<Website>>($"website/groups/{websiteGroupId}/websites", cancellationToken).ConfigureAwait(false)).Items;

		/// <summary>
		///     Get website properties, in the following order:
		///     - Custom
		///     - Auto
		///     - System
		///     - Inherit
		/// </summary>
		/// <param name="websiteId">The Website Id</param>
		/// <param name="cancellationToken">The cancellation token</param>
		public Task<List<Property>> GetWebsitePropertiesAsync(int websiteId, CancellationToken cancellationToken = default)
			=> GetAllAsync<Property>($"website/websites/{websiteId}/properties", cancellationToken);

		/// <summary>
		///     Gets website Group properties
		/// </summary>
		/// <param name="websiteGroupId"></param>
		/// <param name="cancellationToken">The cancellation token</param>
		public Task<List<Property>> GetWebsiteGroupPropertiesAsync(int websiteGroupId, CancellationToken cancellationToken = default)
			=> GetAllAsync<Property>($"website/groups/{websiteGroupId}/properties", cancellationToken);
	}
}