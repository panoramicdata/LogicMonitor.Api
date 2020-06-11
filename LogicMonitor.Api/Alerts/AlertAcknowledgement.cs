namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// An alert acknowledgement
	/// </summary>
	[DataContract]
	public class AlertAcknowledgement
	{
		/// <summary>
		/// The acknowledgement comment
		/// </summary>
		[DataMember(Name="ackComment")]
		public string AcknowledgementComment { get; set; }
	}
}
