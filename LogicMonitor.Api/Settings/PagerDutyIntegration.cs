using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	///     An PagerDuty integration
	/// </summary>
	[DataContract]
	public class PagerDutyIntegration : HttpIntegration
	{
		/// <summary>
		/// Service Key
		/// </summary>
		[DataMember(Name = "serviceKey")]
		public string ServiceKey { get; set; }
	}

}