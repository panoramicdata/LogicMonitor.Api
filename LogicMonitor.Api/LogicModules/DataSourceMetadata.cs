using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	///     LogicModule Metadata
	/// </summary>
	[DataContract]
	public class LogicModuleMetadata
	{
		/// <summary>
		///     The namespace
		/// </summary>
		[DataMember(Name = "namespace")]
		public string Namespace { get; set; }

		/// <summary>
		///     The registryVersion
		/// </summary>
		[DataMember(Name = "registryVersion")]
		public string RegistryVersion { get; set; }

		/// <summary>
		///     The quality
		/// </summary>
		[DataMember(Name = "quality")]
		public string Quality { get; set; }

		/// <summary>
		///     The LM Locator
		/// </summary>
		[DataMember(Name = "lmLocator")]
		public string LmLocator { get; set; }

		/// <inheritdoc />
		/// <returns>'Id : Name - DisplayedAs'</returns>
		public override string ToString() => $"{Namespace}.{LmLocator}/v{RegistryVersion} ({Quality})";
	}
}