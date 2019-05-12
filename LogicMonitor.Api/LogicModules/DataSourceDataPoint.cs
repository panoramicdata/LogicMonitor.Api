using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A DataSource DataPoint
	/// </summary>
	[DataContract]
	public class DataSourceDataPoint : NamedItem
	{
		/// <summary>
		/// The alertExprNote
		/// </summary>
		[DataMember(Name = "alertExprNote")]
		public string AlertExpressionNote { get; set; }

		/// <summary>
		/// The DataPoint Id
		/// </summary>
		[DataMember(Name = "dataPointId")]
		public int DataPointId { get; set; }

		/// <summary>
		/// The DataSource Id
		/// </summary>
		[DataMember(Name = "dataSourceId")]
		public int DataSourceId { get; set; }

		/// <summary>
		/// alertTransitionInterval
		/// </summary>
		[DataMember(Name = "alertTransitionInterval")]
		public int AlertTransitionInterval { get; set; }

		/// <summary>
		/// alertClearTransitionInterval
		/// </summary>
		[DataMember(Name = "alertClearTransitionInterval")]
		public int AlertClearTransitionInterval { get; set; }

		/// <summary>
		/// The consolidation function
		/// </summary>
		[DataMember(Name = "consolidateFunc")]
		public string ConsolidationFunction { get; set; }

		/// <summary>
		/// type
		/// </summary>
		[DataMember(Name = "type")]
		public int Type { get; set; }

		/// <summary>
		/// dataType
		/// </summary>
		[DataMember(Name = "dataType")]
		public int DataType { get; set; }

		/// <summary>
		/// maxDigits
		/// </summary>
		[DataMember(Name = "maxDigits")]
		public int MaxDigits { get; set; }

		/// <summary>
		/// postProcessorMethod
		/// </summary>
		[DataMember(Name = "postProcessorMethod")]
		public string PostProcessorMethod { get; set; }

		/// <summary>
		/// postProcessorMethod
		/// </summary>
		[DataMember(Name = "postProcessorParam")]
		public string PostProcessorParam { get; set; }

		/// <summary>
		/// rawDataFieldName
		/// </summary>
		[DataMember(Name = "rawDataFieldName")]
		public string RawDataFieldName { get; set; }

		/// <summary>
		/// maxValue
		/// </summary>
		[DataMember(Name = "maxValue")]
		public string MaxValue { get; set; }

		/// <summary>
		/// minValue
		/// </summary>
		[DataMember(Name = "minValue")]
		public string MinValue { get; set; }

		/// <summary>
		/// userParam1
		/// </summary>
		[DataMember(Name = "userParam1")]
		public string UserParam1 { get; set; }

		/// <summary>
		/// userParam2
		/// </summary>
		[DataMember(Name = "userParam2")]
		public string UserParam2 { get; set; }

		/// <summary>
		/// userParam3
		/// </summary>
		[DataMember(Name = "userParam3")]
		public string UserParam3 { get; set; }

		/// <summary>
		/// alertForNoData
		/// </summary>
		[DataMember(Name = "alertForNoData")]
		public int AlertForNoData { get; set; }

		/// <summary>
		/// The alert expression
		/// </summary>
		[DataMember(Name = "alertExpr")]
		public string AlertExpression { get; set; }

		/// <summary>
		/// alertSubject
		/// </summary>
		[DataMember(Name = "alertSubject")]
		public string AlertSubject { get; set; }

		/// <summary>
		/// alertBody
		/// </summary>
		[DataMember(Name = "alertBody")]
		public string AlertBody { get; set; }
	}
}