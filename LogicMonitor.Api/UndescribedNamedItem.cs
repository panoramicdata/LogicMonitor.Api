using System.Runtime.Serialization;

namespace LogicMonitor.Api
{
	/// <summary>
	///    LogicMonitor undescribed named item
	/// </summary>
	[DataContract]
	public abstract class UndescribedNamedItem : IdentifiedItem
	{
		/// <summary>
		///    The LogicMonitor Name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		///    Equals override
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}

			var logicMonitorNamedEntity = obj as NamedItem;

			return base.Equals(logicMonitorNamedEntity)
				   && logicMonitorNamedEntity != null
				   && logicMonitorNamedEntity.Name == Name;
		}

		/// <summary>
		///    Get Hashcode override
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() => base.GetHashCode()
				   + (Name ?? string.Empty).GetHashCode();

		/// <summary>
		///    ToString override
		/// </summary>
		/// <returns>'Name (Id)'</returns>
		public override string ToString() => $"{Name} ({Id})";
	}
}