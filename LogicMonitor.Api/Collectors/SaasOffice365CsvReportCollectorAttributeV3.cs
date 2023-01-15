using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SaasOffice365CsvReportCollectorAttributeV3
/// </summary>

[DataContract]
public class SaasOffice365CsvReportCollectorAttributeV3 : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "period")]
	public int Period { get; set;  }

	/// <summary>
	/// Report name
	/// </summary>
	[DataMember(Name = "reportName")]
	public string ReportName { get; set; }

	/// <summary>
	/// instanceColumnName
	/// </summary>
	[DataMember(Name = "instanceColumnName")]
	public string InstanceColumnName { get; set; }
}
