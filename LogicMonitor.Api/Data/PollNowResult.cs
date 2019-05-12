using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Data
{
	/// <summary>
	/// A poll now result
	/// </summary>
	[DataContract]
	public class PollNowResult
	{
		/// <summary>
		/// The status
		/// </summary>
		[DataMember(Name = "status")]
		public PollNowStatus Status { get; set; }

		/// <summary>
		///  The message
		/// </summary>
		[DataMember(Name = "message")]
		public string Message { get; set; }

		/// <summary>
		/// The timestamp
		/// </summary>
		[DataMember(Name = "timestamp")]
		public int Timestamp { get; set; }

		/// <summary>
		/// The date
		/// </summary>
		[DataMember(Name = "date")]
		public string Date { get; set; }

		/// <summary>
		/// The result
		/// </summary>
		[DataMember(Name = "result")]
		public List<PollNowInnerResult> Result { get; set; }

		/// <summary>
		/// The raw data
		/// </summary>
		[DataMember(Name = "rawData")]
		public List<PollNowItem> RawData { get; set; }

		/// <summary>
		/// The diagnosis
		/// </summary>
		[DataMember(Name = "diagnose")]
		public string Diagnose { get; set; }
	}
}