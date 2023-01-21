using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SaasSalesforceSOQLQueryCollectorAttributeV3
/// </summary>

[DataContract]
public class SaasSalesforceSOQLQueryCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set; }

	/// <summary>
	/// whereClause
	/// </summary>
	[DataMember(Name = "whereClause")]
	public string WhereClause { get; set; }

	/// <summary>
	/// groupByClause
	/// </summary>
	[DataMember(Name = "groupByClause")]
	public string GroupByClause { get; set; }

	/// <summary>
	/// soqlEntity
	/// </summary>
	[DataMember(Name = "soqlEntity")]
	public string SoqlEntity { get; set; }

	/// <summary>
	/// dateColumn
	/// </summary>
	[DataMember(Name = "dateColumn")]
	public string DateColumn { get; set; }

	/// <summary>
	/// instanceColumnName
	/// </summary>
	[DataMember(Name = "instanceColumnName")]
	public string InstanceColumnName { get; set; }
}
