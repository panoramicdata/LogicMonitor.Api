using System.Runtime.Serialization;

namespace LogicMonitor.Api
{
	/// <summary>
	/// Item identified by a string
	/// </summary>
	[DataContract]
	public abstract class StringIdentifiedItem : IdentifiedItemBase<string>
	{
	}
}