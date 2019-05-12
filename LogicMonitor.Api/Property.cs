using LogicMonitor.Api.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api
{
	/// <summary>
	/// A simple property
	/// </summary>
	[DataContract]
	public class InstanceProperty : Property
	{
		/// <summary>
		///    The property Id
		/// </summary>
		[DataMember(Name = "Id")]
		public string Id { get; set; }
	}

	/// <summary>
	/// A simple property
	/// </summary>
	[DataContract]
	public class Property
	{
		/// <summary>
		///    The property name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		///    The property value
		/// </summary>
		[DataMember(Name = "value")]
		public string Value { get; set; }

		/// <summary>
		///    The property type
		/// </summary>
		[DataMember(Name = "type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public PropertyType Type { get; set; }

		/// <summary>
		///    The list of inherited items
		/// </summary>
		[DataMember(Name = "inheritList")]
		public List<InheritedItem> InheritList { get; set; }

		/// <inheritdoc />
		public override string ToString() => $"{Type}: {Name}={Value}";
	}
}