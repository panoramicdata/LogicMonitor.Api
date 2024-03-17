namespace LogicMonitor.Api.Experimental;

internal class PagingRequest<T> where T : IHasEndpoint, new()
{
	private readonly bool _hasQuestionMark;
	private readonly string _baseQuery;
	private readonly int _pageSize;
	private int _skip;

	internal PagingRequest(LogicMonitorRequest<T> request)
	{
		_skip = request.Skip;
		_pageSize = request.PageSize;
		var queryStringBuilder = new QueryStringBuilder<T>("rest/" + new T().Endpoint());
		queryStringBuilder.Append("fields", request.Properties);
		queryStringBuilder.Append("filter", request.Filter);
		queryStringBuilder.Append("size", Math.Min(request.Take, 300));
		_hasQuestionMark = queryStringBuilder.HasQuestionMark;
		_baseQuery = queryStringBuilder.ToString();
	}

	internal void IncrementPage() => _skip += _pageSize;

	public override string ToString() => $"{_baseQuery}{(_hasQuestionMark ? "&" : "?")}offset={_skip}";
}