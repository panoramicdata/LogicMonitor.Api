using LogicMonitor.Api.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogicMonitor.Api
{
	/// <summary>
	///     LogItems
	/// </summary>
	public partial class LogicMonitorClient
	{
		/// <summary>
		///     Logs multiple items
		/// </summary>
		/// <param name="writeLogRequests">A list of log write requests</param>
		/// <param name="cancellationToken"></param>
		/// <returns>The response</returns>
		public Task<WriteLogResponse> WriteLogAsync(
			IEnumerable<WriteLogRequest> writeLogRequests,
			CancellationToken cancellationToken = default)
			=> PostAsync<IEnumerable<WriteLogRequest>, WriteLogResponse>(writeLogRequests, "log/ingest", cancellationToken);

		/// <summary>
		///     Gets the LogItems
		/// </summary>
		/// <param name="writeLogRequest">A single log write requests</param>
		/// <param name="cancellationToken"></param>
		/// <returns>The response</returns>
		public Task<WriteLogResponse> WriteLogAsync(
			WriteLogRequest writeLogRequest,
			CancellationToken cancellationToken = default)
			=> WriteLogAsync(new[] { writeLogRequest }, cancellationToken);

		/// <summary>
		///     Gets the LogItems
		/// </summary>
		/// <param name="deviceId">The device id</param>
		/// <param name="message">The message</param>
		/// <param name="cancellationToken"></param>
		/// <returns>The response</returns>
		public Task<WriteLogResponse> WriteLogAsync(
			int deviceId,
			string message,
			CancellationToken cancellationToken = default)
			=> WriteLogAsync(new[] { new WriteLogRequest(deviceId, message) }, cancellationToken);
	}
}