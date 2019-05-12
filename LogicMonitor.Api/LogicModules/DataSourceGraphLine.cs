using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A DataSource Graph DataPoint
	/// </summary>
	[DataContract]
	public class DataSourceGraphLine : NamedItem
	{
		/// <summary>
		/// Color
		/// </summary>
		[DataMember(Name = "color")]
		public string Color { get; set; }

		/// <summary>
		/// The datapoint id
		/// </summary>
		[DataMember(Name = "dataPointId")]
		public int DataPointId { get; set; }

		/// <summary>
		/// The DataPoint name
		/// </summary>
		[DataMember(Name = "dataPointName")]
		public string DataPointName { get; set; }

		///// <summary>
		///// The DataSource Graph Id
		///// </summary>
		//[DataMember(Name = "graphId")]
		//public int GraphId { get; set; }

		/// <summary>
		/// Whether this is a virtual datapoint
		/// </summary>
		[DataMember(Name = "isVirtualDataPoint")]
		public bool IsVirtualDataPoint { get; set; }

		/// <summary>
		/// The DataPoint Legend
		/// </summary>
		[DataMember(Name = "legend")]
		public string Legend { get; set; }

		/// <summary>
		/// The DataPoint Type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; }

		/// <summary>
		/// ToString override
		/// </summary>
		public override string ToString() => $"{Legend} ({Id}, {Color})";
	}
}