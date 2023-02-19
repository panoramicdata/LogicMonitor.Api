using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// AutoDiscoveryFilterOperation
/// </summary>

[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum AutoDiscoveryFilterOperation
{
	/// <summary>
	/// Equal
	/// </summary>
	[EnumMember(Value = "Equal")]
	Equal,

	/// <summary>
	/// NotEqual
	/// </summary>
	[EnumMember(Value = "NotEqual")]
	NotEqual,

	/// <summary>
	/// GreaterThan
	/// </summary>
	[EnumMember(Value = "GreaterThan")]
	GreaterThan,

	/// <summary>
	/// GreaterEqual
	/// </summary>
	[EnumMember(Value = "GreaterEqual")]
	GreaterEqual,

	/// <summary>
	/// LessThan
	/// </summary>
	[EnumMember(Value = "LessThan")]
	LessThan,

	/// <summary>
	/// LessEqual
	/// </summary>
	[EnumMember(Value = "LessEqual")]
	LessEqual,

	/// <summary>
	/// Contain
	/// </summary>
	[EnumMember(Value = "Contain")]
	Contain,

	/// <summary>
	/// NotContain
	/// </summary>
	[EnumMember(Value = "NotContain")]
	NotContain,

	/// <summary>
	/// Exist
	/// </summary>
	[EnumMember(Value = "Exist")]
	Exist,

	/// <summary>
	/// NotExist
	/// </summary>
	[EnumMember(Value = "NotExist")]
	NotExist,

	/// <summary>
	/// RegexMatch
	/// </summary>
	[EnumMember(Value = "RegexMatch")]
	RegexMatch,

	/// <summary>
	/// RegexNotMatch
	/// </summary>
	[EnumMember(Value = "RegexNotMatch")]
	RegexNotMatch
}
