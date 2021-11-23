using LogicMonitor.Api.OpsNotes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LogicMonitor.Api.Converters;

internal class OpsNoteScopeConverter : JsonCreationConverter<OpsNoteScope>
{
	protected override OpsNoteScope Create(Type objectType, JObject jObject)
	{
		var type = jObject["type"].Value<string>().ToLowerInvariant();
		return type switch
		{
			"device" => new DeviceOpsNoteScope(),
			"devicegroup" => new DeviceGroupOpsNoteScope(),
			"website" => new WebsiteOpsNoteScope(),
			"websitegroup" => new WebsiteGroupOpsNoteScope(),
			"groupall" => new AllGroupOpsNoteScope(),
			_ => throw new NotSupportedException($"OpsNoteScopeConverter.cs needs updating to include {type}."),
		};
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotSupportedException();
}
