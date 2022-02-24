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
