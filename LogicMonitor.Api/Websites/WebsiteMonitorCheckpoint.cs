using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites
{
	/// <summary>
	/// Website monitor checkpoint
	/// </summary>
	[DataContract]
	public class WebsiteMonitorCheckpoint : NamedEntity, IHasEndpoint
	{
		/// <summary>
		/// The id
		/// </summary>
		[DataMember(Name = "id")]
		public int Id { get; set; }

		/// <summary>
		/// Whether it is enabled in root
		/// </summary>
		[DataMember(Name = "isEnabledInRoot")]
		public bool IsEnabledInRoot { get; set; }

		/// <summary>
		/// The geographic location
		/// </summary>
		[DataMember(Name = "geoinfo")]
		public string GeographicInformation { get; set; }

		/// <summary>
		/// The geographic location
		/// </summary>
		[DataMember(Name = "displayPrio")]
		public int DisplayPriority { get; set; }

		/// <inheritdoc />
		public string Endpoint() => "website/smcheckpoints";
	}
}