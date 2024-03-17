using System.Collections.ObjectModel;

namespace LogicMonitor.Api.Experimental;

internal class AdvancedFilter<T>(IList<FilterItem<T>> list) : ReadOnlyCollection<FilterItem<T>>(list) where T : IHasEndpoint, new()
{

	/// <inheritdoc/>
	public override string ToString() => string.Join(",", this.Select(x => x.ToString()));
}