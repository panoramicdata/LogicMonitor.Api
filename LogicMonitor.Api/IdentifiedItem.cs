using LogicMonitor.Api.Attributes;
using System.Runtime.Serialization;

namespace LogicMonitor.Api
{
	/// <summary>
	/// An entity with an integer LogicMonitor Id
	/// </summary>
	[DataContract]
	public class IdentifiedItem
	{
		/// <summary>
		/// LogicMonitor Id
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "id")]
		public int Id { get; set; }

		/// <summary>
		/// Equals override
		/// </summary>
		/// <param name="obj">The comparison object</param>
		/// <returns>True if equal</returns>
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			return Id == ((IdentifiedItem)obj).Id;
		}

		/// <summary>
		/// Hashcode override
		/// </summary>
		/// <returns>The hashcode</returns>
		public override int GetHashCode() => Id * 23;
	}
}