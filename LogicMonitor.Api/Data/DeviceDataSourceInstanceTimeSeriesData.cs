using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Data
{
	/// <summary>
	/// Time series graph data
	/// </summary>
	[DataContract]
	public class DeviceDataSourceInstanceTimeSeriesData
	{
		/// <summary>
		/// The DataSource name
		/// </summary>
		[DataMember(Name = "dataSourceName")]
		public string DataSourceName { get; set; }

		/// <summary>
		/// The DataPoints
		/// </summary>
		[DataMember(Name = "dataPoints")]
		public List<string> DataPoints { get; set; }

		/// <summary>
		/// The Values
		/// </summary>
		[DataMember(Name = "values")]
		public List<List<float>> Values { get; set; }

		/// <summary>
		/// The Time
		/// </summary>
		[DataMember(Name = "time")]
		public List<long> Time { get; set; }

		/// <summary>
		/// The NextPageParams
		/// </summary>
		[DataMember(Name = "nextPageParams")]
		public string NextPageParams { get; set; }
	}
}
