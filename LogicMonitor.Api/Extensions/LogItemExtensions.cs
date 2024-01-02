namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Log item extensions
/// </summary>
public static class LogItemExtensions
{

	internal static void ValidateRegexes()
	{
		// Check that no two Regex have the same id
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

	private static readonly List<LogItemRegex> Regexs =
	[
		new(01,
			AuditEventEntityType.Device,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Device""; ""Device=(?<resourceName>.+) \((?<resourceId>.+?)\)""; ""Description=(?<failed>Failed)(?<additionalInfo>.+?)""$", RegexOptions.Singleline)),
		new(02,
			AuditEventEntityType.Device,
			new(@"^""Action=(?<action>Add|Fetch|Update|Delete)""; ""Type=Device""; ""Device=(?<resourceName>.+) \((?<resourceId>.+?)\)""; ""Description=(?<additionalInfo>.*?)""$", RegexOptions.Singleline)),
		new(03,
			AuditEventEntityType.Device,
			new(@"^(?<action>Add|Fetch|Update) host<(?<resourceId>\d+), (?<resourceName>.+?)> \(monitored by collector <(?<collectorId>[-\d]+), (?<collectorName>.+?)>\), (?<additionalInfo>.*?), ( via API token (?<apiTokenId>.+))?$", RegexOptions.Singleline)),
		new(14,
			AuditEventEntityType.ResourceGroup,
			new(@"^(?<action>Add)ed device group (?<resourceGroupName>.+) \((?<resourceGroupId>\d+)\)  via API token (?<apiTokenId>.+?)..$", RegexOptions.Singleline)),
		new(04,
			AuditEventEntityType.Device,
			new(@"^(?<action>Add)ed device (?<resourceName>.+) \((?<resourceId>\d+)\)  via API token (?<apiTokenId>[^{]+?)(?<additionalInfo>.*?)$", RegexOptions.Singleline)),
		new(05,
			AuditEventEntityType.ResourceGroupProperty,
			new(@"^(?<action>Add|Fetch|Update) the host group\((?<resourceGroupName>.+?)\)'s property\(name=(?<propertyName>.+?)\) via API token (?<apiTokenId>.+?)..$", RegexOptions.Singleline)),
		new(06,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>(.+?deviceId=(?<resourceId>\d+?),dataSourceId=(?<logicModuleId>\d+?),instanceChanges=\[instanceId=(?<instanceId>\d+?),oldValue=(?<instanceOldValue>.+?),newValue=(?<instanceNewValue>.+?)\];\];.+?))""$", RegexOptions.Singleline)),
		new(07,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Instance\(s\) disappeared from: (?<resourceName>.+?) \(CollectorID=(?<collectorId>[-\d]+?)\) \[(?<logicModuleName>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<logicModuleId>\d+?),dataSourceDeletedInstanceId\(s\)=(?<dataSourceDeletedInstanceIds>[\d,]+?)\];)""$", RegexOptions.Singleline)),
		new(08,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Found new instance\(s\) for: (?<resourceName>.+?) \(CollectorID=(?<collectorId>[-\d]+?)\) \[(?<dataSourceNewInstanceNames>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<logicModuleId>\d+?),dataSourceNewInstanceId\(s\)=(?<dataSourceNewInstanceIds>[\d,]+?)];)""$", RegexOptions.Singleline)),
		new(09,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Found new instance\(s\) for: (?<resourceName>.+?) \(CollectorID=(?<collectorId>[-\d]+?)\) \[(?<dataSourceNewInstanceNames>.+?)\]; Instance\(s\) disappeared from: (?<resourceName2>.+?) \(CollectorID=(?<collectorId2>[-\d]+?)\) \[(?<dataSourceDeletedInstanceNames>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<logicModuleId>\d+?),dataSourceNewInstanceId\(s\)=(?<dataSourceNewInstanceIds>[\d,]+?),dataSourceDeletedInstanceId\(s\)=(?<dataSourceDeletedInstanceIds>[\d,]+?)\];)""$", RegexOptions.Singleline)),
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
			new(@"^""Action=(?<action>Add)""; ""Type=DataSource""; ""DataSourceName=(?<logicModuleName>.+?)""; ""DeviceName=(?<resourceDisplayName>.+?) \((?<resourceName>.+?)\)""; ""DeviceId=(?<resourceId>\d+?)""; ""Description=(?<dataSourceDescription>.+?)""; ""DataSourceId=(?<logicModuleId>\d+?)""; ""DeviceDataSourceId=(?<deviceDataSourceId>\d+?)""$", RegexOptions.Singleline)),
		new(16,
			AuditEventEntityType.ResourceProperty,
			new(@"^(?<action>Add) property\(name=(?<propertyName>.+), value=(?<propertyValue>.+)\) to host\((?<resourceName>.+)\) via API token (?<apiTokenId>.+).$", RegexOptions.Singleline)),
		new(17,
			AuditEventEntityType.ResourceGroups,
			new(@"^host\(id= (?<resourceId>.+?) ,name= (?<resourceName>.+?).(?<action>add) to groups, list: (?<groupList>.+?),add group number is (?<groupCount>.+?)$", RegexOptions.Singleline)),
		new(18,
			AuditEventEntityType.Device,
			new(@"^(?<action>Delete) the Kubernetes hosts those were marked for deletion \[(?<multipleHosts>.+?)\]$", RegexOptions.Singleline)),
		new(19,
			AuditEventEntityType.ResourceGroups,
			new(@"^ host\(id=(?<resourceId>.+?),name =(?<resourceName>.+?)\) (?<action>add) to (?<groupName>.+?) ,appliesTo=(?<appliesTo>.+?) ,delete number is (?<deleteCount>.+?) ,add number is (?<addCount>.+?)$", RegexOptions.Singleline)),
		new(20,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add)""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceName=(?<instanceName>.+?) \((?<wildValue>.+?)\)""; ""Description=DataSourceName: (?<logicModuleName>.+?) ""$", RegexOptions.Singleline)),
		new(21,
			AuditEventEntityType.None,
			new(@"^(?<userName>.+?) (?<login>signs in) via SAML$", RegexOptions.Singleline)),
		new(22,
			AuditEventEntityType.None,
			new(@"^Failed API request: API token (?<apiTokenId>.+?) attempted to access path '(?<apiPath>.+?)' with Method: (?<apiMethod>.+?)$", RegexOptions.Singleline)),
		new(23,
			AuditEventEntityType.Device,
			new(@"^(?<action>Delete) the aws hosts \[(?<multipleHosts>.+?)\]$", RegexOptions.Singleline)),
		new(24,
			AuditEventEntityType.Account,
			new(@"^(?<userName>.+) (?<login>signs in) \(adminId=(?<userId>\d+)\)\.$", RegexOptions.Singleline)),
		new(25,
			AuditEventEntityType.Account,
			new(@"^(?<action>Add) a new account (?<userName>.+?) \(administrator\)$", RegexOptions.Singleline)),
		new(26,
			AuditEventEntityType.Account,
			new(@"^(?<userName>.+?) (?<action>update) password change password$", RegexOptions.Singleline)),
		new(27,
			AuditEventEntityType.DataSource,
			new(@"^Import DataSource from repository.  Change details : Change datasource : (?<logicModuleName>.+?), dsId=(?<logicModuleId>.+?){(?<datasourceContent>.+)}$", RegexOptions.Singleline)),
		new(28,
			AuditEventEntityType.DataSourceGraph,
			new(@"""Action=(?<action>Add|Update)""; ""Type=DataSourceGraph""; ""(LogicModuleName|DataSourceName)=(?<logicModuleName>.+?)""; ""Device=NA""; ""Description=Add datasource graph, graph=(?<graphName>.+)\((?<graphId>.+?)\), ""$", RegexOptions.Singleline)),
		new(29,
			AuditEventEntityType.None,
			new(@"^(?<discardedEventAlert>An event alert was discarded for EventSource (?<logicModuleName>.+?) because it exceeded the rate limit of \d+ events per \d+ seconds. Adding filters to your EventSource may help reduce the number of alerts triggered\.)$", RegexOptions.Singleline)),
		new(30,
			AuditEventEntityType.ScheduledDownTime,
			new(@"^(?<action>.+?) SDT from (?<sdtStart>.+?) to (?<sdtEnd>.+?) from .+ on Host (?<resourceName>.+) via API token (?<apiTokenId>.+)$", RegexOptions.Singleline)),
		new(31,
			AuditEventEntityType.ScheduledDownTime,
			new(@"^""Action=(?<action>.+?)""; ""Type=SDT""; ""Description=(?<description>.+?)""; ?""DeviceDatasourceInstanceName=(?<instanceName>.+?)""; ?""DeviceDataSourceInstanceId=(?<instanceId>.+?)""; ?""DeviceName=(?<resourceName>.+?)""; ?""DeviceId=(?<resourceId>.+?)""; ?""StartDownTime=(?<startDownTime>.+?)""; ?""EndDownTime=(?<endDownTime>.+?)"";$", RegexOptions.Singleline)),
		new(32,
			AuditEventEntityType.ResourceGroup,
			new(@"^(?<action>.+?)ed device group (?<resourceGroupName>.+?) \((?<resourceGroupId>.+?)\) ,.+$", RegexOptions.Singleline)),
		new(33,
			AuditEventEntityType.DataSource,
			new(@"^""Action=(?<action>Add)""; ""Type=DataSource""; ""(LogicModuleName|DataSourceName)=(?<logicModuleName>.+?)""; ""DeviceName=(?<resourceDisplayName>.+?)""; ""DeviceId=(?<resourceId>\d+?)""; ""Description=(?<dataSourceDescription>.+?)""; ""DataSourceId=(?<logicModuleId>\d+?)""; ""DeviceDataSourceId=(?<deviceDataSourceId>\d+?)""$", RegexOptions.Singleline)),
		new(34,
			AuditEventEntityType.AlertNote,
			new(@"^Note \((?<alertNote>.+?)\) added to \((?<alertId>.+?)\) by \((?<username>.+?)\).$", RegexOptions.Singleline)),
		new(35,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?)ed device (?<resourceName>.+) \((?<resourceId>.+?)\)  via API token (?<apiTokenId>.+)$", RegexOptions.Singleline)),
		new(36,
			AuditEventEntityType.None,
			new(@"^(?<action>regular device total monthly metrics) -> (?<monthlyMetrics>.+?)$")),
		new(37,
			AuditEventEntityType.None,
			new(@"^Throttled API request: API token (?<apiTokenId>.+?) attempted to access path '(?<apiPath>.+?)' with Method: (?<apiMethod>.+?)$", RegexOptions.Singleline)),
		new(38,
			AuditEventEntityType.ScheduledDownTime,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ?""Type=SDT""; ?""Description=(?<description>.+?)""; ?"".+?Name=(?<resourceName>.+?)""; ?"".+?Id=(?<resourceId>.+?)""; ?""StartDownTime=(?<startDownTime>.+?)""; ?""EndDownTime=(?<endDownTime>.+?)"";$", RegexOptions.Singleline)),
		new(39,
			AuditEventEntityType.OpsNote,
			new(@"^(?<action>add) new opsnote \((?<description>.+?)\)$", RegexOptions.Singleline)),
		new(40,
			AuditEventEntityType.AllCollectors,
			new(@"(?<command>.+) (?<action>run) by .+? on collector \(id=(?<collectorId>.+?), hostname=(?<collectorName>.+?), desc=(?<collectorDescription>.+?)\)$", RegexOptions.Singleline)),
		new(41,
			AuditEventEntityType.Device,
			new(@"^Schedule data-collection poll request, hostId=(?<hostId>.+?), agentId=(?<agentId>.+?), requestId=(?<requestId>.+?)$", RegexOptions.Singleline)),
		new(42,
			AuditEventEntityType.ResourceGroup,
			new(@"^ via API token (?<apiTokenId>[^{]+?), (?<action>Delet)ed device group (?<resourceGroupName>.+?) \((?<resourceGroupId>.+?)\), (?<actionTwo>Delet)ed device (?<resourceName>.+?) \(.+?\) \((?<resourceId>.+?)\)$", RegexOptions.Singleline)),
		new(43,
			AuditEventEntityType.ResourceGroup,
			new(@"^ via API token (?<apiTokenId>[^{]+?), (?<action>Delet)ed device group (?<resourceGroupName>.+) \((?<resourceGroupId>.+?)\)$", RegexOptions.Singleline)),
		new(44,
			AuditEventEntityType.Account,
			new(@"^(?<userName>.+?) (?<login>Could not log into the system) - Authentication failed \.$", RegexOptions.Singleline)),
		new(45,
			AuditEventEntityType.Device,
			new(@"^(?<action>Add) a widget test to dashboard (?<resourceName>.+?) via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(46,
			AuditEventEntityType.ResourceGroup,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Group""; ""Device=NA""; ""GroupName=(?<resourceGroupName>.+?)""; ""Description=(?<description>(.|\n)+?)""; ""Alert_threshold_changes=((.|\n)+?)""; ""DataSource=(?<logicModuleName>.+?)""; ""DataSourceId=(?<logicModuleId>\d+?)""; ""Reason=(.+?)""$", RegexOptions.Singleline)),
		new(47,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<description>.+?)""; ""Alert_threshold_changes=\[DataPointId=(.+?),DataPointName=(.+?),OldDataPointValue=(?<instanceOldValue>.+?),NewDataPointValue=(?<instanceNewValue>.+?)\]""; ""InstanceId=(?<instanceId>\d+?)""; ""Reason=(.+?)""$", RegexOptions.Singleline)),
		new(48,
			AuditEventEntityType.ScheduledDownTime,
			new(@"^(?<action>.+?) SDT for .+ on Host (?<resourceName>.+) with scheduled downtime from (?<sdtStart>.+?) to (?<sdtEnd>.+?) via API token (?<apiTokenId>.+)$", RegexOptions.Singleline)),
		new(49,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?) .* folder (?<resourceName>.+?)$", RegexOptions.Singleline)),
		new(50,
			AuditEventEntityType.ResourceGroup,
			new(@"^(?<action>.+?)ed NetScan group '(?<resourceName>.+?)'$", RegexOptions.Singleline)),
		new(51,
			AuditEventEntityType.ResourceGroup,
			new(@"^(?<action>.+?) the website group (?<resourceName>.+?) via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(52,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?)(ed)? .*group (?<resourceName>.+?) .?via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(53,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?) website (?<resourceName>.+?) .?via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(54,
			AuditEventEntityType.Account,
			new(@"^(?<action>.+?) .*account (?<userName>.+?) .?via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(55,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?)ed NetScan '(?<resourceName>.+?)'$", RegexOptions.Singleline)),
		new(56,
			AuditEventEntityType.Account,
			new(@"^(?<action>.+?) .*user role (?<resourceName>.+?) .?via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(57,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?) api tokens - (?<resourceName>.+?) .?via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(58,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?) Collector (?<resourceName>.+?) .?via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(59,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?) (the |a |the widget test of )?dashboard (?<resourceName>.+?) .?via API token (?<apiTokenId>.+?)$", RegexOptions.Singleline)),
		new(60,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?) a new alert rule Name=(?<resourceName>.+?),.+?$", RegexOptions.Singleline)),
		new(61,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+?)(ed)? alert rule (?<resourceName>.+?)$", RegexOptions.Singleline)),
		new(62,
			AuditEventEntityType.Device,
			new(@"""Action=(?<action>.+?)""; ""Type=AppliesToFunction""; ""LogicModuleName=(?<resourceName>.+?)""; ""Device=.+?""; ""LogicModuleId=(?<resourceId>.+?)""; ""Description=(?<description>.+?)"";", RegexOptions.Singleline)),
		new(63,
			AuditEventEntityType.ResourceGroup,
			new(@"^(?<action>Update) the device group (?<resourceGroupName>.+).  \{\n\[\n.+?\n\]\n\}.+$", RegexOptions.Singleline)),
		new(64,
			AuditEventEntityType.Account,
			new(@"^(?<action>.+?) a new account (?<userName>.+) \((?<userRole>.+?)\)$", RegexOptions.Singleline)),
		new(65,
			AuditEventEntityType.UserRole,
			new(@"^(?<action>.+?) user role (?<userRole>.+?)$", RegexOptions.Singleline)),
		new(66,
			AuditEventEntityType.Account,
			new(@"^(?<userName>.+?) (?<login>log in)\.$", RegexOptions.Singleline)),
		new(67,
			AuditEventEntityType.DataSource,
			new(@"^(?<action>.+) (?<logicModuleName>.+) for hostgroup (?<resourceGroupName>.+)$", RegexOptions.Singleline)),
		new(68,
			AuditEventEntityType.Device,
			new(@"^(?<action>.+) (?<remoteSessionType>.+) session to (?<resourceHostname>.+)$", RegexOptions.Singleline)),
		new(69,
			AuditEventEntityType.Account,
			new(@"^Suspended SAML user (?<userName>.+) tried to (?<action>.+)$", RegexOptions.Singleline)),
		new(70,
			AuditEventEntityType.Account,
			new(@"^(?<userName>.+) (?<action>.+)d Two Factor Authentication.$", RegexOptions.Singleline)),
		new(71,
			AuditEventEntityType.Account,
			new(@"^API token (?<apiTokenId>.+) is locked due to too many failed attempts$", RegexOptions.Singleline)),
		new(72,
			AuditEventEntityType.Account,
			new(@"^(?<userName>.+) update password Change password from forgot$", RegexOptions.Singleline)),
		new(73,
			AuditEventEntityType.ConfigSource,
			new(@"^(?<action>.+) (?<logicModuleType>.+)<(?<instanceName>.+)>\. ""ConfigSourceInstanceId=(?<instanceId>\d+)""; ""ConfigSourceName=(?<logicModuleName>.+)""; ""ConfigVersion=(?<logicModuleVersion>\d+)""; ""DeviceName=(?<resourceName>.+)""; ""DeviceId=(?<resourceId>\d+)"";$", RegexOptions.Singleline)),
		new(74,
			AuditEventEntityType.DataSource,
			new(@"^(?<action>.+) (?<logicModuleType>.+)<(?<instanceName>.+)>\. ""DataSourceInstanceId=(?<instanceId>\d+)""; ""(LogicModuleName|DataSourceName)=(?<logicModuleName>.+)""; ""DataSourceVersion=(?<logicModuleVersion>\d+)""; ""DeviceName=(?<resourceName>.+)""; ""DeviceId=(?<resourceId>\d+)"";$", RegexOptions.Singleline)),
		new(75,
			AuditEventEntityType.Account,
			new(@"^User\(name=(?<userName>.+), email=(?<userEmail>.+)\) (?<action>forgot password)$", RegexOptions.Singleline)),
		new(76,
			AuditEventEntityType.Account,
			new(@"^(?<action>.+)  account (?<userName>.+) \((?<description>.*)\)$", RegexOptions.Singleline)),
		new(77,
			AuditEventEntityType.DeviceDataSourceInstance,
			new(@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<description>.+?)""$", RegexOptions.Singleline)),
		new(78,
			AuditEventEntityType.Device,
			new(@"^Remote (?<remoteSessionType>.+) session (?<remoteSessionId>.+) to (?<resourceHostname>.+) (?<action>.+) at (?<time>.+)$", RegexOptions.Singleline)),
		new(79,
			AuditEventEntityType.Account,
			new(@"^Modify account (?<userName>.+), (?<description>.+)$", RegexOptions.Singleline)),
		new(80,
			AuditEventEntityType.Account,
			new(@"^lmsupport (?<login>signs in) on behalf of (?<userName>.+) - restrictSSO=(?<restrictSso>true|false) \(adminId=(?<userId>\d+)\).$", RegexOptions.Singleline)),
		new(81,
			AuditEventEntityType.CollectorGroup,
			new(@"^(?<action>Re-balanced) collector group (?<collectorGroupName>.+)\((?<collectorGroupId>\d+)\),(?<description>.+)$", RegexOptions.Singleline)),
		new(82,
			AuditEventEntityType.ConfigSourceInstance,
			new(@"^Schedule (?<action>collect now) for (?<logicModuleType>ConfigSource) instance<(?<instanceId>\d+)>\. ""ConfigSourceInstanceName=(?<instanceName>.+)""; ""ConfigSourceName=(?<logicModuleName>.+)""; ""DeviceName=(?<resourceName>.+)""; ""DeviceId=(?<resourceId>\d+)"";$", RegexOptions.Singleline)),
		new(83,
			AuditEventEntityType.Collector,
			new(@"^(?<action>Add|Update|Delete) the collector (?<collectorId>\d+) \(hostname=(?<collectorName>.+), desc=(?<collectorDescription>.+)\)$", RegexOptions.Singleline)),
		new(84,
			AuditEventEntityType.Device,
			new(@"^Schedule (?<action>auto-discover poll request), deviceId=(?<resourceId>.+), collectorId=(?<collectorId>\d+), requestId=(?<requestId>\d+)$", RegexOptions.Singleline)),
		new(85,
			AuditEventEntityType.DataSource,
			new(@"^""Action=(?<action>Add|Update)""; ""Type=DataSource""; ""LogicModuleName=(?<logicModuleName>.+)""; ""Device=NA""; (""LogicModuleId=(?<logicModuleId>\d+?)""; )?""Description=(?<description>.*)"";?$", RegexOptions.Singleline)),
		new(86,
			AuditEventEntityType.Device,
			new(@"^""Action=(?<action>Schedule)""; ""Type=Device""; ""DeviceName=(?<resourceName>.+)""; ""DeviceId=(?<resourceId>\d+)""; ""Description=(?<description>.+)""$", RegexOptions.Singleline)),
	];

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
			Id = logItem.Id,
			DateTime = logItem.HappenedOnUtc,
			PerformedByUsername = logItem.PerformedByUsername,
			Host = logItem.IpAddress,
			OriginalDescription = logItem.Description,
			SessionId = logItem.SessionId,
			OriginatorType =
				logItem.PerformedByUsername.StartsWith("System:", StringComparison.Ordinal) ? AuditEventOriginatorType.System :
				logItem.PerformedByUsername == "k8smonitoring" ? AuditEventOriginatorType.CollectorKubernetes :
				logItem.PerformedByUsername == "lmsupport" ? AuditEventOriginatorType.CollectorOther :
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

		auditEvent.ActionType = GetAction(match);
		auditEvent.OutcomeType = match.Groups["failed"].Success ? AuditEventOutcomeType.Failure : AuditEventOutcomeType.Success;
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
			case 36:
				auditEvent.ActionType = AuditEventActionType.GeneralApi;
				break;
			case 37:
				auditEvent.ActionType = AuditEventActionType.GeneralApi;
				auditEvent.OutcomeType = AuditEventOutcomeType.Failure;
				break;
			case 41:
				auditEvent.ActionType = AuditEventActionType.GeneralApi;
				break;
			case 69:
				auditEvent.OutcomeType = AuditEventOutcomeType.Failure;
				break;
			case 70:
			case 71:
			case 72:
				auditEvent.ActionType = AuditEventActionType.Update;
				break;
			default:
				break;
		}

		var resourceIdString = GetGroupValueAsTypeOrNull<string>(match, "resourceId");
		auditEvent.ResourceGroupId = GetGroupValueAsStructOrNull<int>(match, "resourceGroupId");
		auditEvent.ResourceGroupName = GetGroupValueAsTypeOrNull<string>(match, "resourceGroupName");
		auditEvent.AlertId = GetGroupValueAsTypeOrNull<string>(match, "alertId");
		auditEvent.AlertNote = GetGroupValueAsTypeOrNull<string>(match, "alertNote");
		auditEvent.ApiTokenId = GetGroupValueAsTypeOrNull<string>(match, "apiTokenId");
		auditEvent.ApiPath = GetGroupValueAsTypeOrNull<string>(match, "apiPath");
		auditEvent.ApiMethod = GetGroupValueAsTypeOrNull<string>(match, "apiMethod");
		auditEvent.CollectorGroupId = GetGroupValueAsStructOrNull<int>(match, "collectorGroupId");
		auditEvent.CollectorGroupName = GetGroupValueAsTypeOrNull<string>(match, "collectorGroupName");
		auditEvent.CollectorId = GetGroupValueAsStructOrNull<int>(match, "collectorId");
		auditEvent.CollectorName = GetGroupValueAsTypeOrNull<string>(match, "collectorName");
		auditEvent.CollectorDescription = GetGroupValueAsTypeOrNull<string>(match, "collectorDescription");
		auditEvent.Command = GetGroupValueAsTypeOrNull<string>(match, "command");
		auditEvent.DataSourceNewInstanceIds = GetGroupValueAsTypeOrNull<List<int>>(match, "dataSourceNewInstanceIds");
		auditEvent.DataSourceNewInstanceNames = GetGroupValueAsTypeOrNull<List<string>>(match, "dataSourceNewInstanceNames");
		auditEvent.DataSourceDeletedInstanceIds = GetGroupValueAsTypeOrNull<List<int>>(match, "dataSourceDeletedInstanceIds");
		auditEvent.DataSourceDeletedInstanceNames = GetGroupValueAsTypeOrNull<List<string>>(match, "dataSourceDeletedInstanceNames");
		auditEvent.Description = GetGroupValueAsTypeOrNull<string>(match, "description");
		auditEvent.EndDownTime = GetGroupValueAsTypeOrNull<string>(match, "endDownTime");
		auditEvent.MonthlyMetrics = GetGroupValueAsStructOrNull<long>(match, "monthlyMetrics");
		auditEvent.ResourceHostname = GetGroupValueAsTypeOrNull<string>(match, "resourceHostname");
		auditEvent.RemoteSessionType = GetGroupValueAsTypeOrNull<string>(match, "remoteSessionType");
		auditEvent.RemoteSessionId = GetGroupValueAsStructOrNull<long>(match, "remoteSessionId");
		auditEvent.RequestId = GetGroupValueAsStructOrNull<long>(match, "requestId");
		auditEvent.RestrictSso = GetGroupValueAsStructOrNull<bool>(match, "restrictSso");
		auditEvent.StartDownTime = GetGroupValueAsTypeOrNull<string>(match, "startDownTime");
		auditEvent.UserRole = GetGroupValueAsTypeOrNull<string>(match, "userRole");

		auditEvent.InstanceId = GetGroupValueAsStructOrNull<int>(match, "instanceId");
		auditEvent.InstanceName = GetGroupValueAsTypeOrNull<string>(match, "instanceName");
		auditEvent.LogicModuleId = GetGroupValueAsStructOrNull<int>(match, "logicModuleId");
		auditEvent.LogicModuleName = GetGroupValueAsTypeOrNull<string>(match, "logicModuleName");
		auditEvent.LogicModuleVersion = GetGroupValueAsStructOrNull<int>(match, "logicModuleVersion");
		auditEvent.PropertyName = GetGroupValueAsTypeOrNull<string>(match, "propertyName");
		auditEvent.PropertyValue = GetGroupValueAsTypeOrNull<string>(match, "propertyValue");
		auditEvent.RemoteSessionId = GetGroupValueAsStructOrNull<int>(match, "remoteSessionId");
		auditEvent.Time = GetGroupValueAsTypeOrNull<string>(match, "time");
		auditEvent.UserEmail = GetGroupValueAsTypeOrNull<string>(match, "userEmail");
		auditEvent.UserId = GetGroupValueAsStructOrNull<int>(match, "userId");
		auditEvent.UserName = GetGroupValueAsTypeOrNull<string>(match, "userName");
		auditEvent.WildValue = GetGroupValueAsTypeOrNull<string>(match, "wildValue");

		if (match.Groups["multipleHosts"].Success)
		{
			var k8sHosts = match.Groups["multipleHosts"].ToString().Split(',').Select(h => h.Trim());
			if (k8sHosts.Any())
			{
				auditEvent.ResourceIds ??= [];
				auditEvent.ResourceNames ??= [];
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
			var resourceName = GetGroupValueAsTypeOrNull<string>(match, "resourceName");
			auditEvent.ResourceIds = resourceId is null ? null : new() { resourceId.Value };
			auditEvent.ResourceNames = resourceName is null ? null : new() { resourceName };
		}

		return auditEvent;
	}


	private static T? GetGroupValueAsStructOrNull<T>(Match match, string groupName) where T : struct
	{
		if (!match.Groups[groupName].Success)
		{
			return default;
		}

		var stringValue = match.Groups[groupName].Value;

		if (stringValue == "NA")
		{
			return default;
		}

		if (typeof(T) == typeof(int))
		{
			var result = int.Parse(stringValue, CultureInfo.InvariantCulture);
			return result is T t ? t : default;
		}

		if (typeof(T) == typeof(long))
		{
			var result = long.Parse(stringValue, CultureInfo.InvariantCulture);
			return result is T t ? t : default;
		}

		if (typeof(T) == typeof(bool))
		{
			if (bool.TryParse(stringValue, out var boolValue))
			{
				return boolValue is T t ? t : default;
			}

			return default;
		}

		throw new NotSupportedException($"Type {typeof(T)} is not supported");
	}

	private static T? GetGroupValueAsTypeOrNull<T>(Match match, string groupName) where T : class
	{
		if (!match.Groups[groupName].Success)
		{
			return default;
		}

		var stringValue = match.Groups[groupName].Value;

		if (typeof(T) == typeof(string))
		{
			return stringValue is T t ? t : default;
		}

		if (typeof(T) == typeof(List<string>))
		{
			return stringValue
				.Split(',')
				.ToList() is T t ? t : default;
		}

		if (typeof(T) == typeof(List<int>))
		{
			return stringValue
				.Split(',')
				.Select(int.Parse)
				.ToList() is T t ? t : default;
		}

		throw new NotSupportedException($"Type {typeof(T)} is not supported");
	}

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

		if (value.Groups["login"].Success)
		{
			return AuditEventActionType.Login;
		}

		if (value.Groups["discardedEventAlert"].Success)
		{
			return AuditEventActionType.DiscardedEventAlert;
		}

		if (value.Groups["alertId"].Success)
		{
			return AuditEventActionType.Update;
		}

		return value.Groups["action"].Value.ToUpperInvariant() switch
		{
			"ADD" or "CREATE" => AuditEventActionType.Create,
			"COLLECT NOW" => AuditEventActionType.CollectNow,
			"STARTED" => AuditEventActionType.Start,
			"TERMINATED" => AuditEventActionType.End,
			"FETCH" or "VIEW" => AuditEventActionType.Read,
			"LOGIN" => AuditEventActionType.Login,
			"UPDAT" or "UPDATE" or "EDIT" => AuditEventActionType.Update,
			"DELET" or "DELETE" => AuditEventActionType.Delete,
			"ENABLE" => AuditEventActionType.Enable,
			"DOWNLOAD" => AuditEventActionType.Download,
			"DISABLE" => AuditEventActionType.Disable,
			"REQUEST REMOTE" => AuditEventActionType.RequestRemoteSession,
			"RE-BALANCED" => AuditEventActionType.Rebalance,
			"FORGOT PASSWORD" => AuditEventActionType.RequestPasswordReset,
			"RUN" => AuditEventActionType.Run,
			"AUTO-DISCOVER POLL REQUEST" => AuditEventActionType.AutoDiscoveryPollRequest,
			"REGULAR DEVICE TOTAL MONTHLY METRICS" => AuditEventActionType.RegularDeviceTotalMonthlyMetrics,
			"SCHEDULE" => AuditEventActionType.ScheduleActiveDiscovery,
			_ => AuditEventActionType.None
		};
	}

	private class LogItemRegex(int id, AuditEventEntityType entityType, Regex regex)
	{
		public int Id { get; set; } = id;

		public AuditEventEntityType EntityType { get; set; } = entityType;

		public Regex Regex { get; set; } = regex;
	}

}
