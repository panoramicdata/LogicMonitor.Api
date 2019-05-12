using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	///     An integration
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(IntegrationsConverter))]
	[DebuggerDisplay("{Type}:{Name}")]
	public class Integration : NamedItem, IHasEndpoint
	{
		/// <summary>
		///     The integration type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; }

		/// <summary>
		///     Extra configuration
		/// </summary>
		[DataMember(Name = "extra")]
		public string Extra { get; set; }

		/// <summary>
		///     The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "setting/integrations";
	}
}