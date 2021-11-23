using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A widget theme
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum WidgetTheme
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "")]
	Unknown,

	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "newBorderGray")]
	NewBorderGray,

	/// <summary>
	/// newBorderBlue
	/// </summary>
	[EnumMember(Value = "newBorderBlue")]
	NewBorderBlue,

	/// <summary>
	/// newBorderDarkBlue
	/// </summary>
	[EnumMember(Value = "newBorderDarkBlue")]
	NewBorderDarkBlue,

	/// <summary>
	/// newSolidGray
	/// </summary>
	[EnumMember(Value = "newSolidGray")]
	NewSolidGray,

	/// <summary>
	/// newSolidBlue
	/// </summary>
	[EnumMember(Value = "newSolidBlue")]
	NewSolidBlue,

	/// <summary>
	/// newSolidDarkBlue
	/// </summary>
	[EnumMember(Value = "newSolidDarkBlue")]
	NewSolidDarkBlue,

	/// <summary>
	/// newSimpleGray
	/// </summary>
	[EnumMember(Value = "newSimpleGray")]
	NewSimpleGray,

	/// <summary>
	/// newSimpleBlue
	/// </summary>
	[EnumMember(Value = "newSimpleBlue")]
	NewSimpleBlue,

	/// <summary>
	/// newSimpleDarkBlue
	/// </summary>
	[EnumMember(Value = "newSimpleDarkBlue")]
	NewSimpleDarkBlue,

	/// <summary>
	/// borderGray
	/// </summary>
	[EnumMember(Value = "borderGray")]
	BorderGray,

	/// <summary>
	/// borderBlue
	/// </summary>
	[EnumMember(Value = "borderBlue")]
	BorderBlue,

	/// <summary>
	/// borderPurple
	/// </summary>
	[EnumMember(Value = "borderPurple")]
	BorderPurple,

	/// <summary>
	/// solidGray
	/// </summary>
	[EnumMember(Value = "solidGray")]
	SolidGray,

	/// <summary>
	/// solidPurple
	/// </summary>
	[EnumMember(Value = "solidPurple")]
	SolidPurple,

	/// <summary>
	/// simpleGray
	/// </summary>
	[EnumMember(Value = "simpleGray")]
	SimpleGray,

	/// <summary>
	/// simpleBlue
	/// </summary>
	[EnumMember(Value = "simpleBlue")]
	SimpleBlue,

	/// <summary>
	/// simplePurple
	/// </summary>
	[EnumMember(Value = "simplePurple")]
	SimplePurple,
}
