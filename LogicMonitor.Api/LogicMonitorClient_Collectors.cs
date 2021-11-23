using LogicMonitor.Api.Collectors;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api;

/// <summary>
///     ConfigSource portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Gets all collectors
	/// </summary>
	/// <param name="cancellationToken">The cancellation token</param>
	[Obsolete("Use GetAllAsync<Collector>() instead")]
	public Task<List<Collector>> GetAllCollectorsAsync(CancellationToken cancellationToken = default)
		=> GetAllAsync<Collector>(cancellationToken);
	//{
	//	var allCollectors = new List<Collector>();
	//	var collectorGroups = await GetAllAsync<CollectorGroup>().ConfigureAwait(false);
	//	foreach (var collectorGroup in collectorGroups)
	//	{
	//		var collectors = await GetAllCollectorsByCollectorGroupId(collectorGroup.Id, null, cancellationToken).ConfigureAwait(false);
	//		allCollectors.AddRange(collectors);
	//	}
	//	return allCollectors;
	//}

	/// <summary>
	///     Get all collectors in a given collector group
	/// </summary>
	/// <param name="collectorGroupId">The collector group id</param>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<List<Collector>> GetAllCollectorsByCollectorGroupId(int collectorGroupId, Filter<Collector> filter = null, CancellationToken cancellationToken = default)
		=> GetAllAsync(filter, $"setting/collector/groups/{collectorGroupId}/collectors", cancellationToken);

	/// <summary>
	///     Download a collector
	/// </summary>
	/// <param name="collectorId">The collector ID</param>
	/// <param name="fileInfo">The fileInfo to download the Collector to</param>
	/// <param name="collectorPlatformAndArchitecture">The Collector platform and architecture</param>
	/// <param name="collectorDownloadType">The collector download type</param>
	/// <param name="collectorSize">The Collector size</param>
	/// <param name="collectorVersion">The collector version (e.g. 26001 for 26.001)</param>
	/// <param name="cancellationToken"></param>
	public async Task DownloadCollector(
		int collectorId,
		FileInfo fileInfo,
		CollectorPlatformAndArchitecture collectorPlatformAndArchitecture,
		CollectorDownloadType collectorDownloadType = CollectorDownloadType.FullPackage,
		CollectorSize collectorSize = CollectorSize.Small,
		int? collectorVersion = null,
		CancellationToken cancellationToken = default)
	{
		// Get the download token
		var downloadToken = await GetBySubUrlAsync<DownloadToken>($"setting/collector/collectors/{collectorId}/downloadToken", cancellationToken).ConfigureAwait(false);

		var suburl = $"setting/collector/collectors/{collectorId}/{ (collectorDownloadType == CollectorDownloadType.Bootstrap ? "bootstraps" : "installers") }/{collectorPlatformAndArchitecture.ToString().ToLowerInvariant()}?";
		if (collectorVersion != null)
		{
			suburl += $"collectorVersion={collectorVersion}&";
		}
		suburl += $"token={downloadToken.Token}&";
		suburl += "monitorOthers=true&";
		suburl += $"collectorSize={collectorSize.ToString().ToLowerInvariant()}&";
		suburl += "v=2";

		// URL should be like:
		// setting/collector/collectors/202/bootstraps/win64?collectorVersion=26001&token=88137D69D9A48FDFF60AFE0AC5EF4EAB450C9F05&monitorOthers=true&collectorSize=small&v=2

		// Download the collector
		await DownloadFile(suburl, fileInfo, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets the list of available Collector Versions
	/// </summary>
	/// <param name="filter">An optional filter</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	public async Task<List<CollectorVersion>> GetAllCollectorVersionsAsync(Filter<CollectorVersion> filter = null, CancellationToken cancellationToken = default)
		=> (await GetBySubUrlAsync<Page<CollectorVersion>>($"setting/collector/collectors/versions?{filter}", cancellationToken).ConfigureAwait(false))
		.Items
		.OrderByDescending(cv => cv.MajorVersion)
		.ThenByDescending(cv => cv.MinorVersion)
		.ToList();
}
