using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A collection of LogicModule Updates
	/// </summary>
	[DataContract]
	public class LogicModuleUpdateCollection
	{
		/// <summary>
		/// The total number of items
		/// </summary>
		[DataMember(Name = "total")]
		public int Total { get; set; }

		/// <summary>
		/// The LogicModule Update items
		/// </summary>
		[DataMember(Name = "items")]
		public List<LogicModuleUpdate> Items { get; set; }

		/// <summary>
		/// The search ID
		/// </summary>
		[DataMember(Name = "searchId")]
		public string SearchId { get; set; }

		/// <summary>
		/// Is Min
		/// </summary>
		[DataMember(Name = "isMin")]
		public bool IsMin { get; set; }
	}
}