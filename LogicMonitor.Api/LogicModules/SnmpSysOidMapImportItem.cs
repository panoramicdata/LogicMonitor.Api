using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{

	/// <summary>
	/// An SNMP SysOID Map ID and Name for import
	/// </summary>
	[DataContract]
	public class SnmpSysOidMapImportItem
	{
		/// <summary>
		/// The Local ID
		/// </summary>
		[DataMember(Name = "id")]
		public int Id { get; set; }

		/// <summary>
		/// The OID
		/// </summary>
		[DataMember(Name = "oid")]
		public string Oid { get; set; }
	}
}
