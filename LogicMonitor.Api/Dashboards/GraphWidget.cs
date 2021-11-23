using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A graph widget
/// </summary>
[DataContract]
public abstract class GraphWidget : Widget
{
}
