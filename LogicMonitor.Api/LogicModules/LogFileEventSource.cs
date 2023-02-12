﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// LogFileEventSource
/// </summary>

[DataContract]
public class LogFileEventSource : EventSource
{
	/// <summary>
	/// log files
	/// </summary>
	[DataMember(Name = "logFiles", IsRequired = false)]
	public List<LogFile> LogFiles { get; set; } = new();
}