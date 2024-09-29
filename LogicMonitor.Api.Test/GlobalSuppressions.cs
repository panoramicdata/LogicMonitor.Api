// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
	"Performance",
	"CA1848:Use the LoggerMessage delegates",
	Justification = "Work required not worth the performance benefits",
	Scope = "namespaceanddescendants",
	Target = "~N:LogicMonitor.Api.Test")
]
[assembly: SuppressMessage(
	"Naming",
	"CA1707:Identifiers should not contain underscores",
	Justification = "Underscores are appropriate for unit tests",
	Scope = "namespaceanddescendants",
	Target = "~N:LogicMonitor.Api.Test")
]
[assembly: SuppressMessage(
	"Reliability",
	"CA2007:Consider calling ConfigureAwait on the awaited task",
	Justification = "ConfigureAwait is no longer recommended in XUnit tests", Scope = "module")
]

#region Analyzer bug: https://github.com/fluentassertions/fluentassertions.analyzers/issues/383

[assembly: SuppressMessage(
	"FluentAssertionTips",
	"FAA0001:Simplify Assertion",
	Justification = "Analyzer bug",
	Scope = "member",
	Target = "~M:LogicMonitor.Api.Test.Alerts.AlertTests.GetAlerts_SdtsMatchRequest~System.Threading.Tasks.Task")
]
[assembly: SuppressMessage(
	"FluentAssertionTips",
	"FAA0001:Simplify Assertion",
	Justification = "Analyzer bug",
	Scope = "member",
	Target = "~M:LogicMonitor.Api.Test.Alerts.AlertTests.SdtFilter_Works~System.Threading.Tasks.Task")
]
[assembly: SuppressMessage(
	"FluentAssertionTips",
	"FAA0001:Simplify Assertion",
	Justification = "Analyzer bug",
	Scope = "member",
	Target = "~M:LogicMonitor.Api.Test.Alerts.NewAlertTests.GetAlerts_SdtsMatchRequest~System.Threading.Tasks.Task")
]
[assembly: SuppressMessage(
	"FluentAssertionTips",
	"FAA0001:Simplify Assertion",
	Justification = "Analyzer bug",
	Scope = "member",
	Target = "~M:LogicMonitor.Api.Test.Data.RawDataTests.FetchInstanceData~System.Threading.Tasks.Task")
]

#endregion