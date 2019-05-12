namespace LogicMonitor.Api
{
	/// <summary>
	/// A validation issue
	/// </summary>
	public abstract class ValidationIssue
	{
		/// <summary>
		/// The message
		/// </summary>
		public string Message { get; }
	}
}