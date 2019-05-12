using Newtonsoft.Json;
using System.IO;

namespace LogicMonitor.Api
{
	internal class LogicMonitorJsonReader : JsonTextReader
	{
		public LogicMonitorJsonReader(TextReader reader) : base(reader)
		{
		}

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
}