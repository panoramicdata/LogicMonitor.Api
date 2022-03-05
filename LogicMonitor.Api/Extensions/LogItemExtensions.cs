namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Log item extensions
/// </summary>
public static class LogItemExtensions
{

	private static readonly Regex UpdateResourceSuccessRegex = new(
		@"^Update host<(?<resourceId>\d+), (?<resourceName>.+?)> \(monitored by collector <(?<collectorId>\d+), (?<collectorName>.+?)>\), (?<additionalInfo>.*?), ( via API token (?<apiTokenId>.+))?$",
		RegexOptions.Singleline
	);
	private static readonly Regex UpdateDeviceDataSourceInstanceByIdRegex = new(
		@"^""Action=Update""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceId=(?<instanceId>.+?)""; ""Description=(?<description>.+?)""$",
		RegexOptions.Singleline
	);
	private static readonly Regex UpdateDeviceDataSourceInstanceByNameRegex = new(
		@"^""Action=Update""; ""Type=Instance""; ""Device=(?<resourceName>.+?)""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>(.+?deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),instanceChanges=\[instanceId=(?<instanceId>\d+?),oldValue=(?<instanceOldValue>.+?),newValue=(?<instanceNewValue>.+?)\].+?))""$",
		RegexOptions.Singleline
	);
	private static readonly Regex UpdateDeviceDataSourceInstanceByNameFoundNewInstancesRegex = new(
	   @"^""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Found new instance\(s\) for: (?<resourceName>.+?) \(CollectorID=(?<collectorId>\d+?)\) \[(?<dataSourceName>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),dataSourceNewInstanceId\(s\)=(?<dataSourceNewInstanceIds>\d+?)];)""$",
	   RegexOptions.Singleline
	);
	private static readonly Regex UpdateDeviceDataSourceInstanceByNameDeletedInstancesRegex = new(
	   @"^""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=(?<instanceName>.+?)""; ""Description=(?<additionalInfo>Instance\(s\) disappeared from: (?<resourceName>.+?) \(CollectorID=(?<collectorId>\d+?)\) \[(?<dataSourceName>.+?)\]; New_InstanceIds=\[deviceId=(?<resourceId>\d+?),dataSourceId=(?<dataSourceId>\d+?),dataSourceDeletedInstanceId\(s\)=(?<instanceIds>\d+?)\];)""$",
	   RegexOptions.Singleline
	);
	private static readonly Regex CreateSdtRegex = new(
		@"^""Action=Add""; ""Type=SDT""; ""Device=(?<resourceName>.+?)""; ""InstanceId=(?<instanceId>.+?)""; ""Description=(?<description>\d+)""$",
		RegexOptions.Singleline
	);
	private static readonly Regex CreateDeviceFailureRegex = new(
		@"^""Action=Add""; ""Type=Device""; ""Device=(?<resourceName>.+?) \((?<resourceId>.+?)\)""; ""Description=Failed device operation,  adddevice_failed : error  \((?<additionalInfo>.+?)\)""$",
		RegexOptions.Singleline
	);
	private static readonly Regex CollectorScheduledHealthCheck = new(
		@"^Scheduled health check scripts for all collectors$",
		RegexOptions.Singleline
	);
	private static readonly Regex UpdateResourceGroupPropertySuccessRegex = new(
		@"^Update the host group\((?<resourceGroupName>.+?)\)'s property\(name=(?<propertyName>.+?)\) via API token (?<apiTokenId>.+?).$",
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
		if (UpdateResourceSuccessRegex.IsMatch(logItem.Description))
		{
			// Example: Update host<2229, reportmagic-api.reportmagic-alpha.deploy-f8f96bc9-7d74-446f-a386-2c767fa8f5ce> (monitored by collector <254, pdl-k8s>), ,  via API token Xxxxxxxxxxxxxxxxxxxx
			auditEvent.ActionType = AuditEventActionType.Update;
			auditEvent.EntityType = AuditEventEntityType.Resource;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;

			var updateResourceMatch = UpdateResourceSuccessRegex.Match(logItem.Description);

			var resourceIdString = updateResourceMatch.Groups["resourceId"].Value;
			auditEvent.ResourceId = resourceIdString == "NA" ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = updateResourceMatch.Groups["resourceName"].Value;
			auditEvent.CollectorId = int.Parse(updateResourceMatch.Groups["collectorId"].Value, CultureInfo.InvariantCulture);
			auditEvent.CollectorName = updateResourceMatch.Groups["collectorName"].Value;
			auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
			auditEvent.ApiTokenId = updateResourceMatch.Groups["apiTokenId"].Value;
		}
		else if (UpdateDeviceDataSourceInstanceByIdRegex.IsMatch(logItem.Description))
		{
			// Example: "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceId=NA"; "Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; "
			auditEvent.ActionType = AuditEventActionType.Update;
			auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			var updateResourceMatch = UpdateDeviceDataSourceInstanceByIdRegex.Match(logItem.Description);

			var resourceIdString = updateResourceMatch.Groups["resourceId"].Value;
			auditEvent.ResourceId = (resourceIdString == "NA" || string.IsNullOrEmpty(resourceIdString)) ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = updateResourceMatch.Groups["resourceName"].Value;
			auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
			auditEvent.ApiTokenId = updateResourceMatch.Groups["apiTokenId"].Value;
		}
		else if (UpdateDeviceDataSourceInstanceByNameFoundNewInstancesRegex.IsMatch(logItem.Description))
		{
			// Example: "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceId=NA"; "Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; "
			auditEvent.ActionType = AuditEventActionType.Update;
			auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			var updateResourceMatch = UpdateDeviceDataSourceInstanceByNameFoundNewInstancesRegex.Match(logItem.Description);

			var resourceIdString = updateResourceMatch.Groups["resourceId"].Value;
			auditEvent.ResourceId = (resourceIdString == "NA" || string.IsNullOrEmpty(resourceIdString)) ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = updateResourceMatch.Groups["resourceName2"].Value;
			auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
			var dataSourceIdString = updateResourceMatch.Groups["dataSourceId"].Value;
			auditEvent.DataSourceId = int.Parse(dataSourceIdString, CultureInfo.InvariantCulture);
			auditEvent.DataSourceName = updateResourceMatch.Groups["dataSourceName"].Value;
			auditEvent.InstanceName = updateResourceMatch.Groups["instanceName"].Value;
			var dataSourceNewInstanceIdsString = updateResourceMatch.Groups["dataSourceNewInstanceIds"].Value;
			auditEvent.DataSourceNewInstanceIds = dataSourceNewInstanceIdsString
				.Split(',')
				.Select(subString => int.TryParse(subString, out var intValue) ? intValue : (int?)null)
				.ToList();
		}
		else if (UpdateDeviceDataSourceInstanceByNameDeletedInstancesRegex.IsMatch(logItem.Description))
		{
			// Example: "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceId=NA"; "Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; "
			auditEvent.ActionType = AuditEventActionType.Update;
			auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			var updateResourceMatch = UpdateDeviceDataSourceInstanceByNameDeletedInstancesRegex.Match(logItem.Description);

			var resourceIdString = updateResourceMatch.Groups["resourceId"].Value;
			auditEvent.ResourceId = (resourceIdString == "NA" || string.IsNullOrEmpty(resourceIdString)) ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = updateResourceMatch.Groups["resourceName"].Value;
			auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
			var dataSourceIdString = updateResourceMatch.Groups["dataSourceId"].Value;
			auditEvent.DataSourceId = int.Parse(dataSourceIdString, CultureInfo.InvariantCulture);
			auditEvent.DataSourceName = updateResourceMatch.Groups["dataSourceName"].Value;
			auditEvent.InstanceName = updateResourceMatch.Groups["instanceName"].Value;
			var dataSourceNewInstanceIdsString = updateResourceMatch.Groups["instanceIds"].Value;
			auditEvent.DataSourceNewInstanceIds = dataSourceNewInstanceIdsString
				.Split(',')
				.Select(subString => int.TryParse(subString, out var intValue) ? intValue : (int?)null)
				.ToList();
		}
		else if (CreateSdtRegex.IsMatch(logItem.Description))
		{
			// Example: "Action=Add"; "Type=SDT"; "Description= Add SDT for Website Group Beta with scheduled downtime from 2021-01-07 12:15:00 GMT to 2021-01-07 13:10:00 GMT "
			auditEvent.ActionType = AuditEventActionType.Create;
			auditEvent.EntityType = AuditEventEntityType.ScheduledDownTime;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			var updateResourceMatch = CreateSdtRegex.Match(logItem.Description);

			var resourceIdString = updateResourceMatch.Groups["resourceId"].Value;
			auditEvent.ResourceId = resourceIdString == "NA" ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = updateResourceMatch.Groups["resourceName"].Value;
			auditEvent.CollectorId = int.Parse(updateResourceMatch.Groups["collectorId"].Value, CultureInfo.InvariantCulture);
			auditEvent.CollectorName = updateResourceMatch.Groups["collectorName"].Value;
			auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
			auditEvent.ApiTokenId = updateResourceMatch.Groups["apiTokenId"].Value;
		}
		else if (CreateDeviceFailureRegex.IsMatch(logItem.Description))
		{
			// Example: "Action=Add"; "Type=Device"; "Device=ReportMagic beta-API (0)"; "Description=Failed device operation,  adddevice_failed : error  (invalid normal device name: ReportMagic beta-API)"
			auditEvent.ActionType = AuditEventActionType.Create;
			auditEvent.EntityType = AuditEventEntityType.Resource;
			auditEvent.OutcomeType = AuditEventOutcomeType.Failure;
			var updateResourceMatch = CreateDeviceFailureRegex.Match(logItem.Description);

			auditEvent.ResourceId = int.Parse(updateResourceMatch.Groups["resourceId"].Value, CultureInfo.InvariantCulture);
			auditEvent.ResourceName = updateResourceMatch.Groups["resourceName"].Value;
			auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
		}
		else if (UpdateDeviceDataSourceInstanceByNameRegex.IsMatch(logItem.Description))
		{
			// Example: "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceName=NA"; "Description=Value(s) changed for: pdl-k8s-test-03.panoramicdata.com-node-PDL-K8S-TEST (CollectorID=297) [Critical Linux Processes-java from 9947 to 22713]; valueChanges=[deviceId=3271,dataSourceId=94545589,instanceChanges=[instanceId=263219850,oldValue=22713,newValue=9947];];'"
			auditEvent.ActionType = AuditEventActionType.Update;
			auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			var updateResourceMatch = UpdateDeviceDataSourceInstanceByNameRegex.Match(logItem.Description);

			auditEvent.ResourceName = updateResourceMatch.Groups["resourceName"].Value;
			var instanceIdString = updateResourceMatch.Groups["instanceId"].Value;
			auditEvent.InstanceId = instanceIdString == "NA" ? null : int.Parse(instanceIdString, CultureInfo.InvariantCulture);
			auditEvent.InstanceName = updateResourceMatch.Groups["instanceName"].Value;
			auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
		}
		else if (CollectorScheduledHealthCheck.IsMatch(logItem.Description))
		{
			// Example: "Scheduled health check scripts for all collectors"
			auditEvent.ActionType = AuditEventActionType.ScheduledHealthCheckScript;
			auditEvent.EntityType = AuditEventEntityType.AllCollectors;
			auditEvent.OutcomeType = AuditEventOutcomeType.Unknown;
		}
		else if (UpdateResourceGroupPropertySuccessRegex.IsMatch(logItem.Description))
		{
			// Example: "Update the host group(PDL - Panoramic Data/Datacenter/Private/Servers/Kubernetes Cluster: PDL-K8S-PROD)'s property(name=kubernetes.version.history) via API token MZkW3Ldwg5S84s5eWUc7."
			auditEvent.ActionType = AuditEventActionType.Update;
			auditEvent.EntityType = AuditEventEntityType.ResourceGroupProperty;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			var updateResourceMatch = UpdateDeviceDataSourceInstanceByNameRegex.Match(logItem.Description);

			auditEvent.ResourceGroupName = updateResourceMatch.Groups["resourceGroupName"].Value;
			auditEvent.PropertyName = updateResourceMatch.Groups["resourceGroupNamepropertyName"].Value;
			auditEvent.ApiTokenId = updateResourceMatch.Groups["apiTokenId"].Value;
		}
		else
		{
			// Not recognised
			auditEvent.ActionType = AuditEventActionType.None;
			auditEvent.EntityType = AuditEventEntityType.None;
			auditEvent.OutcomeType = AuditEventOutcomeType.None;
		}

		return auditEvent;
	}
}
