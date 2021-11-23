using LogicMonitor.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.OpsNotes;

/// <summary>
///    An operational note
/// </summary>
[DataContract]
public class OpsNote : StringIdentifiedItem, IHasEndpoint
{
	/// <summary>
	///    The OpsNote note
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; }

	/// <summary>
	///    The creator
	/// </summary>
	[DataMember(Name = "createdBy")]
	public string CreatedBy { get; set; }

	/// <summary>
	///    The timestamp of the OpsNote
	/// </summary>
	[DataMember(Name = "happenOnInSec")]
	public int HappenOnSeconds { get; set; }

	/// <summary>
	///    The scopes
	/// </summary>
	[DataMember(Name = "scopes")]
	public List<OpsNoteScope> Scopes { get; set; }

	/// <summary>
	///    The tags
	/// </summary>
	[DataMember(Name = "tags")]
	public List<OpsNoteTag> Tags { get; set; }

	/// <summary>
	///    The DateTime version of HappenOnTimeStampUtc
	/// </summary>
	[IgnoreDataMember]
	public DateTime HappenOnUtc => HappenOnSeconds.ToDateTimeUtc();

	/// <inheritdoc />
	public string Endpoint() => "setting/opsnotes";
}
