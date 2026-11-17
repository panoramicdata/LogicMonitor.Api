namespace LogicMonitor.Api.Resources.Uptime.Serialization;

/// <summary>
/// Translates between the strongly-typed Uptime surface and the <c>device/devices</c> wire format
/// (top-level device fields, custom properties and, for ping checks, the nested
/// <c>website.private.serviceParameters</c> JSON blob). Both the resource converter and the creation-DTO
/// converter delegate here so there is a single source of truth for the mapping.
/// </summary>
internal static partial class UptimeResourceWireMapper
{
	private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

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
			["testLocation"] = BuildTestLocation(definition),
			["properties"] = new JArray(),
		};

		if (!string.IsNullOrWhiteSpace(definition.ResourceGroupIds))
		{
			device["groupIds"] = new JArray(ParseGroupIds(definition.ResourceGroupIds));
		}

		if (definition is IPingCheckDefinition ping)
		{
			WritePing(device, ping);
		}
		else if (definition is IWebCheckDefinition web)
		{
			WriteWeb(device, web);
		}

		return device;
	}

	/// <summary>
	/// Internal checks run from real Collectors (collectorIds); external checks from Site Monitor
	/// Groups (smgIds). Exactly one of the two lists is populated.
	/// </summary>
	private static JObject BuildTestLocation(IUptimeCheckDefinition definition)
		=> new()
		{
			["collectorIds"] = new JArray(definition.IsInternal ? definition.SyntheticsCollectorIds : []),
			["smgIds"] = new JArray(definition.IsInternal ? [] : definition.TestLocation.SmgIds),
		};

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
}
