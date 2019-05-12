using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api
{
	/// <summary>
	/// A Page of objects, together with the total count and the search id
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DataContract]
	public class Page<T>
	{
		/// <summary>
		/// The item count
		/// </summary>
		[DataMember(Name = "total")]
		public int TotalCount { get; set; }

		/// <summary>
		/// The Search Id
		/// </summary>
		[DataMember(Name = "searchId")]
		public string SearchId { get; set; }

		/// <summary>
		/// Whether it is min
		/// </summary>
		[DataMember(Name = "isMin")]
		public bool IsMin { get; set; }

		/// <summary>
		/// The items
		/// </summary>
		[DataMember(Name = "items")]
		public List<T> Items { get; set; }
	}
}