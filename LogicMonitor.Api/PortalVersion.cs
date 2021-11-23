using System.Runtime.Serialization;

namespace LogicMonitor.Api;

/// <summary>
/// The portal version information
/// </summary>
[DataContract]
public class PortalVersion
{
	/// <summary>
	/// The version
	/// </summary>
	[DataMember(Name = "version")]
	public Version Version { get; set; }

	/// <summary>
	/// The extra information
	/// </summary>
	[DataMember(Name = "extra")]
	public Extra Extra { get; set; }

	/// <summary>
	/// The hash
	/// </summary>
	[DataMember(Name = "hash")]
	public string Hash { get; set; }

	/// <summary>
	/// The build time
	/// </summary>
	[DataMember(Name = "buildAt")]
	public string BuildAt { get; set; }

	/// <summary>
	/// The branch
	/// </summary>
	[DataMember(Name = "branch")]
	public string Branch { get; set; }

	/// <summary>
	/// The result key
	/// </summary>
	[DataMember(Name = "resultKey")]
	public string ResultKey { get; set; }
}

/// <summary>
/// A version
/// </summary>
[DataContract]
public class Version
{
	/// <summary>
	/// The module
	/// </summary>
	[DataMember(Name = "module")]
	public string Module { get; set; }

	/// <summary>
	/// The major version
	/// </summary>
	[DataMember(Name = "major")]
	public int Major { get; set; }

	/// <summary>
	/// The minor version
	/// </summary>
	[DataMember(Name = "minor")]
	public int Minor { get; set; }

	/// <summary>
	/// The revision
	/// </summary>
	[DataMember(Name = "revision")]
	public int Revision { get; set; }

	/// <summary>
	/// The major version
	/// </summary>
	[DataMember(Name = "build")]
	public int Build { get; set; }
}

/// <summary>
/// Extra version information
/// </summary>
[DataContract]
public class Extra
{
	/// <summary>
	/// The database version
	/// </summary>
	[DataMember(Name = "dbVersion")]
	public int DatabaseVersion { get; set; }
}
