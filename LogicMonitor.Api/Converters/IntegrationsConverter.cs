using LogicMonitor.Api.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LogicMonitor.Api.Converters
{
	internal class IntegrationsConverter : JsonCreationConverter<Integration>
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotSupportedException();

		protected override Integration Create(Type objectType, JObject jObject)
		{
			var integration = jObject["type"].Value<string>().ToLowerInvariant();
			switch (integration)
			{
				case "slack":
					return new SlackIntegration();

				case "http":
					return new HttpIntegration();

				case "servicenow":
					return new ServiceNowIntegration();

				case "email":
					return new EmailIntegration();

				case "autotask":
					return new AutoTaskIntegration();

				default:
					throw new NotSupportedException($"{integration} deserialization not supported.  IntegrationsConverter.cs needs updating.");
			}
		}
	}
}