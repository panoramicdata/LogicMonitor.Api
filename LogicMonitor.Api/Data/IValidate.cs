namespace LogicMonitor.Api.Data;

/// <summary>
/// Validation interface
/// </summary>
public interface IValidate
{
	/// <summary>
	/// Throws an exception if invalid
	/// </summary>
	void Validate();
}
