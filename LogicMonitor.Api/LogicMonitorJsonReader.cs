namespace LogicMonitor.Api;

internal class LogicMonitorJsonReader(TextReader reader) : JsonTextReader(reader)
{
	public override bool Read()
	{
		var hasToken = base.Read();

		if (hasToken && TokenType == JsonToken.PropertyName && Value?.Equals("type") == true)
		{
			SetToken(JsonToken.PropertyName, "$type");
		}

		return hasToken;
	}
}
