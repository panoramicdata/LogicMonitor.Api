using LogicMonitor.Api.Extensions;
using System;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Flows
{
	/// <summary>
	/// A Flow
	/// </summary>
	[DataContract]
	public class Flow
	{
		/// <summary>
		/// The data type
		/// </summary>
		[DataMember(Name = "dataType")]
		public string DataType { get; set; }

		/// <summary>
		/// Device Display Name. This is only populated when the request is
		/// for a Device Group's flow, not when for a Device's flow
		/// </summary>
		[DataMember(Name = "deviceDisplayName")]
		public string DisplayName { get; set; }

		/// <summary>
		/// Source IP address
		/// </summary>
		[DataMember(Name = "srcIP")]
		public string SourceIp { get; set; }

		/// <summary>
		/// Source DNS
		/// </summary>
		[DataMember(Name = "srcDNS")]
		public string SourceDns { get; set; }

		/// <summary>
		/// Source port
		/// </summary>
		[DataMember(Name = "srcPort")]
		public int SourcePort { get; set; }

		/// <summary>
		/// Destination IP address
		/// </summary>
		[DataMember(Name = "dstIP")]
		public string DestinationIp { get; set; }

		/// <summary>
		/// Destination DNS
		/// </summary>
		[DataMember(Name = "dstDNS")]
		public string DestinationDns { get; set; }

		/// <summary>
		/// Destination port
		/// </summary>
		[DataMember(Name = "dstPort")]
		public int DestinationPort { get; set; }

		/// <summary>
		/// First collection time in seconds since the Epoch
		/// </summary>
		[DataMember(Name = "firstEpochInSec")]
		public long FirstSeenSeconds { get; set; }

		/// <summary>
		/// First collection time in seconds since the Epoch
		/// </summary>
		[IgnoreDataMember]
		public DateTime? FirstSeenUtc => FirstSeenSeconds.ToNullableDateTimeUtc();

		/// <summary>
		/// Last collection time in seconds since the Epoch
		/// </summary>
		[DataMember(Name = "lastEpochInSec")]
		public long LastSeenSeconds { get; set; }

		/// <summary>
		/// Last collection time in seconds since the Epoch
		/// </summary>
		[IgnoreDataMember]
		public DateTime? LastSeenUtc => LastSeenSeconds.ToNullableDateTimeUtc();

		/// <summary>
		/// Protocol
		/// </summary>
		[DataMember(Name = "protocol")]
		public string Protocol { get; set; }

		/// <summary>
		/// Inbound interface id
		/// </summary>
		[DataMember(Name = "ifIn")]
		public int InboundInterfaceId { get; set; }

		/// <summary>
		/// Outbound interface id
		/// </summary>
		[DataMember(Name = "ifOut")]
		public int OutboundInterfaceId { get; set; }

		/// <summary>
		/// Usage bytes
		/// </summary>
		[DataMember(Name = "usage")]
		public long UsageMb { get; set; }

		/// <summary>
		/// Usage bytes
		/// Actually usage is in MBytes, so we'll calculate this one
		/// </summary>
		[IgnoreDataMember]
		public long UsageBytes => UsageMb * 1000000;

		/// <summary>
		/// The percent usage
		/// </summary>
		[DataMember(Name = "percentUsage")]
		public double PercentUsage { get; set; }

		/// <summary>
		/// Source bytes
		/// Not provided any more by LogicMonitor
		/// </summary>
		[IgnoreDataMember]
		public long SourceBytes => SourceMb * 1000000;

		/// <summary>
		/// Source megabytes
		/// </summary>
		[DataMember(Name = "sourceMBytes")]
		public long SourceMb { get; set; }

		/// <summary>
		/// Destination bytes.
		/// Not populated any more by LogicMonitor
		/// </summary>
		[IgnoreDataMember]
		public long DestinationBytes => DestinationMb * 1000000;

		/// <summary>
		/// Destination megabytes
		/// </summary>
		[DataMember(Name = "destinationMBytes")]
		public long DestinationMb { get; set; }

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		public override string ToString() => $"{GetEndPointInfoString(SourceIp, SourceDns, SourcePort, SourceBytes)} <-> {GetEndPointInfoString(DestinationIp, DestinationDns, DestinationPort, DestinationBytes)}";

		private static string GetEndPointInfoString(string ip, string dns, int port, long bytes) => $"{dns}{(dns == ip ? string.Empty : $"({ip})")}:{port} [{bytes:N0} bytes]";
	}
}