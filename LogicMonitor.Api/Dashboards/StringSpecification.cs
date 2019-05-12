using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A string specification
	/// </summary>
	[DataContract]
	public class StringSpecification
	{
		/// <summary>
		/// The value
		/// </summary>
		[DataMember(Name = "value")]
		public string Value { get; set; }

		/// <summary>
		/// Whether this is a glob
		/// </summary>
		[DataMember(Name = "isGlob")]
		public bool IsGlob { get; set; }
	}
}