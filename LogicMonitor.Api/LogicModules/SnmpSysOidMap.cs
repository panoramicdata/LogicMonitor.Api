using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// An AppliesTo Function
	/// </summary>
	[DataContract]
	public class SnmpSysOidMap : IdentifiedItem, IHasEndpoint
	{
		/// <summary>
		/// The parameters
		/// </summary>
		[DataMember(Name = "oid")]
		public string Oid { get; set; }

		/// <summary>
		/// The categories
		/// </summary>
		[DataMember(Name = "categories")]
		public string Categories { get; set; }

		/// <summary>
		/// Published
		/// </summary>
		[DataMember(Name = "published")]
		public int Published { get; set; }

		/// <summary>
		/// ToString override
		/// </summary>
		/// <returns>'Id : Oid'</returns>
		public override string ToString() => $"{Id} : {Oid}";

		/// <summary>
		///    The endpoint
		/// </summary>
		public string Endpoint() => "setting/oids";
	}
}