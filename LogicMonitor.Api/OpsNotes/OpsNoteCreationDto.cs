using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.OpsNotes
{
	/// <summary>
	/// An OpsNote creation DTO
	/// </summary>
	[DataContract]
	public class OpsNoteCreationDto : CreationDto<OpsNote>
	{
		/// <inheritdoc />
		public OpsNoteCreationDto()
		{
			Tags = new List<OpsNoteTagCreationDto>();
		}

		/// <summary>
		/// The note
		/// </summary>
		[DataMember(Name = "note")]
		public string Note { get; set; }

		/// <summary>
		/// The note
		/// </summary>
		[DataMember(Name = "happenOnInSec")]
		public int DateTimeUtcSeconds { get; set; }

		/// <summary>
		/// Tags
		/// </summary>
		[DataMember(Name = "tags")]
		public List<OpsNoteTagCreationDto> Tags { get; set; }

		/// <summary>
		/// Scopes
		/// </summary>
		[DataMember(Name = "scopes")]
		public List<OpsNoteScopeCreationDto> Scopes { get; set; }
	}
}