using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api;

public partial class LogicMonitorClient
{
	/// <summary>
	///     Company logo.
	///     If the LogicMonitor Service Provider logo or an SVG image is present, this will be set to null.
	/// </summary>
	/// <param name="imageType"></param>
	/// <param name="cancellationToken"></param>
	public async Task<byte[]> GetImageByteArrayAsync(ImageType imageType, CancellationToken cancellationToken = default)
		=> imageType switch
		{
			ImageType.CompanyLogo => await DownloadImage("setting/logos/company", cancellationToken).ConfigureAwait(false),
			ImageType.LoginLogo => await DownloadImage("setting/logos/login", cancellationToken).ConfigureAwait(false),
			_ => throw new ArgumentOutOfRangeException(nameof(imageType), imageType, null),
		};

	private async Task<byte[]> DownloadImage(string subUrl, CancellationToken cancellationToken)
		=> (await GetAsync<List<byte>>(true, subUrl, cancellationToken).ConfigureAwait(false)).ToArray();

	private async Task DownloadFile(string subUrl, FileInfo fileInfo, CancellationToken cancellationToken)
	{
		var tempFileInfo = await GetAsync<DownloadFileInfo>(true, subUrl, cancellationToken).ConfigureAwait(false);
		tempFileInfo.FileInfo.MoveTo(fileInfo.FullName);
	}
}
