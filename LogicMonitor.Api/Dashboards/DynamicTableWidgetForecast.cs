using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A dynamic table widget forecast
	/// </summary>
	[DataContract]
	public class DynamicTableWidgetForecast
	{
		/// <summary>
		///     The timeRange
		/// </summary>
		[DataMember(Name = "timeRange")]
		public string TimeRange { get; set; }

		/// <summary>
		///     The severity
		/// </summary>
		[DataMember(Name = "severity")]
		public string Severity { get; set; }

		/// <summary>
		///     The confidence
		/// </summary>
		[DataMember(Name = "confidence")]
		public int Confidence { get; set; }

		/// <summary>
		///     The algorithm
		/// </summary>
		[DataMember(Name = "algorithm")]
		public string Algorithm { get; set; }
	}
}