using LogicMonitor.Api.Dashboards;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LogicMonitor.Api.Converters;

internal class MapPointConverter : JsonCreationConverter<MapPoint>
{
	protected override MapPoint Create(Type objectType, JObject jObject)
	{
		var type = jObject["type"].Value<string>().ToLowerInvariant();
		return type switch
		{
			"device" => new DeviceMapPoint(),
			"group" => new DeviceGroupMapPoint(),
			_ => throw new NotSupportedException($"MapPointConverter.cs needs updating to include {type}."),
		};
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotSupportedException();
}
