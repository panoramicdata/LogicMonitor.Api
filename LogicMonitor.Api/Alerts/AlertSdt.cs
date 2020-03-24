using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// An alert SDT
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(AlertSdtConverter))]
	public abstract class AlertSdt
	{
		/// <summary>
		/// The underlying type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; }

		/// <summary>
		/// The SDT type
		/// </summary>
		[DataMember(Name = "sdtType")]
		public string SdtType { get; set; }

		/// <summary>
		/// The time zone to use
		/// </summary>
		[DataMember(Name = "timezone")]
		public string Timezone { get; set; }

		/// <summary>
		/// The start month day
		/// </summary>
		[DataMember(Name = "monthDay")]
		public int MonthDay { get; set; }

		/// <summary>
		/// The user that set the SDT
		/// </summary>
		[DataMember(Name = "admin")]
		public string Admin { get; set; }

		/// <summary>
		/// The end date time in seconds since the Epoch
		/// </summary>
		[DataMember(Name = "endDateTime")]
		public long EndDateTimeSeconds { get; set; }

		/// <summary>
		/// The week of month
		/// </summary>
		[DataMember(Name = "weekofMonth")]
		public int WeekOfMonth { get; set; }

		/// <summary>
		/// Whether the SDT is effective
		/// </summary>
		[DataMember(Name = "isEffective")]
		public bool IsEffective { get; set; }

		/// <summary>
		/// The minute
		/// </summary>
		[DataMember(Name = "minute")]
		public int Minute { get; set; }

		/// <summary>
		/// The duration
		/// </summary>
		[DataMember(Name = "duration")]
		public int Duration { get; set; }

		/// <summary>
		/// The end hour
		/// </summary>
		[DataMember(Name = "endHour")]
		public int EndHour { get; set; }

		/// <summary>
		/// The start date time in seconds since the Epoch
		/// </summary>
		[DataMember(Name = "startDateTime")]
		public long StartDateTime { get; set; }

		/// <summary>
		/// The hour
		/// </summary>
		[DataMember(Name = "hour")]
		public int Hour { get; set; }

		/// <summary>
		/// The week day
		/// </summary>
		[DataMember(Name = "weekDay")]
		public string WeekDay { get; set; }

		/// <summary>
		/// The comment
		/// </summary>
		[DataMember(Name = "comment")]
		public string Comment { get; set; }

		/// <summary>
		/// The SDT Id
		/// </summary>
		[DataMember(Name = "id")]
		public int Id { get; set; }

		/// <summary>
		/// The category
		/// </summary>
		[DataMember(Name = "category")]
		public Category Category { get; set; }

		/// <summary>
		/// The end minute
		/// </summary>
		[DataMember(Name = "endMinute")]
		public int EndMinute { get; set; }
	}

	/// <summary>
	/// A device alert SDT
	/// </summary>
	[DataContract]
	public class DeviceAlertSdt : AlertSdt
	{
		/// <summary>
		/// The device id
		/// </summary>
		[DataMember(Name = "hostId")]
		public int DeviceId { get; set; }
	}

	/// <summary>
	/// A website alert SDT
	/// </summary>
	[DataContract]
	public class WebsiteAlertSdt : AlertSdt
	{
		/// <summary>
		/// The website id
		/// </summary>
		[DataMember(Name = "websiteId")]
		public int DeviceId { get; set; }
	}

	/// <summary>
	/// A device group alert SDT
	/// </summary>
	[DataContract]
	public class DeviceGroupAlertSdt : AlertSdt
	{
		/// <summary>
		/// The device group id
		/// </summary>
		[DataMember(Name = "hostGroupId")]
		public int DeviceGroupId { get; set; }
	}

	/// <summary>
	/// A website group alert SDT
	/// </summary>
	[DataContract]
	public class WebsiteGroupAlertSdt : AlertSdt
	{
		/// <summary>
		/// The website group id
		/// </summary>
		[DataMember(Name = "websiteGroupId")]
		public int WebsiteGroupId { get; set; }
	}

	/// <summary>
	/// A device data source instance SDT
	/// </summary>
	[DataContract]
	public class DeviceDataSourceInstanceSdt : AlertSdt
	{
		/// <summary>
		/// The DataSourceInstance ID (note: these two are identical but both present)
		/// </summary>
		[DataMember(Name = "dsiId")]
		public int DsiId { get; set; }

		/// <summary>
		/// The DataSourceInstance ID (note: these two are identical but both present)
		/// </summary>
		[DataMember(Name = "dataSourceInstanceId")]
		public int DataSourceInstanceId { get; set; }
	}
}