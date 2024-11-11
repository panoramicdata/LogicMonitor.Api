namespace LogicMonitor.Api.Settings;

/// <summary>
/// Kubernetes Resource Stats
/// </summary>
[DataContract]
public class KubernetesResourceStats
{
	/// <summary>
	/// Storage Class count
	/// </summary>
	[DataMember(Name = "Storage Classes")]
	public int StorageClassCount { get; set; }

	/// <summary>
	/// Pod disruption budget count
	/// </summary>
	[DataMember(Name = "Pod Disruption Budgets")]
	public int PodDisruptionBudgetCount { get; set; }

	/// <summary>
	/// Secret count
	/// </summary>
	[DataMember(Name = "Secrets")]
	public int SecretCount { get; set; }

	/// <summary>
	/// Config map count
	/// </summary>
	[DataMember(Name = "ConfigMaps")]
	public int ConfigMapCount { get; set; }

	/// <summary>
	/// Pod count
	/// </summary>
	[DataMember(Name = "Pods")]
	public int PodCount { get; set; }

	/// <summary>
	/// Deployment count
	/// </summary>
	[DataMember(Name = "Deployments")]
	public int DeploymentCount { get; set; }

	/// <summary>
	/// Custom resource definition count
	/// </summary>
	[DataMember(Name = "Custom Resource Definitions")]
	public int CustomResourceDefinitionCount { get; set; }

	/// <summary>
	/// Replica set count
	/// </summary>
	[DataMember(Name = "ReplicaSets")]
	public int ReplicaSetCount { get; set; }

	/// <summary>
	/// Endpoint count
	/// </summary>
	[DataMember(Name = "Endpoints")]
	public int EndpointCount { get; set; }

	/// <summary>
	/// Job count
	/// </summary>
	[DataMember(Name = "Jobs")]
	public int JobCount { get; set; }

	/// <summary>
	/// Resource quota count
	/// </summary>
	[DataMember(Name = "Resource Quotas")]
	public int ResourceQuotaCount { get; set; }

	/// <summary>
	/// Role binding count
	/// </summary>
	[DataMember(Name = "Role Bindings")]
	public int RoleBindingCount { get; set; }

	/// <summary>
	/// Persistent volume count
	/// </summary>
	[DataMember(Name = "Persistent Volumes")]
	public int PersistentVolumeCount { get; set; }

	/// <summary>
	/// Service account count
	/// </summary>
	[DataMember(Name = "Service Accounts")]
	public int ServiceAccountCount { get; set; }

	/// <summary>
	/// Cluster role count
	/// </summary>
	[DataMember(Name = "Cluster Roles")]
	public int ClusterRoleCount { get; set; }

	/// <summary>
	/// Role count
	/// </summary>
	[DataMember(Name = "Roles")]
	public int RoleCount { get; set; }

	/// <summary>
	/// Node count
	/// </summary>
	[DataMember(Name = "Nodes")]
	public int NodeCount { get; set; }

	/// <summary>
	/// Persistent volume claim count
	/// </summary>
	[DataMember(Name = "Persistent Volume Claims")]
	public int PersistentVolumeClaimCount { get; set; }

	/// <summary>
	/// Priority class count
	/// </summary>
	[DataMember(Name = "Priority Classes")]
	public int PriorityClasseCount { get; set; }

	/// <summary>
	/// Service count
	/// </summary>
	[DataMember(Name = "Services")]
	public int ServiceCount { get; set; }

	/// <summary>
	/// Cron job count
	/// </summary>
	[DataMember(Name = "CronJobs")]
	public int CronJobCount { get; set; }

	/// <summary>
	/// Ingress count
	/// </summary>
	[DataMember(Name = "Ingresses")]
	public int IngressCount { get; set; }

	/// <summary>
	/// DaemonSet count
	/// </summary>
	[DataMember(Name = "DaemonSets")]
	public int DaemonSetCount { get; set; }

	/// <summary>
	/// Network Policy count
	/// </summary>
	[DataMember(Name = "Network Policies")]
	public int NetworkPolicyCount { get; set; }

	/// <summary>
	/// Cluster Role Binding count
	/// </summary>
	[DataMember(Name = "Cluster Role Bindings")]
	public int ClusterRoleBindingCount { get; set; }

	/// <summary>
	/// StatefulSet count
	/// </summary>
	[DataMember(Name = "StatefulSets")]
	public int StatefulSetCount { get; set; }
}
