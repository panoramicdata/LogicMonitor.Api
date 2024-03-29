﻿namespace LogicMonitor.Api;

/// <summary>
/// An instance property
/// </summary>
[DataContract]
public class InstanceProperty : EntityProperty
{
	/// <summary>
	///    The property Id
	/// </summary>
	[DataMember(Name = "Id")]
	public string Id { get; set; } = string.Empty;
}
