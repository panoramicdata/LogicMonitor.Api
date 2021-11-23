// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.


[assembly: SuppressMessage(
	"Performance",
	"RCS1197:Optimize StringBuilder.Append/AppendLine call.",
	Justification = "Code readability for an infrequently-used call.",
	Scope = "member",
	Target = "~M:LogicMonitor.Api.Extensions.StringBuilderExtensions.AppendIfNotNullInternal(System.Text.StringBuilder,System.String,System.Object,System.String,System.String,System.Boolean)")]

[assembly: SuppressMessage(
	"Security",
	"SCS0028:Type information used to serialize and deserialize objects",
	Justification = "Unavoidable for type determination.  Assessed as safe, as LogicMonitor itself is sending the data.",
	Scope = "member",
	Target = "~M:LogicMonitor.Api.PortalResponse`1.GetObject(Newtonsoft.Json.JsonConverter[])~`0")]

[assembly: SuppressMessage(
	"Security",
	"SCS0018:Path traversal: injection possible in {1} argument passed to '{0}'",
	Justification = "Assessed as safe as injection of a file write location is the method's intention.",
	Scope = "member",
	Target = "~M:LogicMonitor.Api.LogicMonitorClient.SerializeAndCompress``1(``0,System.String)~System.Byte[]")]

[assembly: SuppressMessage(
	"Security",
	"SCS0005:Weak random generator",
	Justification = "Not used for cryptography",
	Scope = "member",
	Target = "~M:LogicMonitor.Api.LogicMonitorClient.GetRestAlertsWithV84Bug(LogicMonitor.Api.Alerts.AlertFilter,System.TimeSpan)~System.Threading.Tasks.Task{System.Collections.Generic.List{LogicMonitor.Api.Alerts.Alert}}")]
