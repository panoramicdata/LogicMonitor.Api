namespace LogicMonitor.Api.Data;

/// <summary>
///    An item of raw data
/// </summary>
[DataContract]
public class RawDataSet
{
	/// <summary>
	///    The data source name
	/// </summary>
	[DataMember(Name = "dataSourceName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	///    The data point names
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<string> DataPoints { get; set; } = [];

	/// <summary>
	///    The timestamps
	/// </summary>
	[DataMember(Name = "time")]
	public List<long> UtcTimeStamps { get; set; } = [];

	/// <summary>
	///    The values as objects (string or double)
	/// </summary>
	[DataMember(Name = "values")]
	public List<List<object>> ValuesAsObjects { get; set; } = [];

	/// <summary>
	///    The data point values
	/// </summary>
	[IgnoreDataMember]
	public List<List<double?>> Values => Enumerable
				.Range(0, ValuesAsObjects.Count)
				.Select(index => ValuesAsObjects[index]
					.Select(valueAsObject =>
						{
							if (double.TryParse(valueAsObject.ToString(), out var valueAsDouble))
							{
								return valueAsDouble;
							}

							return (double?)null;
						}
					)
					.ToList()
				)
				.ToList();

	/// <summary>
	///    The next page parameters
	/// </summary>
	[DataMember(Name = "nextPageParams")]
	public string NextPageParameters { get; set; } = string.Empty;
}
