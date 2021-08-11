namespace LogicMonitor.Api
{
	/// <summary>
	///    Provides the ability to get by id and by page, create, delete and update
	/// </summary>
	public interface IHasEndpoint
	{
		/// <summary>
		///    The endpoint
		/// </summary>
		string Endpoint();
	}
}