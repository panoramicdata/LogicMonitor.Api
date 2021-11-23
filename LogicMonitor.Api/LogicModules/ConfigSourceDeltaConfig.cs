using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A ConfigSource's delta config
/// </summary>
[DataContract]
public class ConfigSourceDeltaConfig
{
	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	/// The content
	/// </summary>
	[DataMember(Name = "content")]
	public string Content { get; set; }

	/// <summary>
	/// The row index
	/// </summary>
	[DataMember(Name = "rowNo")]
	public string RowIndex { get; set; }
}
