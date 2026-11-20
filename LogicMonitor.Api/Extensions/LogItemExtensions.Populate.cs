namespace LogicMonitor.Api.Extensions;

public static partial class LogItemExtensions
{
	private static void PopulateFromMatchGroups(AuditEvent auditEvent, Match match)
	{
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
		auditEvent.DashboardName = GetGroupValueAsTypeOrNull<string>(match, "dashboardName");
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
		auditEvent.WidgetName = GetGroupValueAsTypeOrNull<string>(match, "widgetName");

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
	}

	/// <summary>
	/// Resolves the resource id/name pair (or, for Kubernetes messages, the set of them).
	/// </summary>
	private static void PopulateResources(AuditEvent auditEvent, Match match)
	{
		if (match.Groups["multipleHosts"].Success)
		{
			PopulateKubernetesResources(auditEvent, match);
			return;
		}

		var resourceIdString = GetGroupValueAsTypeOrNull<string>(match, "resourceId");
		int? resourceId = (resourceIdString == "NA" || string.IsNullOrEmpty(resourceIdString)) ? null : int.Parse(resourceIdString, CultureInfo.InvariantCulture);
		var resourceName = GetGroupValueAsTypeOrNull<string>(match, "resourceName");
		auditEvent.ResourceIds = resourceId is null ? null : new() { resourceId.Value };
		auditEvent.ResourceNames = resourceName is null ? null : new() { resourceName };
	}

	private static void PopulateKubernetesResources(AuditEvent auditEvent, Match match)
	{
		var k8sHosts = match.Groups["multipleHosts"].ToString().Split(',').Select(h => h.Trim());
		if (!k8sHosts.Any())
		{
			return;
		}

		auditEvent.ResourceIds ??= [];
		auditEvent.ResourceNames ??= [];
		foreach (var k8sHost in k8sHosts)
		{
			var k8sHostMatch = SafeMatch(_k8sHostRegex, k8sHost);
			if (k8sHostMatch.Success)
			{
				auditEvent.ResourceIds.Add(int.Parse(k8sHostMatch.Groups["resourceId"].Value, CultureInfo.InvariantCulture));
				auditEvent.ResourceNames.Add(k8sHostMatch.Groups["resourceName"].Value.ToString());
			}
		}
	}

	/// <summary>
	/// For the patterns that list affected DataSource instances, expands that list into the
	/// instance ids/names and the distinct set of resource ids they belong to.
	/// </summary>
	private static void PopulateAffectedInstances(AuditEvent auditEvent, Match match)
	{
		if (auditEvent.MatchedRegExId is not (96 or 108 or 111))
		{
			return;
		}

		if (!match.Groups["affectedInstances"].Success)
		{
			return;
		}

		var dataSourceInstanceIds = new List<int>();
		var dataSourceInstanceNames = new List<string>();
		var resourceIds = new List<int>();
		foreach (var affectedInstanceMatch in SafeMatches(_dataSourceInstanceEntryRegex, match.Groups["affectedInstances"].Value))
		{
			if (!affectedInstanceMatch.Success)
			{
				continue;
			}

			var instanceName = affectedInstanceMatch.Groups["instanceName"].Value.TrimStart().TrimStart(',', '"');
			var resourceId = int.Parse(affectedInstanceMatch.Groups["resourceId"].Value, CultureInfo.InvariantCulture);
			dataSourceInstanceNames.Add(instanceName);
			dataSourceInstanceIds.Add(int.Parse(affectedInstanceMatch.Groups["instanceId"].Value, CultureInfo.InvariantCulture));
			if (!resourceIds.Contains(resourceId))
			{
				resourceIds.Add(resourceId);
			}
		}

		auditEvent.DataSourceNewInstanceNames = dataSourceInstanceNames;
		auditEvent.DataSourceNewInstanceIds = dataSourceInstanceIds;
		auditEvent.ResourceIds = new(resourceIds);
	}
}
