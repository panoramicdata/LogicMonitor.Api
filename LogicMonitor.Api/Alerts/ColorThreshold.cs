﻿namespace LogicMonitor.Api.Alerts;

/// <summary>
/// Color threshold
/// </summary>
[DataContract]
public class ColorThreshold
{
	/// <summary>
	/// The level
	/// </summary>
	[DataMember(Name = "level")]
	public int Level { get; set; }

	/// <summary>
	/// The threshold
	/// </summary>
	[DataMember(Name = "threshold")]
	public int Threshold { get; set; }

	/// <summary>
	/// The relation
	/// </summary>
	[DataMember(Name = "relation")]
	public string Relation { get; set; } = string.Empty;
}

