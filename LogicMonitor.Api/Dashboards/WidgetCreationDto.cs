using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	///     Widget creation DTO
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DataContract]
	public abstract class WidgetCreationDto<T> : CreationDto<T> where T : Widget
	{
		/// <summary>
		///     The refresh periodicity in minutes (as a string)
		/// </summary>
		[DataMember(Name = "interval")]
		public WidgetInterval RefreshIntervalMinutes { get; set; }

		/// <summary>
		///     The widget theme
		/// </summary>
		[DataMember(Name = "theme")]
		public WidgetTheme Theme { get; set; }

		/// <summary>
		///     The widget theme
		/// </summary>
		[DataMember(Name = "userPermission")]
		public UserPermission UserPermission { get; set; } = UserPermission.Write;

		/// <summary>
		///     The dashboard Id as a string
		/// </summary>
		[DataMember(Name = "dashboardId")]
		[Obsolete("Use DashboardId instead.  This property is for serialization/deserialization only.")]
		public string DashboardIdString { get; set; }

		/// <summary>
		///     The dashboard Id
		/// </summary>
		[JsonIgnore]
		public int DashboardId
		{
#pragma warning disable 618
			get => int.Parse(DashboardIdString);
			set => DashboardIdString = value.ToString();
#pragma warning restore 618
		}

		///// <summary>
		/////    The dashboard Id as a string
		///// </summary>
		//[DataMember(Name = "dashboardId")]
		//public string DashboardId { get; set; }

		/// <summary>
		///     The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		///     The description
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		/// <summary>
		///     The type
		/// </summary>
		[DataMember(Name = "type")]
		public abstract string Type { get; }
	}
}