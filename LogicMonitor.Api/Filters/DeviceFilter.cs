using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Filters;

/// <summary>
/// DeviceFilter
/// </summary>
[DataContract]
public class DeviceFilter
{
	/// <summary>
	/// comment
	/// </summary>
	[DataMember(Name = "comment", IsRequired = false)]
	public string Comment { get; set; } = string.Empty;

	/// <summary>
	/// attribute
	/// </summary>
	[DataMember(Name = "attribute", IsRequired = true)]
	public string Attribute { get; set; } = null!;

	/// <summary>
	/// operation
	/// </summary>
	[DataMember(Name = "operation", IsRequired = false)]
	public string Operation { get; set; } = string.Empty;

	/// <summary>
	/// value
	/// </summary>
	[DataMember(Name = "value", IsRequired = false)]
	public string Value { get; set; } = string.Empty;
}
