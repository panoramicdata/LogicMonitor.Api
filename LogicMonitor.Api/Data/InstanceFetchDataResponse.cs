using System.Runtime.Serialization;

namespace LogicMonitor.Api.Data
{
	/// <summary>
	/// An instance FetchData response
	/// </summary>
	[DataContract]
	public class InstanceFetchDataResponse
	{
		/// <summary>
		/// DeviceDataSourceInstanceId
		/// </summary>
		[DataMember(Name = "instanceId")]
		public string DeviceDataSourceInstanceId { get; set; }

		/// <summary>
		/// Error message
		/// </summary>
		[DataMember(Name = "errMsg")]
		public string ErrorMessage { get; set; }

		/// <summary>
		/// DataSource name
		/// </summary>
		[DataMember(Name = "dataSourceName")]
		public string DataSourceName { get; set; }

		/// <summary>
		/// Data Points
		/// </summary>
		[DataMember(Name = "dataPoints")]
		public string[] DataPoints { get; set; }

		/// <summary>
		/// Data Values
		/// </summary>
		[DataMember(Name = "values")]
		public object[][] DataValues { get; set; }

		/// <summary>
		/// Timestamps
		/// </summary>
		[DataMember(Name = "time")]
		public long[] Timestamps { get; set; }

		/// <summary>
		/// Next page parameters
		/// </summary>
		[DataMember(Name = "nextPageParams")]
		public string NextPageParameters { get; set; }
	}
}
