namespace LogicMonitor.Api;

/// <summary>
///    Recycle bin Portal interaction
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///    Restores items in the recycle bin
	/// </summary>
	/// <param name="recycleBinItemIds">The recycle bin item ids to restore</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The website</returns>
	public Task<List<string>> RecycleBinRestoreAsync(List<string> recycleBinItemIds, CancellationToken cancellationToken = default)
	  => PostAsync<List<string>, List<string>>(recycleBinItemIds, "recyclebin/recycles/batchrestore", cancellationToken);

	/// <summary>
	///    Deletes items from the recycle bin
	/// </summary>
	/// <param name="recycleBinItemIds">The recycle bin item ids to delete</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The website</returns>
	public Task<List<string>> RecycleBinDeleteAsync(List<string> recycleBinItemIds, CancellationToken cancellationToken = default)
	  => PostAsync<List<string>, List<string>>(recycleBinItemIds, "recyclebin/recycles/batchdelete", cancellationToken);
}
