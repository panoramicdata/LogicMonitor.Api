namespace LogicMonitor.Api.Data;

/// <summary>
///    A Base class for all GraphDataRequests
/// </summary>
public abstract class GraphDataRequestBase : IValidate
{
	/// <inheritdoc />
	public abstract void Validate();
}
