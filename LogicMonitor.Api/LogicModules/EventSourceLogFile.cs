using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// An EventSource log file
	/// </summary>
	[DataContract]
	public class EventSourceLogFile
	{
		/// <summary>
		/// Path
		/// </summary>
		[DataMember(Name = "path")]
		public string Path { get; set; }

		/// <summary>
		/// UseGlob
		/// </summary>
		[DataMember(Name = "useGlob")]
		public string UseGlob { get; set; }

		/// <summary>
		/// The encoding
		/// </summary>
		[DataMember(Name = "encoding", IsRequired = false)]
		public string Encoding { get; set; }

		/// <summary>
		/// Matches
		/// </summary>
		[DataMember(Name = "matches")]
		public List<EventSourceMatch> Matches { get; set; }

		/// <summary>
		/// Excludes
		/// </summary>
		[DataMember(Name = "excludes")]
		public List<object> Excludes { get; set; }
	}
}