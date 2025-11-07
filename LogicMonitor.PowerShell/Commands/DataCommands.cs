using LogicMonitor.Api.Data;
using LogicMonitor.Api.Time;
using System.Management.Automation;

namespace LogicMonitor.PowerShell.Commands
{
	/// <summary>
	/// Gets performance data from LogicMonitor resources
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "LMResourceData")]
	[OutputType(typeof(object))]
	public class GetLMResourceDataCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 0)]
		public int ResourceId { get; set; }

		/// <summary>
		/// DataSource name or ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 1)]
		public string DataSource { get; set; } = string.Empty;

		/// <summary>
		/// Instance name (optional, for multi-instance datasources)
		/// </summary>
		[Parameter()]
		public string? Instance { get; set; }

		/// <summary>
		/// Start time for data query
		/// </summary>
		[Parameter(Mandatory = true)]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// End time for data query
		/// </summary>
		[Parameter(Mandatory = true)]
		public DateTime EndTime { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Retrieving data for Resource ID {ResourceId}, DataSource: {DataSource}");

				// Convert times to Unix timestamps
				var startTimeUnix = ((DateTimeOffset)StartTime).ToUnixTimeSeconds();
				var endTimeUnix = ((DateTimeOffset)EndTime).ToUnixTimeSeconds();

				// Build the API URL
				var subUrl = $"device/devices/{ResourceId}/devicedatasources/{DataSource}/data";

				var queryParams = new List<string>
  {
					$"start={startTimeUnix}",
		   $"end={endTimeUnix}"
 };

				if (!string.IsNullOrEmpty(Instance))
				{
					queryParams.Add($"filter=instanceName:\"{Instance}\"");
				}

				subUrl += "?" + string.Join("&", queryParams);

				// Get the data
				var result = Client!.GetJObjectAsync(subUrl, CancellationToken.None)
				   .GetAwaiter().GetResult();

				WriteVerboseMessage("Successfully retrieved resource data.");
				WriteObject(result);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Gets raw data from a LogicMonitor resource
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "LMRawData")]
	[OutputType(typeof(RawDataSet))]
	public class GetLMRawDataCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 0)]
		public int ResourceId { get; set; }

		/// <summary>
		/// Data source ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 1)]
		public int DataSourceId { get; set; }

		/// <summary>
		/// Instance ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 2)]
		public int InstanceId { get; set; }

		/// <summary>
		/// Start time for data retrieval
		/// </summary>
		[Parameter()]
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// End time for data retrieval
		/// </summary>
		[Parameter()]
		public DateTime? EndTime { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Retrieving raw data for Resource {ResourceId}, DataSource {DataSourceId}, Instance {InstanceId}");

				var rawDataSet = Client!.GetRawDataSetAsync(ResourceId, DataSourceId, InstanceId, StartTime, EndTime, CancellationToken.None)
					.GetAwaiter().GetResult();

				WriteVerboseMessage($"Retrieved raw data with {rawDataSet.Values.Count} data points");
				WriteObject(rawDataSet);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Triggers a poll now operation for a LogicMonitor resource instance
	/// </summary>
	[Cmdlet(VerbsLifecycle.Invoke, "LMPollNow")]
	[OutputType(typeof(PollNowResponse))]
	public class InvokeLMPollNowCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 0)]
		public int ResourceId { get; set; }

		/// <summary>
		/// Resource data source ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 1)]
		public int ResourceDataSourceId { get; set; }

		/// <summary>
		/// Resource data source instance ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 2)]
		public int ResourceDataSourceInstanceId { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Triggering poll now for Resource {ResourceId}, DataSource {ResourceDataSourceId}, Instance {ResourceDataSourceInstanceId}");

				var pollNowResponse = Client!.PollNowAsync(ResourceId, ResourceDataSourceId, ResourceDataSourceInstanceId, CancellationToken.None)
					.GetAwaiter().GetResult();

				WriteVerboseMessage($"Poll now completed with status: {pollNowResponse.RequestStatus}");
				WriteObject(pollNowResponse);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Gets graph data from a LogicMonitor resource data source instance
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "LMGraphData")]
	[OutputType(typeof(GraphData))]
	public class GetLMGraphDataCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource data source instance ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 0)]
		public int ResourceDataSourceInstanceId { get; set; }

		/// <summary>
		/// Data source graph ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 1)]
		public int DataSourceGraphId { get; set; }

		/// <summary>
		/// Start time for data retrieval
		/// </summary>
		[Parameter()]
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// End time for data retrieval
		/// </summary>
		[Parameter()]
		public DateTime? EndTime { get; set; }

		/// <summary>
		/// Graph width
		/// </summary>
		[Parameter()]
		public int Width { get; set; } = 500;

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Retrieving graph data for Instance {ResourceDataSourceInstanceId}, Graph {DataSourceGraphId}");

				var graphDataRequest = new ResourceDataSourceInstanceGraphDataRequest
				{
					ResourceDataSourceInstanceId = ResourceDataSourceInstanceId,
					DataSourceGraphId = DataSourceGraphId,
					Width = Width
				};

				// Set time parameters
				if (StartTime.HasValue && EndTime.HasValue)
				{
					graphDataRequest.TimePeriod = TimePeriod.Zoom;
					graphDataRequest.StartDateTime = StartTime;
					graphDataRequest.EndDateTime = EndTime;
				}
				else
				{
					// Default to last hour
					graphDataRequest.TimePeriod = TimePeriod.OneHour;
				}

				var graphData = Client!.GetGraphDataAsync(graphDataRequest, CancellationToken.None)
					.GetAwaiter().GetResult();

				WriteVerboseMessage($"Retrieved graph data with {graphData.Lines.Count} lines");
				WriteObject(graphData);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Gets fetch data across multiple resource instances
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "LMFetchData")]
	[OutputType(typeof(FetchDataResponse))]
	public class GetLMFetchDataCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource data source instance IDs
		/// </summary>
		[Parameter(Mandatory = true, Position = 0)]
		public int[] InstanceIds { get; set; } = [];

		/// <summary>
		/// Start time for data retrieval
		/// </summary>
		[Parameter(Mandatory = true, Position = 1)]
		public DateTimeOffset StartTime { get; set; }

		/// <summary>
		/// End time for data retrieval
		/// </summary>
		[Parameter(Mandatory = true, Position = 2)]
		public DateTimeOffset EndTime { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Fetching data for {InstanceIds.Length} instances from {StartTime} to {EndTime}");

				var instanceIdList = InstanceIds.ToList();
				var fetchDataResponse = Client!.GetFetchDataResponseAsync(instanceIdList, StartTime, EndTime, CancellationToken.None)
					.GetAwaiter().GetResult();

				WriteVerboseMessage($"Retrieved fetch data with {fetchDataResponse.TotalCount} instance responses");
				WriteObject(fetchDataResponse);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}
}