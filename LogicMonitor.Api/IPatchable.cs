namespace LogicMonitor.Api
{
	/// <summary>
	///    Entities with patch support
	/// </summary>
	public interface IPatchable : IHasEndpoint
	{
		/// <summary>
		///    The entity's patch id
		/// </summary>
		int Id { get; }
	}
}