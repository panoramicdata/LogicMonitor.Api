using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	///     The DataPoint configuration
	/// </summary>
	[DataContract]
	public class DataPointConfiguration : IdentifiedItem
	{
		/// <summary>
		///     The Alert clear interval
		/// </summary>
		[DataMember(Name = "alertClearInterval")]
		public int AlertClearInterval { get; set; }

		/// <summary>
		///     The Alert expression
		/// </summary>
		[DataMember(Name = "alertExpr")]
		public string AlertExpression { get; set; }

		/// <summary>
		///     The Alert expression note
		/// </summary>
		[DataMember(Name = "alertExprNote")]
		public string AlertExpressionNote { get; set; }

		/// <summary>
		///     The Alert transition interval
		/// </summary>
		[DataMember(Name = "alertTransitionInterval")]
		public int AlertTransitionInterval { get; set; }

		/// <summary>
		///     The Alerting disabled on
		/// </summary>
		[DataMember(Name = "alertingDisabledOn")]
		public object AlertingDisabledOn { get; set; }
		// LogicMonitor sometimes returns a string, so the following cannot be used
		// public AlertingDisabledOn AlertingDisabledOn { get;set; }

		/// <summary>
		///     The current Alert Id
		/// </summary>
		[DataMember(Name = "currentAlertId")]
		public int CurrentAlertId { get; set; }

		/// <summary>
		///     The dataPoint description
		/// </summary>
		[DataMember(Name = "dataPointDescription")]
		public string DataPointDescription { get; set; }

		/// <summary>
		///     The dataPointId
		/// </summary>
		[DataMember(Name = "dataPointId")]
		public int DataPointId { get; set; }

		/// <summary>
		///     The dataPointName
		/// </summary>
		[DataMember(Name = "dataPointName")]
		public string DataPointName { get; set; }

		/// <summary>
		///     The dataSourceInstanceAlias
		/// </summary>
		[DataMember(Name = "dataSourceInstanceAlias")]
		public string DataSourceInstanceAlias { get; set; }

		/// <summary>
		///     The dataSourceInstance Id
		/// </summary>
		[DataMember(Name = "dataSourceInstanceId")]
		public int DataSourceInstanceId { get; set; }

		/// <summary>
		///     The DeviceGroup Full Path
		/// </summary>
		[DataMember(Name = "deviceGroupFullPath")]
		public string DeviceGroupFullPath { get; set; }

		/// <summary>
		///     The DeviceGroup Full Path
		/// </summary>
		[DataMember(Name = "deviceGroupId")]
		public int DeviceGroupId { get; set; }

		/// <summary>
		///     Whether alerting is disabled
		/// </summary>
		[DataMember(Name = "disableAlerting")]
		public bool DisableAlerting { get; set; }

		/// <summary>
		///     Whether to disable DataPoint Alert HostGroups
		/// </summary>
		[DataMember(Name = "disableDpAlertHostGroups")]
		public object DisableDpAlertHostGroups { get; set; }

		/// <summary>
		///     The parent DeviceGroup Alert Expression List
		/// </summary>
		[DataMember(Name = "parentDeviceGroupAlertExprList")]
		public List<ParentDeviceGroupAlertExpression> ParentDeviceGroupAlertExprList { get; set; }

		/// <summary>
		///     The Global alert expression
		/// </summary>
		[DataMember(Name = "globalAlertExpr")]
		public string GlobalAlertExpr { get; set; }

		/// <inheritdoc />
		public override string ToString()
			=> $"{DataPointName} : Expression:{AlertExpression} GlobalExpression: {GlobalAlertExpr}{(DisableAlerting ? " (Disabled)" : string.Empty)}";
	}
}