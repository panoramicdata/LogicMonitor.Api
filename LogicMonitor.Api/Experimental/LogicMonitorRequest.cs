namespace LogicMonitor.Api.Experimental;

internal class LogicMonitorRequest<T> where T : IHasEndpoint, new()
{
	public IReadOnlyCollection<string>? Properties { get; set; }

	public AdvancedFilter<T>? Filter { get; set; }

	public int Skip { get; set; }

	public int Take { get; set; } = int.MaxValue;

	public int PageSize { get; } = 300;
}