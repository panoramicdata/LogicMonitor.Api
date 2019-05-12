using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// Alert Status
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum AlertStatus
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// None
		/// </summary>
		[EnumMember(Value = "none")]
		None = 1,

		/// <summary>
		/// Unconfirmed warning none
		/// </summary>
		[EnumMember(Value = "unconfirmed-warning-none")]
		UnconfirmedWarningNone = 11,

		/// <summary>
		/// Unconfirmed warning none
		/// </summary>
		[EnumMember(Value = "unconfirmed-warn-none")]
		UnconfirmedWarnNone = UnconfirmedWarningNone,

		/// <summary>
		/// Unconfirmed warning Sdt
		/// </summary>
		[EnumMember(Value = "unconfirmed-warning-sdt")]
		UnconfirmedWarningSdt = 12,

		/// <summary>
		/// Unconfirmed warning Sdt
		/// </summary>
		[EnumMember(Value = "unconfirmed-warn-sdt")]
		UnconfirmedWarnSdt = UnconfirmedWarningSdt,

		/// <summary>
		/// Unconfirmed error none
		/// </summary>
		[EnumMember(Value = "unconfirmed-error-none")]
		UnconfirmedErrorNone = 13,

		/// <summary>
		/// Unconfirmed error Sdt
		/// </summary>
		[EnumMember(Value = "unconfirmed-error-sdt")]
		UnconfirmedErrorSdt = 14,

		/// <summary>
		/// Unconfirmed critical none
		/// </summary>
		[EnumMember(Value = "unconfirmed-critical-none")]
		UnconfirmedCriticalNone = 15,

		/// <summary>
		/// Unconfirmed critical Sdt
		/// </summary>
		[EnumMember(Value = "unconfirmed-critical-sdt")]
		UnconfirmedCriticalSdt = 16,

		/// <summary>
		/// Confirmed warning none
		/// </summary>
		[EnumMember(Value = "confirmed-warning-none")]
		ConfirmedWarningNone = 17,

		/// <summary>
		/// Confirmed warning none
		/// </summary>
		[EnumMember(Value = "confirmed-warn-none")]
		ConfirmedWarnNone = ConfirmedWarningNone,

		/// <summary>
		/// Confirmed warning Sdt
		/// </summary>
		[EnumMember(Value = "confirmed-warning-sdt")]
		ConfirmedWarningSdt = 18,

		/// <summary>
		/// Confirmed warning Sdt
		/// </summary>
		[EnumMember(Value = "confirmed-warn-sdt")]
		ConfirmedWarnSdt = ConfirmedWarningSdt,

		/// <summary>
		/// Confirmed error none
		/// </summary>
		[EnumMember(Value = "confirmed-error-none")]
		ConfirmedErrorNone = 19,

		/// <summary>
		/// Confirmed error Sdt
		/// </summary>
		[EnumMember(Value = "confirmed-error-sdt")]
		ConfirmedErrorSdt = 20,

		/// <summary>
		/// Confirmed critical none
		/// </summary>
		[EnumMember(Value = "confirmed-critical-none")]
		ConfirmedCriticalNone = 21,

		/// <summary>
		/// Confirmed critical Sdt
		/// </summary>
		[EnumMember(Value = "confirmed-critical-sdt")]
		ConfirmedCriticalSdt = 22,

		/// <summary>
		/// Alive
		/// </summary>
		[EnumMember(Value = "alive")]
		Alive = 23,

		/// <summary>
		/// Dead
		/// </summary>
		[EnumMember(Value = "dead")]
		Dead = 24
	}
}