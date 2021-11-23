using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A Netflow filter
	/// </summary>
	[DataContract]
	public class NetflowFilter
	{
		private string _direction = "bidirectional";

		/// <summary>
		/// The ifIdx
		/// </summary>
		[DataMember(Name = "ifIdx")]
		public int InterfaceIndex { get; set; } = -1;

		/// <summary>
		/// The qosType
		/// </summary>
		[DataMember(Name = "qosType")]
		public string QosType { get; set; } = "all";

		/// <summary>
		/// The nodeA
		/// </summary>
		[DataMember(Name = "nodeA")]
		public string NodeA { get; set; } = "";

		/// <summary>
		/// The nodeB
		/// </summary>
		[DataMember(Name = "nodeB")]
		public string NodeB { get; set; } = "";

		/// <summary>
		/// The direction
		/// </summary>
		[DataMember(Name = "direction")]
		public string Direction
		{
			get => _direction;
			set
			{
				_direction = value switch
				{
					"" => "bidirectional",
					"bidirectional" or "leftwards" or "rightwards" => value,
					_ => throw new ArgumentOutOfRangeException($"Unexpected direction {value}"),
				};
			}
		}

		/// <summary>
		/// The protocol
		/// </summary>
		[DataMember(Name = "protocol")]
		public string Protocol { get; set; } = "tcp,udp";

		/// <summary>
		/// The ports
		/// </summary>
		[DataMember(Name = "ports")]
		public string Ports { get; set; } = "";

		/// <summary>
		/// The top
		/// </summary>
		[DataMember(Name = "top")]
		public int Top { get; set; } = 10;

		/// <summary>
		/// The ifName
		/// </summary>
		[DataMember(Name = "ifName")]
		public string InterfaceName { get; set; }

		/// <summary>
		/// The IP version
		/// </summary>
		[DataMember(Name = "ipVersion")]
		public string IpVersion { get; set; }

		/// <summary>
		/// The conversation
		/// </summary>
		[DataMember(Name = "conversation")]
		public List<NetflowFilterConversation> Conversations { get; set; }

		/// <summary>
		/// Converts to a URL encoded string for the query URL
		/// </summary>
		internal string AsUrlEncodedString()
		=> HttpUtility.UrlEncode(JsonConvert.SerializeObject(this));

		/// <summary>
		/// Validates the netflow filter
		/// </summary>
		public void Validate()
		{
			if (InterfaceIndex < -1)
			{
				throw new ArgumentException("InterfaceIndex must be -1 for all interfaces or >= 0");
			}
		}
	}
}