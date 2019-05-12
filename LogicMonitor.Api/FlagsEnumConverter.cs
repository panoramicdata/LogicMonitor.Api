using Newtonsoft.Json;
using System;
using System.Linq;

namespace LogicMonitor.Api
{
	internal class FlagsEnumConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType.IsEnum && objectType.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var values = serializer.Deserialize<string[]>(reader);

			return values
				 .Select(x => Enum.Parse(objectType, x))
				 .Aggregate(0, (current, value) => current | (int)value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var enumArr = Enum.GetValues(value.GetType())
				 .Cast<int>()
				 .Where(x => (x & (int)value) == x)
				 .Select(x => Enum.ToObject(value.GetType(), x).ToString())
				 .ToArray();

			serializer.Serialize(writer, enumArr);
		}
	}
}