using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// PropertyMatchRule
/// </summary>

[DataContract]
public class PropertyMatchRule
{
	/// <summary>
	/// underscore
	/// </summary>
	[DataMember(Name = "underscore", IsRequired=false)]
	public bool Underscore { get; set; }

	/// <summary>
	/// caseInsensitive
	/// </summary>
	[DataMember(Name = "caseInsensitive", IsRequired = false)]
	public bool CaseInsensitive { get; set; }
}
