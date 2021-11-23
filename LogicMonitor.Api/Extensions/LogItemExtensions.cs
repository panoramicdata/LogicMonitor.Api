namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Log item extensions
/// </summary>
public static class LogItemExtensions
{
	private static readonly Regex UpdateResourceRegex = new(@"Update host<(?<resourceId>\d+), (?<resourceHostname>.+?)> \(monitored by collector <(?<collectorId>\d+), (?<collectorName>.+?)>\), (?<additionalInfo>.+?), ( via API token (?<apiTokenId>.+))?$");
	private static readonly Regex UpdateDeviceDataSourceInstanceRegex = new(@"""Action=Update""; ""Type=Instance""; ""Device=(?<resourceHostname>.+?)""; ""InstanceId=(?<instanceId>\d+)""; ""Description=(?<description>\d+)$");
	private static readonly Regex CreateSdtRegex = new(@"Update host<(?<resourceId>\d+), (?<resourceHostname>.+?)> \(monitored by collector <(?<collectorId>\d+), (?<collectorName>.+?)>\), (?<additionalInfo>.+?), ( via API token (?<apiTokenId>.+))?$");

	/// <summary>
	/// Converts a logItem to an AuditItem
	/// </summary>
	/// <param name="logItem"></param>
	public static AuditEvent ToAuditItem(this LogItem logItem)
	{
		var auditEvent = new AuditEvent
		{
			DateTime = logItem.HappenedOnUtc,
			UserName = logItem.UserName,
			Host = logItem.IpAddress,
			OriginalDescription = logItem.Description,
			SessionId = logItem.SessionId,
			OriginatorType =
				logItem.UserName.StartsWith("System:") ? AuditEventOriginatorType.System :
				logItem.UserName == "k8smonitoring" ? AuditEventOriginatorType.CollectorKubernetes :
				logItem.UserName == "lmsupport" ? AuditEventOriginatorType.CollectorOther :
				AuditEventOriginatorType.User,
		};

		// Interpret the description field
		if (logItem.Description.StartsWith("Update host"))
		{
			// Example: Update host<2229, reportmagic-api.reportmagic-alpha.deploy-f8f96bc9-7d74-446f-a386-2c767fa8f5ce> (monitored by collector <254, pdl-k8s>), ,  via API token Xxxxxxxxxxxxxxxxxxxx
			auditEvent.ActionType = AuditEventActionType.Update;
			auditEvent.EntityType = AuditEventEntityType.Resource;
			var updateResourceMatch = UpdateResourceRegex.Match(logItem.Description);
			auditEvent.IsInterpreted = updateResourceMatch.Success;
			if (auditEvent.IsInterpreted)
			{
				auditEvent.EntityId = int.Parse(updateResourceMatch.Groups["resourceId"].Value);
				auditEvent.EntityNotes = updateResourceMatch.Groups["resourceHostname"].Value;
				auditEvent.CollectorId = int.Parse(updateResourceMatch.Groups["collectorId"].Value);
				auditEvent.CollectorName = updateResourceMatch.Groups["collectorName"].Value;
				auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
				auditEvent.ApiTokenId = updateResourceMatch.Groups["apiTokenId"].Value;
			}
		}
		else if (logItem.Description.StartsWith("\"Action=Update\"; \"Type=Instance\";"))
		{
			// Example: "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceId=NA"; "Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; "
			auditEvent.ActionType = AuditEventActionType.Update;
			auditEvent.EntityType = AuditEventEntityType.DeviceDataSourceInstance;
			var updateResourceMatch = UpdateDeviceDataSourceInstanceRegex.Match(logItem.Description);
			auditEvent.IsInterpreted = updateResourceMatch.Success;
			if (auditEvent.IsInterpreted)
			{
				auditEvent.EntityId = int.Parse(updateResourceMatch.Groups["resourceId"].Value);
				auditEvent.EntityNotes = updateResourceMatch.Groups["resourceHostname"].Value;
				auditEvent.CollectorId = int.Parse(updateResourceMatch.Groups["collectorId"].Value);
				auditEvent.CollectorName = updateResourceMatch.Groups["collectorName"].Value;
				auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
				auditEvent.ApiTokenId = updateResourceMatch.Groups["apiTokenId"].Value;
			}
		}
		else if (logItem.Description.StartsWith("\"Action=Add\"; \"Type=SDT\";"))
		{
			// Example: "Action=Add"; "Type=SDT"; "Description= Add SDT for Website Group Beta with scheduled downtime from 2021-01-07 12:15:00 GMT to 2021-01-07 13:10:00 GMT "
			auditEvent.ActionType = AuditEventActionType.Create;
			auditEvent.EntityType = AuditEventEntityType.ScheduledDownTime;
			var updateResourceMatch = CreateSdtRegex.Match(logItem.Description);
			auditEvent.IsInterpreted = updateResourceMatch.Success;
			if (auditEvent.IsInterpreted)
			{
				auditEvent.EntityId = int.Parse(updateResourceMatch.Groups["resourceId"].Value);
				auditEvent.EntityNotes = updateResourceMatch.Groups["resourceHostname"].Value;
				auditEvent.CollectorId = int.Parse(updateResourceMatch.Groups["collectorId"].Value);
				auditEvent.CollectorName = updateResourceMatch.Groups["collectorName"].Value;
				auditEvent.AdditionalInformation = updateResourceMatch.Groups["additionalInfo"].Value;
				auditEvent.ApiTokenId = updateResourceMatch.Groups["apiTokenId"].Value;
			}
		}
		else
		{
			// Not recognised
		}
		return auditEvent;
	}
}
