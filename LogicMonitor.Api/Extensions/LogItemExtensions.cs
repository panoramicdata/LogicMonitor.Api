namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Log item extensions
/// </summary>
public static class LogItemExtensions
{

	internal static void ValidateRegexs()
	{
		// Check that no two Regexs have the same id		
		var regexIds = new HashSet<int>();
		foreach (var regex in Regexs)
		{
			if (!regexIds.Add(regex.Id))
			{
				throw new InvalidDataException($"Duplicate regex id {regex.Id} found");
			}
		}
	}

	private static readonly Regex _k8sHostRegex = new(@"^(?<resourceName>.+?)\(id=(?<resourceId>.+?)\)$", RegexOptions.Singleline);

	private static readonly List<LogItemRegex> Regexs = new()
	{
		new(01,
			AuditEventEntityType.Resource,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Device""; ""Device=(?<resourceName>.+?) \((?<resourceId>.+?)\)""; ""Description=(?<failed>Failed)(?<additionalInfo>.+?)""$", RegexOptions.Singleline)),
		new(02,
			AuditEventEntityType.Resource,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Device""; ""Device=(?<resourceName>.+?) \((?<resourceId>.+?)\)""; ""Description=(?<additionalInfo>.+?)""$", RegexOptions.Singleline)),
		new(03,
			AuditEventEntityType.Resource,
			new(@"^(?<action>Add|Fetch|Update) host<(?<resourceId>\d+), (?<resourceName>.+?)> \(monitored by collector <(?<collectorId>[-\d]+), (?<collectorName>.+?)>\), (?<additionalInfo>.*?), ( via API token (?<apiTokenId>.+))?$", RegexOptions.Singleline)),
		new(14,
			AuditEventEntityType.ResourceGroup,
			new(@"^(?<action>Add)ed device group (?<resourceGroupName>.+?) \((?<resourceId>\d+)\)  via API token (?<apiTokenId>.+?)..$", RegexOptions.Singleline)),
		new(04,
			AuditEventEntityType.Resource,
			new(@"^(?<action>Add)ed device (?<resourceName>.+?) \((?<resourceId>\d+)\)  via API token (?<apiTokenId>[^{]+?)(?<additionalInfo>.*?)$", RegexOptions.Singleline)),
		new(05,
			AuditEventEntityType.ResourceGroupProperty,
			new(@"^(?<action>Add|Fetch|Update) the host group\((?<resourceGroupName>.+?)\)'s property\(name=(?<propertyName>.+?)\) via API token (?<apiTokenId>.+?)..$", RegexOptions.Singleline)),
		new(06,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>(.+?deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),instanceChanges=\[instanceId=(?<instanceId>\d+?),oldValue=(?<instanceOldValue>.+?),newValue=(?<instanceNewValue>.+?)\];\];.+?))""$", RegexOptions.Singleline)),
		new(07,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Instance\(s\) disappeared from: (?<resourceName>.+?) \(CollectorID=(?<collectorId>[-\d]+?)\) \[(?<dataSourceName>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),dataSourceDeletedInstanceId\(s\)=(?<dataSourceDeletedInstanceIds>[\d,]+?)\];)""$", RegexOptions.Singleline)),
		new(08,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Found new instance\(s\) for: (?<resourceName>.+?) \(CollectorID=(?<collectorId>[-\d]+?)\) \[(?<dataSourceNewInstanceNames>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),dataSourceNewInstanceId\(s\)=(?<dataSourceNewInstanceIds>[\d,]+?)];)""$", RegexOptions.Singleline)),
		new(09,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Found new instance\(s\) for: (?<resourceName>.+?) \(CollectorID=(?<collectorId>[-\d]+?)\) \[(?<dataSourceNewInstanceNames>.+?)\]; Instance\(s\) disappeared from: (?<resourceName2>.+?) \(CollectorID=(?<collectorId2>[-\d]+?)\) \[(?<dataSourceDeletedInstanceNames>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),dataSourceNewInstanceId\(s\)=(?<dataSourceNewInstanceIds>[\d,]+?),dataSourceDeletedInstanceId\(s\)=(?<dataSourceDeletedInstanceIds>[\d,]+?)\];)""$", RegexOptions.Singleline)),
		new(10,
			AuditEventEntityType.AllCollectors,
			new(@"^(?<scheduledHealthCheck>Scheduled health check scripts for all collectors)$", RegexOptions.Singleline)),
		new(11,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceId=(?<instanceId>.+?)""; ""Description=(?<description>.+?)""$", RegexOptions.Singleline)),
		new(12,
			AuditEventEntityType.ScheduledDownTime,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=SDT""; ""Device=(?<resourceName>.+?)""; ""InstanceId=(?<instanceId>.+?)""; ""Description=(?<description>.+?)""$", RegexOptions.Singleline)),
		new(13,
			AuditEventEntityType.ResourceGroup,
			new(@"^(?<action>Update) the device group (?<resourceGroupName>.+?).Nothing has been changed. via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(15,
			AuditEventEntityType.DataSource,
			new(@"""Action=(?<action>Add)""; ""Type=DataSource""; ""DataSourceName=(?<dataSourceName>.+?)""; ""DeviceName=(?<resourceDisplayName>.+?) \((?<resourceName>.+?)\)""; ""DeviceId=(?<resourceId>\d+?)""; ""Description=(?<dataSourceDescription>.+?)""; ""DataSourceId=(?<dataSourceId>\d+?)""; ""DeviceDataSourceId=(?<deviceDataSourceId>\d+?)""$", RegexOptions.Singleline)),
		new(16,
			AuditEventEntityType.ResourceProperty,
			new(@"^(?<action>Add) property\(name=(?<propertyName>.+), value=(?<propertyValue>.+)\) to host\((?<resourceName>.+)\) via API token (?<apiTokenId>.+).$", RegexOptions.Singleline)),
		new(17,
			AuditEventEntityType.ResourceGroups,
			new(@"^host\(id= (?<resourceId>.+?) ,name= (?<resourceName>.+?).(?<action>add) to groups, list: (?<groupList>.+?),add group number is (?<groupCount>.+?)$", RegexOptions.Singleline)),
		new(18,
			AuditEventEntityType.Resource,
			new(@"^(?<action>Delete) the Kubernetes hosts those were marked for deletion \[(?<multipleHosts>.+?)\]$", RegexOptions.Singleline)),
		new(19,
			AuditEventEntityType.ResourceGroups,
			new(@"^ host\(id=(?<resourceId>.+?),name =(?<resourceName>.+?)\) (?<action>add) to (?<groupName>.+?) ,appliesTo=(?<appliesTo>.+?) ,delete number is (?<deleteCount>.+?) ,add number is (?<addCount>.+?)$", RegexOptions.Singleline)),
		new(20,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add)""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceName=(?<instanceName>.+?) \((?<wildValue>.+?)\)""; ""Description=DataSourceName: (?<dataSourceName>.+?) ""$", RegexOptions.Singleline)),
		new(21,
			AuditEventEntityType.None,
			new(@"^(?<loginName>.+?) signs in via SAML$", RegexOptions.Singleline)),
		new(22,
			AuditEventEntityType.None,
			new(@"^Failed API request: API token (?<apiTokenId>.+?) attempted to access path '(?<apiPath>.+?)' with Method: (?<apiMethod>.+?)$", RegexOptions.Singleline)),
		new(23,
			AuditEventEntityType.Resource,
			new(@"^(?<action>Delete) the aws hosts \[(?<multipleHosts>.+?)\]$", RegexOptions.Singleline)),
		new(24,
			AuditEventEntityType.None,
			new(@"^(?<loginName>.+?) signs in \(adminId=(?<adminId>.+?)\)\.$", RegexOptions.Singleline)),
		new(25,
			AuditEventEntityType.Account,
			new(@"^(?<action>Add) a new account (?<accountName>.+?) \(administrator\)$", RegexOptions.Singleline)),
		new(26,
			AuditEventEntityType.Account,
			new(@"^(?<accountName>.+?) (?<action>update) password change password$", RegexOptions.Singleline)),
		new(27,
			AuditEventEntityType.DataSource,
			new(@"^Import DataSource from repository.  Change details : Change datasource : (?<dataSourceName>.+?), dsId=(?<dataSourceId>.+?){(?<datasourceContent>.+)}$", RegexOptions.Singleline)),
		new(28,
			AuditEventEntityType.DataSourceGraph,
			new(@"""Action=(?<action>Add)""; ""Type=DataSourceGraph""; ""DataSourceName=(?<dataSourceName>.+?)""; ""Device=NA""; ""Description=Add datasource graph, graph=(?<graphName>.+?)\((?<graphId>.+?)\), ""$", RegexOptions.Singleline)),
		new(29,
			AuditEventEntityType.None,
			new(@"^(?<discardedEventAlert>An event alert was discarded for EventSource Azure Advisor Recommendations because it exceeded the rate limit of 150 events per 60 seconds. Adding filters to your EventSource may help reduce the number of alerts triggered\.)$", RegexOptions.Singleline)),
		new(30,
			AuditEventEntityType.ScheduledDownTime,
			new(@"^(?<action>.+?) SDT from (?<sdtStart>.+?) to (?<sdtEnd>.+?) from .+ on Host (?<resourceName>.+) via API token (?<apiTokenId>.+)$", RegexOptions.Singleline)),
		new(31,
			AuditEventEntityType.ScheduledDownTime,
			new(@"^(?<action>.+?) SDT for .+ on Host (?<resourceName>.+) with scheduled downtime from (?<sdtStart>.+?) to (?<sdtEnd>.+?) via API token (?<apiTokenId>.+)$", RegexOptions.Singleline)),
	};

	/// <summary>
	/// Converts a logItem to an AuditItem
	/// </summary>
	/// <param name="logItem"></param>
	public static AuditEvent ToAuditEvent(this LogItem logItem)
	{
		if (logItem is null)
		{
			throw new ArgumentNullException(nameof(logItem));
		}

		var auditEvent = new AuditEvent
		{
			DateTime = logItem.HappenedOnUtc,
			UserName = logItem.UserName,
			Host = logItem.IpAddress,
			OriginalDescription = logItem.Description,
			SessionId = logItem.SessionId,
			OriginatorType =
				logItem.UserName.StartsWith("System:", StringComparison.Ordinal) ? AuditEventOriginatorType.System :
				logItem.UserName == "k8smonitoring" ? AuditEventOriginatorType.CollectorKubernetes :
				logItem.UserName == "lmsupport" ? AuditEventOriginatorType.CollectorOther :
				AuditEventOriginatorType.User,
		};

		// Interpret the description field
		var entityTypeMatch = GetMatchFromDescription(logItem.Description);
		if (entityTypeMatch.LogItemRegex is null || entityTypeMatch.Match is null)
		{
			// Not recognised
			return auditEvent;
		}

		auditEvent.MatchedRegExId = entityTypeMatch.LogItemRegex.Id;
		// Have we determined the EntityType already?
		auditEvent.EntityType = entityTypeMatch.LogItemRegex.EntityType;
		var match = entityTypeMatch.Match;
		//if (auditEvent.EntityType == AuditEventEntityType.None)
		//{
		//	// Is this a DeviceDataSourceInstance entry?
		//	if (entityTypeMatch.Groups["dataSourceNewInstanceIds"].Success
		//		|| entityTypeMatch.Groups["dataSourceDeletedInstanceIds"].Success)
		//	{
		//		// YES
		//		auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
		//	}
		//	else
		//	{
		//		// Is this a scheduledHealthCheck
		//		if (entityTypeMatch.Groups["scheduledHealthCheck"].Success)
		//		{
		//			auditEvent.ActionType = AuditEventActionType.ScheduledHealthCheckScript;
		//			auditEvent.EntityType = AuditEventEntityType.AllCollectors;
		//			auditEvent.OutcomeType = AuditEventOutcomeType.Unknown;
		//		}
		//	}

		//	if (auditEvent.EntityType == AuditEventEntityType.None) { }
		//	// NO - Don't know the type, can't continue
		//	return auditEvent;
		//}

		auditEvent.ActionType = GetAction(match);
		auditEvent.OutcomeType = match.Groups["failed"].Success ? AuditEventOutcomeType.Failure : AuditEventOutcomeType.Success;

		var resourceIdString = GetGroupValueAsStringOrNull(match, "resourceId");
		auditEvent.ResourceGroupName = GetGroupValueAsStringOrNull(match, "resourceGroupName");
		auditEvent.CollectorId = GetGroupValueAsIntOrNull(match, "collectorId");
		auditEvent.CollectorName = GetGroupValueAsStringOrNull(match, "collectorName");
		auditEvent.ApiTokenId = GetGroupValueAsStringOrNull(match, "apiTokenId");
		auditEvent.ApiPath = GetGroupValueAsStringOrNull(match, "apiPath");
		auditEvent.ApiMethod = GetGroupValueAsStringOrNull(match, "apiMethod");

		// Add metadata that can't be extracted from the description using the regex
		switch (auditEvent.MatchedRegExId)
		{
			case 22:
				auditEvent.ActionType = AuditEventActionType.GeneralApi;
				auditEvent.OutcomeType = AuditEventOutcomeType.Failure;
				break;
			case 27:
				auditEvent.ActionType = AuditEventActionType.Update;
				break;
		}

		auditEvent.DataSourceId = GetGroupValueAsIntOrNull(match, "dataSourceId");
		auditEvent.DataSourceName = GetGroupValueAsStringOrNull(match, "dataSourceName");
		auditEvent.InstanceId = GetGroupValueAsIntOrNull(match, "instanceId");
		auditEvent.InstanceName = GetGroupValueAsStringOrNull(match, "instanceName");
		auditEvent.WildValue = GetGroupValueAsStringOrNull(match, "wildValue");

		auditEvent.LoginName = GetGroupValueAsStringOrNull(match, "loginName");
		auditEvent.AccountName = GetGroupValueAsStringOrNull(match, "accountName");

		auditEvent.PropertyName = GetGroupValueAsStringOrNull(match, "propertyName");
		auditEvent.PropertyValue = GetGroupValueAsStringOrNull(match, "propertyValue");

		auditEvent.DataSourceNewInstanceIds = match.Groups["dataSourceNewInstanceIds"].Success
			? match.Groups["dataSourceNewInstanceIds"].Value
				.Split(',')
				.Select(subString => int.Parse(subString, CultureInfo.InvariantCulture))
				.ToList()
			: null;
		auditEvent.DataSourceNewInstanceNames = match.Groups["dataSourceNewInstanceNames"].Success
			? match.Groups["dataSourceNewInstanceNames"].Value
				.Split(',')
				.ToList()
			: null;
		auditEvent.DataSourceDeletedInstanceIds = match.Groups["dataSourceDeletedInstanceIds"].Success
			? match.Groups["dataSourceDeletedInstanceIds"].Value
				.Split(',')
				.Select(subString => int.Parse(subString, CultureInfo.InvariantCulture))
				.ToList()
			: null;
		auditEvent.DataSourceDeletedInstanceNames = match.Groups["dataSourceDeletedInstanceNames"].Success
			? match.Groups["dataSourceDeletedInstanceNames"].Value
				.Split(',')
				.ToList()
			: null;

		if (match.Groups["multipleHosts"].Success)
		{
			var k8sHosts = match.Groups["multipleHosts"].ToString().Split(',').Select(h => h.Trim());
			if (k8sHosts.Any())
			{
				auditEvent.ResourceIds ??= new();
				auditEvent.ResourceNames ??= new();
				foreach (var k8sHost in k8sHosts)
				{
					var k8sHostMatch = _k8sHostRegex.Match(k8sHost);
					if (k8sHostMatch.Success)
					{
						auditEvent.ResourceIds.Add(int.Parse(k8sHostMatch.Groups["resourceId"].Value, CultureInfo.InvariantCulture));
						auditEvent.ResourceNames.Add(k8sHostMatch.Groups["resourceName"].Value.ToString());
					}
				}
			}
		}
		else
		{
			int? resourceId = (resourceIdString == "NA" || string.IsNullOrEmpty(resourceIdString)) ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			var resourceName = GetGroupValueAsStringOrNull(match, "resourceName");
			auditEvent.ResourceIds = resourceId is null ? null : new() { resourceId.Value };
			auditEvent.ResourceNames = resourceName is null ? null : new() { resourceName };
		}

		return auditEvent;

		//if ((match = DeviceOtherRegex.Match(logItem.Description)).Success)
		//{
		//	// Example: "Action=Add"; "Type=Device"; "Device=ReportMagic beta-API (0)"; "Description=Failed device operation,  adddevice_failed : error  (invalid normal device name: ReportMagic beta-API)"
		//	// Example: "Action=Add"; "Type=Device"; "Device=ReportMagic alpha-Worker (0)"; "Description=Failed device operation,  adddevice_failed : error  (invalid normal device name: ReportMagic alpha-Worker)"
		//	auditEvent.ActionType = GetAction(match);
		//	auditEvent.EntityType = AuditEventEntityType.Resource;
		//	auditEvent.OutcomeType = AuditEventOutcomeType.Failure;
		//	auditEvent.ResourceId = int.Parse(match.Groups["resourceId"].Value, CultureInfo.InvariantCulture);
		//	auditEvent.ResourceName = match.Groups["resourceName"].Value;
		//	return auditEvent;
		//}

		//if ((match = ResourceGroupPropertySuccessRegex.Match(logItem.Description)).Success)
		//{
		//	// Example: "Update the host group(PDL - Panoramic Data/Datacenter/Private/Servers/Kubernetes Cluster: PDL-K8S-PROD)'s property(name=kubernetes.version.history) via API token MZkW3Ldwg5S84s5eWUc7."
		//	auditEvent.ActionType = GetAction(match);
		//	auditEvent.EntityType = AuditEventEntityType.ResourceGroupProperty;
		//	auditEvent.OutcomeType = AuditEventOutcomeType.Success;

		//	auditEvent.ResourceGroupName = match.Groups["resourceGroupName"].Value;
		//	auditEvent.PropertyName = match.Groups["resourceGroupNamepropertyName"].Value;
		//	auditEvent.ApiTokenId = match.Groups["apiTokenId"].Value;
		//	return auditEvent;
		//}
	}

	private static string? GetGroupValueAsStringOrNull(Match match, string groupName)
		=> match.Groups[groupName].Success ? match.Groups[groupName].Value : null;

	private static int? GetGroupValueAsIntOrNull(Match match, string groupName)
		=> match.Groups[groupName].Success
			? int.TryParse(match.Groups[groupName].Value, out var intValue) ? intValue : null
			: null;

	private static (LogItemRegex? LogItemRegex, Match? Match) GetMatchFromDescription(string description)
		=> Regexs
			.Select(entry => (LogItemRegex: entry, Match: entry.Regex.Match(description)))
			.FirstOrDefault(entry => entry.Match.Success);

	private static AuditEventActionType GetAction(Match value)
	{
		if (value.Groups["scheduledHealthCheck"].Success)
		{
			return AuditEventActionType.ScheduledHealthCheckScript;
		}

		if (value.Groups["loginName"].Success)
		{
			return AuditEventActionType.Login;
		}

		if (value.Groups["discardedEventAlert"].Success)
		{
			return AuditEventActionType.DiscardedEventAlert;
		}

		return value.Groups["action"].Value.ToUpperInvariant() switch
		{
			"ADD" => AuditEventActionType.Create,
			"FETCH" => AuditEventActionType.Read,
			"UPDATE" => AuditEventActionType.Update,
			"DELETE" => AuditEventActionType.Delete,
			_ => AuditEventActionType.None
		};
	}

	private class LogItemRegex
	{
		public LogItemRegex(int id, AuditEventEntityType entityType, Regex regex)
		{
			Id = id;
			EntityType = entityType;
			Regex = regex;
		}

		public int Id { get; set; } = default!;
		public AuditEventEntityType EntityType { get; set; } = default!;
		public Regex Regex { get; set; } = default!;
	}

}
