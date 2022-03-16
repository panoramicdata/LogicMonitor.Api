namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Log item extensions
/// </summary>
public static class LogItemExtensions
{
	private static readonly Regex ResourceSuccessRegex = new(
		@"^(?<action>Add|Fetch|Update) host<(?<resourceId>\d+), (?<resourceName>.+?)> \(monitored by collector <(?<collectorId>\d+), (?<collectorName>.+?)>\), (?<additionalInfo>.*?), ( via API token (?<apiTokenId>.+))?$",
		RegexOptions.Singleline
	);
	private static readonly Regex ResourceSuccess2Regex = new(
		@"^(?<action>Add)ed device (?<resourceName>.+?) \((?<resourceId>\d+)\)  via API token (?<apiTokenId>[^{]+?)(?<additionalInfo>.*?)$",
		RegexOptions.Singleline
	);
	private static readonly Regex ResourceGroupPropertySuccessRegex = new(
		@"^(?<action>Add|Fetch|Update) the host group\((?<resourceGroupName>.+?)\)'s property\(name=(?<propertyName>.+?)\) via API token (?<apiTokenId>.+?)..$",
		RegexOptions.Singleline
	);
	private static readonly Regex DeviceDataSourceInstanceByIdRegex = new(
		@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceId=(?<instanceId>.+?)""; ""Description=(?<description>.+?)""$",
		RegexOptions.Singleline
	);
	private static readonly Regex DeviceDataSourceInstanceByNameRegex = new(
		@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>(.+?deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),instanceChanges=\[instanceId=(?<instanceId>\d+?),oldValue=(?<instanceOldValue>.+?),newValue=(?<instanceNewValue>.+?)\].+?))""$",
		RegexOptions.Singleline
	);
	private static readonly Regex DeviceDataSourceInstanceByNameFoundNewInstancesRegex = new(
		@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Found new instance\(s\) for: (?<resourceName>.+?) \(CollectorID=(?<collectorId>\d+?)\) \[(?<dataSourceName>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),dataSourceNewInstanceId\(s\)=(?<dataSourceNewInstanceIds>\d+?)];)""$",
		RegexOptions.Singleline
	);
	private static readonly Regex DeviceDataSourceInstanceByNameDeletedInstancesRegex = new(
	   @"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Instance\(s\) disappeared from: (?<resourceName>.+?) \(CollectorID=(?<collectorId>\d+?)\) \[(?<dataSourceName>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),dataSourceDeletedInstanceId\(s\)=(?<instanceIds>\d+?)\];)""$",
	   RegexOptions.Singleline
	);
	private static readonly Regex SdtRegex = new(
		@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=SDT""; ""Device=(?<resourceName>.+?)""; ""InstanceId=(?<instanceId>.+?)""; ""Description=(?<description>\d+)""$",
		RegexOptions.Singleline
	);
	private static readonly Regex DeviceOtherRegex = new(
		@"^""Action=(?<action>Add|Fetch|Update)""; ""Type=Device""; ""Device=(?<resourceName>.+?) \((?<resourceId>.+?)\)""; ""Description=(?<additionalInfo>.+?)""$",
		RegexOptions.Singleline
	);
	private static readonly Regex CollectorScheduledHealthCheck = new(
		@"^Scheduled health check scripts for all collectors$",
		RegexOptions.Singleline
	);
	// "Action=Add"; "Type=DataSource"; "DataSourceName=Kubernetes_Script_Uptime"; "DeviceName=lm-collector-panoramic-5-deploy-lm-collector-panoramic-PDL-K8S-PROD"; "DeviceId=7589"; "Description=Addition of datasource to device"; "DataSourceId=101626885"; "DeviceDataSourceId=532526"
	private static readonly Regex DataSourceRegex = new(
		@"^""""Action=(?<action>Add|Fetch|Update)""; ""Type=DataSource""; ""DataSourceName=(?<dataSourceName>.+?)""; ""DeviceName=(?<resourceName>.+?)""; ""DeviceId=(?<resourceId>\d+?)""; ""Description=(?<additionalInfo>.+?)""; ""DataSourceId=(?<dataSourceId>\d+?)""; ""DeviceDataSourceId=(?<deviceDataSourceId>\d+?)""$",
		RegexOptions.Singleline
	);

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
		Match match;
		if ((match = ResourceSuccessRegex.Match(logItem.Description)).Success)
		{
			// Example: Update host<2229, reportmagic-api.reportmagic-alpha.deploy-f8f96bc9-7d74-446f-a386-2c767fa8f5ce> (monitored by collector <254, pdl-k8s>), ,  via API token Xxxxxxxxxxxxxxxxxxxx

			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.Resource;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			var resourceIdString = match.Groups["resourceId"].Value;
			auditEvent.ResourceId = resourceIdString == "NA" ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = match.Groups["resourceName"].Value;
			auditEvent.CollectorId = int.Parse(match.Groups["collectorId"].Value, CultureInfo.InvariantCulture);
			auditEvent.CollectorName = match.Groups["collectorName"].Value;
			auditEvent.AdditionalInformation = match.Groups["additionalInfo"].Value;
			auditEvent.ApiTokenId = match.Groups["apiTokenId"].Value;
			return auditEvent;
		}
		if ((match = ResourceSuccess2Regex.Match(logItem.Description)).Success)
		{
			// Example: Added device lm-collector-panoramic-5-deploy-lm-collector-panoramic-PDL-K8S-PROD (7589)  via API token MZkW3Ldwg5S84s5eWUc7{[{"name":"kubernetes.label.app","value":"lm-collector-panoramic-5"}, {"name":"auto.clustername","value":"PDL-K8S-PROD"}, {"name":"auto.selflink","value":"/apis/apps/v1/namespaces/lm-collector-panoramic/deployments/lm-collector-panoramic-5"}, {"name":"kubernetes.resourceCreatedOn","value":"1646988769"}, {"name":"auto.uid","value":"8536007f-be25-445d-bea5-b1434c1e6ca8"}, {"name":"auto.resourcename","value":"lm-collector-panoramic-5-deploy-lm-collector-panoramic-PDL-K8S-PROD"}, {"name":"auto.namespace","value":"lm-collector-panoramic"}, {"name":"auto.name","value":"lm-collector-panoramic-5"}, {"name":"system.categories","value":"KubernetesDeployment"}]}

			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.Resource;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			var resourceIdString = match.Groups["resourceId"].Value;
			auditEvent.ResourceId = resourceIdString == "NA" ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = match.Groups["resourceName"].Value;
			auditEvent.ApiTokenId = match.Groups["apiTokenId"].Value;
			auditEvent.AdditionalInformation = match.Groups["additionalInfo"].Value;
			return auditEvent;
		}

		if ((match = DeviceDataSourceInstanceByIdRegex.Match(logItem.Description)).Success)
		{
			// Example: "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceId=NA"; "Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; "
			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			var resourceIdString = match.Groups["resourceId"].Value;
			auditEvent.ResourceId = (resourceIdString == "NA" || string.IsNullOrEmpty(resourceIdString)) ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = match.Groups["resourceName"].Value;
			auditEvent.AdditionalInformation = match.Groups["additionalInfo"].Value;
			auditEvent.ApiTokenId = match.Groups["apiTokenId"].Value;
			return auditEvent;
		}

		if ((match = DeviceDataSourceInstanceByNameFoundNewInstancesRegex.Match(logItem.Description)).Success)
		{
			// Example: "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceId=NA"; "Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; "
			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			var resourceIdString = match.Groups["resourceId"].Value;
			auditEvent.ResourceId = (resourceIdString == "NA" || string.IsNullOrEmpty(resourceIdString)) ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = match.Groups["resourceName2"].Value;
			auditEvent.AdditionalInformation = match.Groups["additionalInfo"].Value;
			var dataSourceIdString = match.Groups["dataSourceId"].Value;
			auditEvent.DataSourceId = int.Parse(dataSourceIdString, CultureInfo.InvariantCulture);
			auditEvent.DataSourceName = match.Groups["dataSourceName"].Value;
			auditEvent.InstanceName = match.Groups["instanceName"].Value;
			var dataSourceNewInstanceIdsString = match.Groups["dataSourceNewInstanceIds"].Value;
			auditEvent.DataSourceNewInstanceIds = dataSourceNewInstanceIdsString
				.Split(',')
				.Select(subString => int.TryParse(subString, out var intValue) ? intValue : (int?)null)
				.ToList();
			return auditEvent;
		}

		if ((match = DeviceDataSourceInstanceByNameDeletedInstancesRegex.Match(logItem.Description)).Success)
		{
			// Example: "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceId=NA"; "Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; "
			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			var resourceIdString = match.Groups["resourceId"].Value;
			auditEvent.ResourceId = (resourceIdString == "NA" || string.IsNullOrEmpty(resourceIdString)) ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = match.Groups["resourceName"].Value;
			auditEvent.AdditionalInformation = match.Groups["additionalInfo"].Value;
			var dataSourceIdString = match.Groups["dataSourceId"].Value;
			auditEvent.DataSourceId = int.Parse(dataSourceIdString, CultureInfo.InvariantCulture);
			auditEvent.DataSourceName = match.Groups["dataSourceName"].Value;
			auditEvent.InstanceName = match.Groups["instanceName"].Value;
			var dataSourceNewInstanceIdsString = match.Groups["instanceIds"].Value;
			auditEvent.DataSourceNewInstanceIds = dataSourceNewInstanceIdsString
				.Split(',')
				.Select(subString => int.TryParse(subString, out var intValue) ? intValue : (int?)null)
				.ToList();

			return auditEvent;
		}

		if ((match = SdtRegex.Match(logItem.Description)).Success)
		{
			// Example: "Action=Add"; "Type=SDT"; "Description= Add SDT for Website Group Beta with scheduled downtime from 2021-01-07 12:15:00 GMT to 2021-01-07 13:10:00 GMT "
			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.ScheduledDownTime;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			var resourceIdString = match.Groups["resourceId"].Value;
			auditEvent.ResourceId = resourceIdString == "NA" ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = match.Groups["resourceName"].Value;
			auditEvent.CollectorId = int.Parse(match.Groups["collectorId"].Value, CultureInfo.InvariantCulture);
			auditEvent.CollectorName = match.Groups["collectorName"].Value;
			auditEvent.AdditionalInformation = match.Groups["additionalInfo"].Value;
			auditEvent.ApiTokenId = match.Groups["apiTokenId"].Value;
			return auditEvent;
		}

		if ((match = DeviceOtherRegex.Match(logItem.Description)).Success)
		{
			// Example: "Action=Add"; "Type=Device"; "Device=ReportMagic beta-API (0)"; "Description=Failed device operation,  adddevice_failed : error  (invalid normal device name: ReportMagic beta-API)"
			// Example: "Action=Add"; "Type=Device"; "Device=ReportMagic alpha-Worker (0)"; "Description=Failed device operation,  adddevice_failed : error  (invalid normal device name: ReportMagic alpha-Worker)"
			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.Resource;
			auditEvent.OutcomeType = AuditEventOutcomeType.Failure;
			auditEvent.ResourceId = int.Parse(match.Groups["resourceId"].Value, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = match.Groups["resourceName"].Value;
			auditEvent.AdditionalInformation = match.Groups["additionalInfo"].Value;
			return auditEvent;
		}

		if ((match = DeviceDataSourceInstanceByNameRegex.Match(logItem.Description)).Success)
		{
			// Example: "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceName=NA"; "Description=Value(s) changed for: pdl-k8s-test-03.panoramicdata.com-node-PDL-K8S-TEST (CollectorID=297) [Critical Linux Processes-java from 9947 to 22713]; valueChanges=[deviceId=3271,dataSourceId=94545589,instanceChanges=[instanceId=263219850,oldValue=22713,newValue=9947];];'"
			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			auditEvent.ResourceName = match.Groups["resourceName"].Value;
			var instanceIdString = match.Groups["instanceId"].Value;
			auditEvent.InstanceId = instanceIdString == "NA" ? null : int.Parse(instanceIdString, CultureInfo.InvariantCulture);
			auditEvent.InstanceName = match.Groups["instanceName"].Value;
			auditEvent.AdditionalInformation = match.Groups["additionalInfo"].Value;
			return auditEvent;
		}

		if ((match = CollectorScheduledHealthCheck.Match(logItem.Description)).Success)
		{
			// Example: "Scheduled health check scripts for all collectors"
			auditEvent.ActionType = AuditEventActionType.ScheduledHealthCheckScript;
			auditEvent.EntityType = AuditEventEntityType.AllCollectors;
			auditEvent.OutcomeType = AuditEventOutcomeType.Unknown;
			return auditEvent;
		}

		if ((match = ResourceGroupPropertySuccessRegex.Match(logItem.Description)).Success)
		{
			// Example: "Update the host group(PDL - Panoramic Data/Datacenter/Private/Servers/Kubernetes Cluster: PDL-K8S-PROD)'s property(name=kubernetes.version.history) via API token MZkW3Ldwg5S84s5eWUc7."
			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.ResourceGroupProperty;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			auditEvent.ResourceGroupName = match.Groups["resourceGroupName"].Value;
			auditEvent.PropertyName = match.Groups["resourceGroupNamepropertyName"].Value;
			auditEvent.ApiTokenId = match.Groups["apiTokenId"].Value;
			return auditEvent;
		}
		if ((match = DataSourceRegex.Match(logItem.Description)).Success)
		{
			// Example: "Action=Add"; "Type=DataSource"; "DataSourceName=Kubernetes_Script_Uptime"; "DeviceName=lm-collector-panoramic-5-deploy-lm-collector-panoramic-PDL-K8S-PROD"; "DeviceId=7589"; "Description=Addition of datasource to device"; "DataSourceId=101626885"; "DeviceDataSourceId=532526"
			auditEvent.ActionType = GetAction(match);
			auditEvent.EntityType = AuditEventEntityType.ResourceGroupProperty;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			auditEvent.DataSourceName = match.Groups["dataSourceName"].Value;
			auditEvent.DataSourceId = int.Parse(match.Groups["dataSourceId"].Value, CultureInfo.InvariantCulture);
			auditEvent.DeviceDataSourceId = int.Parse(match.Groups["deviceDataSourceId"].Value, CultureInfo.InvariantCulture);

			auditEvent.ResourceName = match.Groups["resourceName"].Value;
			auditEvent.ResourceId = int.Parse(match.Groups["resourceId"].Value, CultureInfo.InvariantCulture);
			auditEvent.ApiTokenId = match.Groups["apiTokenId"].Value;

			auditEvent.AdditionalInformation = match.Groups["additionalInfo"].Value;

			return auditEvent;
		}

		// Not recognised
		auditEvent.ActionType = AuditEventActionType.None;
		auditEvent.EntityType = AuditEventEntityType.None;
		auditEvent.OutcomeType = AuditEventOutcomeType.None;
		return auditEvent;
	}

	private static AuditEventActionType GetAction(Match value)
		=> value.Groups["action"].Value switch
		{
			"Add" => AuditEventActionType.Create,
			"Fetch" => AuditEventActionType.Read,
			"Update" => AuditEventActionType.Update,
			"Delete" => AuditEventActionType.Delete,
			_ => AuditEventActionType.None
		};
}
