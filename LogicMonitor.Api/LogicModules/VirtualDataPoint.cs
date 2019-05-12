using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	///     A virtual data point
	/// </summary>
	[DataContract]
	public class VirtualDataPoint
	{
		/// <summary>
		///     The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		///     The calculation
		/// </summary>
		[DataMember(Name = "rpn")]
		public string Rpn { get; set; }
	}
}