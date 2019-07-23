using LogicMonitor.Api.Dashboards;
using System.Collections.Generic;

namespace LogicMonitor.Api
{
	/// <summary>
	/// An abstract clone request
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class CloneRequest<T> where T : ICloneableItem
	{
	}
}