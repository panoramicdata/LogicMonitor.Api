using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// InheritanceProp
/// </summary>

[DataContract]
public class InheritanceProp : IdentifiedItem
{
	/// <summary>
	/// The fullpath of the property
	/// </summary>
	[DataMember(Name = "fullpath", IsRequired = false)]
	public string Fullpath { get; set; }

	/// <summary>
	/// The type of the parent property from which are inheriting
	/// </summary>
	[DataMember(Name = "type", IsRequired = false)]
	public string Type { get; set; }

	/// <summary>
	/// The property value for the group
	/// </summary>
	[DataMember(Name = "value", IsRequired = false)]
	public string Value { get; set; }
}
