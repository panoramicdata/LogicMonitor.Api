using System.Runtime.Serialization;

namespace LogicMonitor.Api.Collectors
{
	/// <summary>
	/// Collector version
	/// </summary>
	[DataContract]
	public class CollectorVersion
	{
		/// <summary>
		/// Major Version
		/// </summary>
		[DataMember(Name = "majorVersion")]
		public int MajorVersion { get; set; }

		/// <summary>
		/// Minor Version
		/// </summary>
		[DataMember(Name = "minorVersion")]
		public int MinorVersion { get; set; }

		/// <summary>
		/// Whether the release is mandatory
		/// </summary>
		[DataMember(Name = "mandatory")]
		public bool Mandatory { get; set; }

		/// <summary>
		/// Whether the release is stable
		/// </summary>
		[DataMember(Name = "stable")]
		public bool IsStable { get; set; }

		/// <summary>
		/// The release DateTime in seconds since the Epoch
		/// </summary>
		[DataMember(Name = "releaseEpoch")]
		public int ReleaseDateTimeSeconds { get; set; }

		/// <summary>
		/// Whether the release supports 32-bit Windows
		/// </summary>
		[DataMember(Name = "has32bitWindows")]
		public bool Has32bitWindows { get; set; }

		/// <summary>
		/// Whether the release supports 32-bit Linux
		/// </summary>
		[DataMember(Name = "has32bitLinux")]
		public bool Has32bitLinux { get; set; }
	}
}
