using System.Runtime.Serialization;

namespace LogicMonitor.Api
{
	/// <summary>
	/// An entity with an integer LogicMonitor Id
	/// </summary>
	[DataContract]
	public class IdentifiedItem : IdentifiedItemBase<int>
	{
	}
}