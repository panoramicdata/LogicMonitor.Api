﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A Resource Property update DTO
/// Supports put and patch only
/// </summary>
[DataContract]
public class InstancePropertyUpdateDto : IHasSingletonEndpoint
{
	/// <summary>
	/// Resource Ids
	/// </summary>
	[DataMember(Name = "resourceIds")]
	public Dictionary<string, string> ResourceIds { get; set; }

	/// <summary>
	/// DataSource unique name
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSource { get; set; }

	/// <summary>
	/// DataSource Display Name
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; }

	/// <summary>
	/// Instance Name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; }

	/// <summary>
	/// Instance Properties
	/// </summary>
	[DataMember(Name = "instanceProperties")]
	public Dictionary<string, string> InstanceProperties { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "instance_property/ingest";
}
