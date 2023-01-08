namespace LogicMonitor.Api.Reports;

public class Metric
{
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; }

	[DataMember(Name = "instances")]
	public string Instances { get; set; }

	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; }

	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }
}
