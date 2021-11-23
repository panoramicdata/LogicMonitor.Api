using System.Runtime.Serialization;

namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// A Website ops note scope
/// </summary>
[DataContract]
public class WebsiteOpsNoteScope : OpsNoteScope
{
	/// <summary>
	/// The Website id
	/// </summary>
	[DataMember(Name = "websiteId")]
	public int WebsiteId { get; set; }

	/// <summary>
	/// The website name
	/// </summary>
	[DataMember(Name = "websiteName")]
	public string WebsiteName { get; set; }
}
