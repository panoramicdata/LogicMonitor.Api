namespace LogicMonitor.Api
{
	/// <summary>
	///    Provides the ability to get by id and by page
	/// </summary>
	public interface IHasSingletonEndpoint
	{
		/// <summary>
		///    The endpoint
		/// </summary>
		/// <returns></returns>
		string Endpoint();
	}
}