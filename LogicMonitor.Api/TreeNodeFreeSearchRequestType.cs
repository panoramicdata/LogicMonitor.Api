using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api;

/// <summary>
/// A tree node free search request type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum TreeNodeFreeSearchRequestType
{
	/// <summary>
	/// A tree node free search
	/// </summary>
	[EnumMember(Value = "treeNodeFreeSearch")]
	TreeNodeFreeSearch
}
