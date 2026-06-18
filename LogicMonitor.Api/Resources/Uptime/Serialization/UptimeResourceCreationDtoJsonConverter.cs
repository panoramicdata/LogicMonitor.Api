namespace LogicMonitor.Api.Resources.Uptime.Serialization;

/// <summary>
/// Newtonsoft converter that flattens an Uptime creation DTO into the <c>device/devices</c> POST body.
/// Write-only: the response is deserialized into the resource type, not back into the DTO.
/// </summary>
internal sealed class UptimeResourceCreationDtoJsonConverter : JsonConverter
{
	public override bool CanConvert(Type objectType) => typeof(IUptimeCheckDefinition).IsAssignableFrom(objectType);

	public override bool CanRead => false;

	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		=> throw new NotSupportedException($"{nameof(UptimeResourceCreationDtoJsonConverter)} is write-only.");

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
		if (value is not IUptimeCheckDefinition definition)
		{
			writer.WriteNull();
			return;
		}

		// Creation has no id.
		UptimeResourceWireMapper.Write(writer, definition, id: null);
	}
}
