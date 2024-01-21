namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     Widget creation DTO
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public abstract class WidgetCreationDto<T> : CreationDto<T>, IHasName, IHasDescription where T : Widget
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
	public string DashboardIdString { get; set; } = string.Empty;

	/// <summary>
	///     The dashboard Id
	/// </summary>
	[JsonIgnore]
	public int DashboardId
	{
#pragma warning disable CS0618 // Type or member is obsolete
		get => int.Parse(DashboardIdString, CultureInfo.InvariantCulture);
		set => DashboardIdString = value.ToString(CultureInfo.InvariantCulture);
#pragma warning restore CS0618 // Type or member is obsolete
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
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///     The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///     The type
	/// </summary>
	[DataMember(Name = "type")]
	public abstract string Type { get; }
}
