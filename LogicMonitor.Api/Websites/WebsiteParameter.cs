using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites;

/// <summary>
/// A website parameter
/// </summary>
[DataContract]
public class WebsiteParameter : NamedEntity
{
	/// <summary>
	/// The website id
	/// </summary>
	[DataMember(Name = "websiteId")]
	public int WebsiteId { get; set; }

	/// <summary>
	/// The website id
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; }
}
