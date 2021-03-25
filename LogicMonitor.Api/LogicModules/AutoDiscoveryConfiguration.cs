using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// An autodiscovery configuration
	/// </summary>
	[DataContract]
	public class AutoDiscoveryConfiguration
	{
		/// <summary>
		/// persistentInstance
		/// </summary>
		[DataMember(Name = "persistentInstance")]
		public bool PersistentInstance { get; set; }

		/// <summary>
		/// disableInstance
		/// </summary>
		[DataMember(Name = "disableInstance")]
		public bool DisableInstance { get; set; }

		/// <summary>
		/// deleteInactiveInstance
		/// </summary>
		[DataMember(Name = "deleteInactiveInstance")]
		public bool DeleteInactiveInstance { get; set; }

		/// <summary>
		/// instanceAutoGroupMethod
		/// </summary>
		[DataMember(Name = "instanceAutoGroupMethod")]
		public string InstanceAutoGroupMethod { get; set; }

		/// <summary>
		/// instanceAutoGroupMethodParams
		/// </summary>
		[DataMember(Name = "instanceAutoGroupMethodParams")]
		public string InstanceAutoGroupMethodParams { get; set; }

		/// <summary>
		/// The scheduleInterval
		/// </summary>
		[DataMember(Name = "scheduleInterval")]
		public int ScheduleIntervalSeconds { get; set; }

		/// <summary>
		/// The autodiscovery method
		/// </summary>
		[DataMember(Name = "method")]
		public AutoDiscoveryMethod AutoDiscoveryMethod { get; set; }

		/// <summary>
		/// The version
		/// </summary>
		[DataMember(Name = "filters")]
		public AutoDiscoveryFilter[] AutoDiscoveryFilters { get; set; }

		/// <summary>
		/// The datasource name
		/// </summary>
		[DataMember(Name = "dataSourceName")]
		public string DataSourceName { get; set; }
	}
}