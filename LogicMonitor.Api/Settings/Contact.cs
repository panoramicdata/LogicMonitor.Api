using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings;

/// <summary>
///     Contact information
/// </summary>
[DataContract]
public class Contact
{
	/// <summary>
	///     Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	///     E-mail address
	/// </summary>
	[DataMember(Name = "email")]
	public string EmailAddress { get; set; }

	/// <summary>
	///     Phone number
	/// </summary>
	[DataMember(Name = "phone")]
	public string PhoneNumber { get; set; }
}
