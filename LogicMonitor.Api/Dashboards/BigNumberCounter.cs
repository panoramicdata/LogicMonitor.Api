using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// Big Number counter
	/// </summary>
	[DataContract]
	public class BigNumberCounter
	{
		/// <summary>
		/// The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// The appliesTo
		/// </summary>
		[DataMember(Name = "appliesTo")]
		public string AppliesTo { get; set; }
	}
}