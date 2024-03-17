
namespace LogicMonitor.Api.Experimental;

internal class QueryStringBuilder<T>(string subUrl) where T : IHasEndpoint, new()
{
	private readonly StringBuilder _stringBuilder = new(subUrl);
	internal bool HasQuestionMark { get; private set; }

	public void Append(string key, IReadOnlyCollection<string>? values)
	{
		if (values == null || values.Count == 0)
		{
			return;
		}

		AddDelimiter();
		_stringBuilder.Append($"{key}={string.Join(",", values.Select(Api.LogicMonitorClient.GetSerializationName<T>))}");
	}

	private void AddDelimiter()
	{
		if (HasQuestionMark)
		{
			_stringBuilder.Append('&');
			return;
		}

		_stringBuilder.Append('?');
		HasQuestionMark = true;
	}

	public void Append(string key, AdvancedFilter<T>? filter)
	{
		if (filter == null)
		{
			return;
		}

		AddDelimiter();
		_stringBuilder.Append($"{key}={filter}");
	}

	public void Append(string key, int value)
	{
		AddDelimiter();
		_stringBuilder.Append($"{key}={value}");
	}

	public override string ToString()
		=> _stringBuilder.ToString();
}