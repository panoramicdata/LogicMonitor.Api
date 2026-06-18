namespace LogicMonitor.Api.Resources.Uptime.Serialization;

/// <summary>
/// Newtonsoft converter that maps an <see cref="UptimeResource"/> to/from the <c>device/devices</c> wire
/// format. Because it takes full control of (de)serialization it also bypasses the strict
/// <c>MissingMemberHandling.Error</c> used elsewhere - we read only the fields we care about and ignore
/// the (large) remainder of the device payload.
/// </summary>
internal sealed class UptimeResourceJsonConverter : JsonConverter
{
	public override bool CanConvert(Type objectType) => typeof(UptimeResource).IsAssignableFrom(objectType);

	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}

		var device = JObject.Load(reader);
		return UptimeResourceWireMapper.Read(device, objectType);
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
		if (value is not UptimeResource resource)
		{
			writer.WriteNull();
			return;
		}

		UptimeResourceWireMapper.Write(writer, resource, resource.Id);
	}
}

