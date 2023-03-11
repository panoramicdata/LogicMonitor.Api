namespace LogicMonitor.Api.Settings;

/// <summary>
///     Single sign-on (SSO) information
/// </summary>
[DataContract]
public class SingleSignOn : IHasSingletonEndpoint
{
	/// <summary>
	///     IdPMetadata
	/// </summary>
	[DataMember(Name = "IdPMetadata")]
	public string IdPMetadata { get; set; } = string.Empty;

	/// <summary>
	///     SAML Version
	/// </summary>
	[DataMember(Name = "samlVersion")]
	public string SamlVersion { get; set; } = string.Empty;

	/// <summary>
	///     Cookie expire seconds
	/// </summary>
	[DataMember(Name = "cookieExpireSeconds")]
	public int CookieExpireSeconds { get; set; }

	/// <summary>
	///     Default role id
	/// </summary>
	[DataMember(Name = "defaultRole")]
	public int DefaultRoleId { get; set; }

	/// <summary>
	///     Enable SSO status
	/// </summary>
	[DataMember(Name = "enableSSO")]
	public bool Enabled { get; set; }

	/// <summary>
	///     Enable SLO status
	/// </summary>
	[DataMember(Name = "enableSLO")]
	public bool EnableSlo { get; set; }

	/// <summary>
	///     Restricted status
	/// </summary>
	[DataMember(Name = "restrictSSO")]
	public bool Restricted { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/integrations/sso";
}
