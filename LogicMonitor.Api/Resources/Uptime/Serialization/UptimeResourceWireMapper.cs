namespace LogicMonitor.Api.Resources.Uptime.Serialization;

/// <summary>
/// Translates between the strongly-typed Uptime surface and the <c>device/devices</c> wire format
/// (top-level device fields, custom properties and, for ping checks, the nested
/// <c>website.private.serviceParameters</c> JSON blob). Both the resource converter and the creation-DTO
/// converter delegate here so there is a single source of truth for the mapping.
/// </summary>
internal static class UptimeResourceWireMapper
{
	private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

	#region Write

	/// <summary>
	/// Writes the device JSON body for a check definition. <paramref name="id"/> is emitted only for an
	/// update of an existing resource.
	/// </summary>
	public static void Write(JsonWriter writer, IUptimeCheckDefinition definition, int? id)
		=> BuildDevice(definition, id).WriteTo(writer);

	private static JObject BuildDevice(IUptimeCheckDefinition definition, int? id)
	{
		var displayName = string.IsNullOrWhiteSpace(definition.DisplayName) ? definition.Name : definition.DisplayName;
		var isPing = definition is IPingCheckDefinition;

		// The v3 Uptime creation contract (X-Version: 3, device/devices?type=uptime{ping,web}check) expects a
		// flat, structured payload — NOT the legacy customProperties/website.private.serviceParameters blob.
		// Only this shape makes the server register the device as an Uptime resource (system.device.provider =
		// Uptime, system.uptime.type = internal), which is what causes the Ping_Check_*/Web_Check_* DataSources
		// to apply and data to be collected. Verified against a live LM Uptime portal.
		var device = new JObject
		{
			["type"] = isPing ? "uptimepingcheck" : "uptimewebcheck",
			["model"] = "websiteDevice",
			["deviceType"] = (int)definition.ResourceType,
			["id"] = id ?? 0,
			["name"] = definition.Name,
			["displayName"] = displayName,
			["description"] = definition.Description,
			["isInternal"] = definition.IsInternal,
			["disableAlerting"] = definition.DisableAlerting,
			["individualSmAlertEnable"] = definition.Alerting.IndividualCheckpointAlertsEnabled,
			["individualAlertLevel"] = LevelToWire(definition.Alerting.IndividualAlertLevel),
			["overallAlertLevel"] = LevelToWire(definition.Alerting.OverallAlertLevel),
			["pollingInterval"] = definition.PollingIntervalMinutes,
			["transition"] = definition.Alerting.FailedCheckCountBeforeAlerting,
			["globalSmAlertCond"] = (int)definition.Alerting.AlertCondition,
			["useDefaultLocationSetting"] = definition.UseDefaultLocationSetting,
			["useDefaultAlertSetting"] = definition.UseDefaultAlertSetting,
			// Internal checks run from real Collectors (collectorIds); external checks from Site Monitor Groups (smgIds).
			["testLocation"] = new JObject
			{
				["collectorIds"] = new JArray(definition.IsInternal ? definition.SyntheticsCollectorIds : []),
				["smgIds"] = new JArray(definition.IsInternal ? [] : definition.TestLocation.SmgIds),
			},
			["properties"] = new JArray(),
		};

		if (!string.IsNullOrWhiteSpace(definition.ResourceGroupIds))
		{
			device["groupIds"] = new JArray(ParseGroupIds(definition.ResourceGroupIds));
		}

		switch (definition)
		{
			case IPingCheckDefinition ping:
				WritePing(device, ping);
				break;
			case IWebCheckDefinition web:
				WriteWeb(device, web);
				break;
		}

		return device;
	}

	private static int[] ParseGroupIds(string resourceGroupIds)
		=> resourceGroupIds
			.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
			.Select(s => int.TryParse(s, NumberStyles.Integer, Invariant, out var v) ? v : (int?)null)
			.Where(v => v.HasValue)
			.Select(v => v!.Value)
			.ToArray();

	private static void WritePing(JObject device, IPingCheckDefinition ping)
	{
		device["host"] = ping.HostName;
		device["count"] = ping.PacketCount;
		device["percentPktsNotReceiveInTime"] = ping.PercentPacketsNotReceivedInTime;
		device["timeoutInMSPktsNotReceive"] = ping.TimeoutMs;
	}

	private static void WriteWeb(JObject device, IWebCheckDefinition web)
	{
		var domain = string.IsNullOrWhiteSpace(web.Domain) ? web.HostName : web.Domain;

		device["domain"] = domain;
		device["schema"] = SchemeToWire(web.Scheme);
		device["ignoreSSL"] = web.IgnoreSsl;
		device["pageLoadAlertTimeInMS"] = web.PageLoadAlertTimeMs;
		device["alertExpr"] = web.AlertExpression;
		device["triggerSSLStatusAlert"] = web.TriggerSslStatusAlerts;
		device["triggerSSLExpirationAlert"] = web.TriggerSslExpirationAlerts;

		var steps = web.Steps.Count > 0 ? web.Steps : [new UptimeWebCheckStep()];
		var stepArray = new JArray();
		for (var i = 0; i < steps.Count; i++)
		{
			stepArray.Add(BuildWebStep(steps[i], web.IsInternal, i));
		}

		device["steps"] = stepArray;
	}

	// v3 web step. Internal checks use step type "script"; external checks use "config".
	private static JObject BuildWebStep(UptimeWebCheckStep step, bool isInternal, int sequence) => new()
	{
		["type"] = isInternal ? "script" : "config",
		["enable"] = step.Enabled,
		["useDefaultRoot"] = step.UseDefaultRoot,
		["url"] = step.Url ?? string.Empty,
		["HTTPVersion"] = step.HttpVersion,
		["HTTPMethod"] = step.HttpMethod,
		["name"] = string.IsNullOrEmpty(step.Name) ? $"__step{sequence.ToString(Invariant)}" : step.Name,
		["followRedirection"] = step.FollowRedirection,
		["fullpageLoad"] = step.FullPageLoad,
		["requireAuth"] = false,
		["auth"] = new JObject { ["domain"] = string.Empty, ["password"] = string.Empty, ["type"] = "basic", ["userName"] = string.Empty },
		["HTTPHeaders"] = string.Empty,
		["HTTPBody"] = string.Empty,
		["timeout"] = 30,
		["reqType"] = "config",
		["respType"] = "config",
		["matchType"] = "plain",
		["keyword"] = step.Keyword,
		["invertMatch"] = "false",
		["statusCode"] = step.StatusCode,
		["path"] = string.Empty,
	};

	#endregion

	#region Read

	/// <summary>
	/// Builds a strongly-typed resource from a device JSON response.
	/// </summary>
	public static UptimeResource Read(JObject device, Type objectType)
	{
		var resource = CreateInstance(device, objectType);
		PopulateCommon(resource, device);

		switch (resource)
		{
			case PingCheckResource ping:
				PopulatePing(ping, device);
				break;
			case WebCheckResource web:
				PopulateWeb(web, device);
				break;
		}

		return resource;
	}

	private static UptimeResource CreateInstance(JObject device, Type objectType)
	{
		if (objectType == typeof(PingCheckResource))
		{
			return new PingCheckResource();
		}

		if (objectType == typeof(WebCheckResource))
		{
			return new WebCheckResource();
		}

		// Abstract / base requested - discriminate on deviceType.
		var deviceType = GetInt(device, "deviceType", 0);
		return deviceType switch
		{
			(int)ResourceType.Ping => new PingCheckResource(),
			(int)ResourceType.Web => new WebCheckResource(),
			_ => throw new NotSupportedException($"Unable to map device of type {deviceType} to an {nameof(UptimeResource)}."),
		};
	}

	private static void PopulateCommon(UptimeResource resource, JObject device)
	{
		var customProperties = ReadCustomProperties(device);

		resource.Id = GetInt(device, "id", 0);
		resource.Name = GetString(device, "name");
		resource.DisplayName = GetString(device, "displayName");
		resource.Description = GetString(device, "description");
		resource.ResourceGroupIds = GetString(device, "hostGroupIds");
		resource.PreferredCollectorId = GetInt(device, "preferredCollectorId", 0);
		resource.DisableAlerting = GetBool(device, "disableAlerting", false);
		resource.IsInternal = GetBool(device, "isInternal", true);
		resource.SyntheticsCollectorIds = ReadIntList(device["syntheticsCollectorIds"]);
		resource.TestLocation = ReadTestLocation(device["testLocation"]);

		// Host name / polling interval can appear either as a custom property or a top-level field.
		resource.HostName = customProperties.TryGetValue(UptimeWireKeys.Hostname, out var host) && !string.IsNullOrEmpty(host)
			? host
			: GetString(device, "host", GetString(device, "domain"));

		resource.PollingIntervalMinutes = customProperties.TryGetValue(UptimeWireKeys.PollingInterval, out var pollingInterval) && int.TryParse(pollingInterval, out var pollingIntervalValue)
			? pollingIntervalValue
			: GetInt(device, "pollingInterval", resource.PollingIntervalMinutes);

		resource.UseDefaultAlertSetting = ReadBoolProperty(customProperties, UptimeWireKeys.UseDefaultAlertSetting);
		resource.UseDefaultLocationSetting = ReadBoolProperty(customProperties, UptimeWireKeys.UseDefaultLocationSetting);

		resource.Alerting = new UptimeAlertSettings
		{
			OverallAlertLevel = LevelFromWire(GetString(device, "overallAlertLevel"), Level.Warning),
			IndividualAlertLevel = LevelFromWire(GetString(device, "individualAlertLevel"), Level.Warning),
			IndividualCheckpointAlertsEnabled = GetBool(device, "individualSmAlertEnable", false),
			FailedCheckCountBeforeAlerting = GetInt(device, "transition", 1),
			AlertCondition = (SiteMonitorAlertCondition)GetInt(device, "globalSmAlertCond", 0),
		};
	}

	private static void PopulatePing(PingCheckResource ping, JObject device)
	{
		// Prefer the nested serviceParameters blob (the create wire shape); fall back to top-level fields.
		var customProperties = ReadCustomProperties(device);
		if (customProperties.TryGetValue(UptimeWireKeys.ServiceParameters, out var serviceParametersJson)
			&& !string.IsNullOrWhiteSpace(serviceParametersJson)
			&& TryParse<UptimePingServiceParameters>(serviceParametersJson, out var serviceParameters))
		{
			ping.PacketCount = ParseInt(serviceParameters!.Count, ping.PacketCount);
			ping.TimeoutMs = ParseInt(serviceParameters.TimeoutInMSPktsNotReceive, ping.TimeoutMs);
			ping.PercentPacketsNotReceivedInTime = ParseInt(serviceParameters.PercentPktsNotReceiveInTime, ping.PercentPacketsNotReceivedInTime);

			ping.Alerting.OverallAlertLevel = LevelFromWire(serviceParameters.OverallAlertLevel, ping.Alerting.OverallAlertLevel);
			ping.Alerting.IndividualAlertLevel = LevelFromWire(serviceParameters.IndividualAlertLevel, ping.Alerting.IndividualAlertLevel);
			ping.Alerting.IndividualCheckpointAlertsEnabled = ParseBool(serviceParameters.IndividualSmAlertEnable, ping.Alerting.IndividualCheckpointAlertsEnabled);
			ping.Alerting.FailedCheckCountBeforeAlerting = ParseInt(serviceParameters.Transition, ping.Alerting.FailedCheckCountBeforeAlerting);
			ping.Alerting.AlertCondition = (SiteMonitorAlertCondition)ParseInt(serviceParameters.GlobalSmAlertCond, (int)ping.Alerting.AlertCondition);

			if (!string.IsNullOrEmpty(serviceParameters.Dns))
			{
				ping.HostName = serviceParameters.Dns;
			}

			return;
		}

		ping.PacketCount = GetInt(device, "count", ping.PacketCount);
		ping.TimeoutMs = GetInt(device, "timeoutInMSPktsNotReceive", ping.TimeoutMs);
		ping.PercentPacketsNotReceivedInTime = GetInt(device, "percentPktsNotReceiveInTime", ping.PercentPacketsNotReceivedInTime);
	}

	private static void PopulateWeb(WebCheckResource web, JObject device)
	{
		web.Domain = GetString(device, "domain", web.HostName);
		web.Scheme = SchemeFromWire(GetString(device, "schema"), web.Scheme);
		web.IgnoreSsl = GetBool(device, "ignoreSSL", false);
		web.PageLoadAlertTimeMs = GetInt(device, "pageLoadAlertTimeInMS", web.PageLoadAlertTimeMs);
		web.TriggerSslStatusAlerts = GetBool(device, "triggerSSLStatusAlert", false);
		web.TriggerSslExpirationAlerts = GetBool(device, "triggerSSLExpirationAlert", false);
		web.AlertExpression = GetString(device, "alertExpr");

		if (device["steps"] is JArray steps)
		{
			web.Steps = steps.ToObject<List<UptimeWebCheckStep>>() ?? [];
		}
	}

	#endregion

	#region Helpers

	private static Dictionary<string, string> ReadCustomProperties(JObject device)
	{
		var result = new Dictionary<string, string>(StringComparer.Ordinal);
		if (device["customProperties"] is not JArray array)
		{
			return result;
		}

		foreach (var property in array)
		{
			var name = property["name"]?.ToString();
			if (!string.IsNullOrEmpty(name))
			{
				result[name!] = property["value"]?.ToString() ?? string.Empty;
			}
		}

		return result;
	}

	private static UptimeTestLocation ReadTestLocation(JToken? token)
	{
		if (token is not JObject obj)
		{
			return new UptimeTestLocation();
		}

		return new UptimeTestLocation
		{
			All = obj["all"]?.Value<bool>() ?? false,
			CollectorIds = ReadIntList(obj["collectorIds"]),
			SmgIds = ReadIntList(obj["smgIds"]),
		};
	}

	private static List<int> ReadIntList(JToken? token)
		=> token is JArray array ? array.Select(t => t.Value<int>()).ToList() : [];

	private static bool ReadBoolProperty(Dictionary<string, string> properties, string key)
		=> properties.TryGetValue(key, out var value) && ParseBool(value, false);

	private static string GetString(JObject device, string key, string fallback = "")
		=> device[key]?.Type is JTokenType.String or JTokenType.Integer or JTokenType.Float
			? device[key]!.ToString()
			: fallback;

	private static int GetInt(JObject device, string key, int fallback)
		=> device[key] is { } token && int.TryParse(token.ToString(), NumberStyles.Integer, Invariant, out var value)
			? value
			: fallback;

	private static bool GetBool(JObject device, string key, bool fallback)
		=> device[key] is { } token && bool.TryParse(token.ToString(), out var value) ? value : fallback;

	private static int ParseInt(string value, int fallback)
		=> int.TryParse(value, NumberStyles.Integer, Invariant, out var result) ? result : fallback;

	private static bool ParseBool(string value, bool fallback)
		=> bool.TryParse(value, out var result) ? result : fallback;

	private static bool TryParse<T>(string json, out T? value)
	{
		try
		{
			value = JsonConvert.DeserializeObject<T>(json);
			return value is not null;
		}
		catch (JsonException)
		{
			value = default;
			return false;
		}
	}

	private static string BoolToWire(bool value) => value ? "true" : "false";

	private static string LevelToWire(Level level) => level switch
	{
		Level.Warning => "warn",
		Level.Error => "error",
		Level.Critical => "critical",
		_ => "warn",
	};

	private static Level LevelFromWire(string value, Level fallback) => value switch
	{
		"warn" => Level.Warning,
		"error" => Level.Error,
		"critical" => Level.Critical,
		_ => fallback,
	};

	private static string SchemeToWire(UptimeHttpScheme scheme) => scheme == UptimeHttpScheme.Http ? "http" : "https";

	private static UptimeHttpScheme SchemeFromWire(string value, UptimeHttpScheme fallback) => value switch
	{
		"http" => UptimeHttpScheme.Http,
		"https" => UptimeHttpScheme.Https,
		_ => fallback,
	};

	#endregion
}

