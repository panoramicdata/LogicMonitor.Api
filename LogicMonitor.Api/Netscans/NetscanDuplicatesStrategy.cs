using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Netscans
{
	/// <summary>
	///    The Netscan policy duplication strategy
	/// </summary>
	[DataContract(Name = "duplicate")]
	public class NetscanDuplicatesStrategy
	{
		/// <summary>
		///    The NetscanPolicyExcludeDuplicatesStrategy type
		/// </summary>
		[DataMember(Name = "type")]
		public NetscanExcludeDuplicatesStrategy Type { get; set; }

		/// <summary>
		///    The groups
		/// </summary>
		[DataMember(Name = "groups")]
		public List<object> Groups { get; set; }

		/// <summary>
		///    The collectors
		/// </summary>
		[DataMember(Name = "collectors")]
		public List<object> Collectors { get; set; }
	}
}