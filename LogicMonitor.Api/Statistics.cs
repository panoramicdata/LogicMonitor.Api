namespace LogicMonitor.Api;

/// <summary>
/// Summary information about the connection
/// </summary>
public class Statistics : IdentifiedItem
{
	/// <summary>
	/// Amount of data transferred in the downlink in bytes
	/// </summary>
	public long DataTransferDownlinkBytes { get; set; }

	/// <summary>
	/// Amount of data transferred in the uplink in bytes
	/// </summary>
	public long DataTransferUplinkBytes { get; set; }

	/// <summary>
	/// The number of successful API calls
	/// </summary>
	public int ApiCallSuccessCount { get; set; }

	/// <summary>
	/// The number of failed API calls
	/// </summary>
	public int ApiCallFailureCount { get; set; }

	/// <summary>
	/// The number of POST calls
	/// </summary>
	public int ApiCallPostCount { get; set; }

	/// <summary>
	/// The number of GET calls
	/// </summary>
	public int ApiCallGetCount { get; set; }

	/// <summary>
	/// The number of DELETE calls
	/// </summary>
	public int ApiCallDeleteCount { get; internal set; }

	/// <summary>
	/// The number of TRACE calls
	/// </summary>
	public int ApiCallTraceCount { get; internal set; }

	/// <summary>
	/// The number of HEAD calls
	/// </summary>
	public int ApiCallHeadCount { get; internal set; }

	/// <summary>
	/// The number of PUT calls
	/// </summary>
	public int ApiCallPutCount { get; internal set; }

	/// <summary>
	/// The number of OPTIONS calls
	/// </summary>
	public int ApiCallOptionsCount { get; internal set; }

	/// <summary>
	/// The number of other calls
	/// </summary>
	public int ApiCallOtherCount { get; internal set; }
	public int ApiCallPatchCount { get; internal set; }
}
