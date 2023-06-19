namespace LogicMonitor.Api;

/// <summary>
///    LogicMonitor undescribed named item
/// </summary>
[DataContract]
public abstract class UndescribedNamedItem : IdentifiedItem
{
	/// <summary>
	///    The LogicMonitor Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    Equals override
	/// </summary>
	/// <param name="obj"></param>
	public override bool Equals(object obj)
	{
		if (obj is null || obj.GetType() != GetType())
		{
			return false;
		}

		var logicMonitorNamedEntity = obj as NamedItem;

		if (logicMonitorNamedEntity is null)
		{
			return false;
		}

		return base.Equals(logicMonitorNamedEntity)
			   && logicMonitorNamedEntity is not null
			   && logicMonitorNamedEntity.Name == Name;
	}

	/// <summary>
	///    Get Hashcode override
	/// </summary>
	public override int GetHashCode() => base.GetHashCode()
			   + (Name ?? string.Empty).GetHashCode();

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>'Name (Id)'</returns>
	public override string ToString() => $"{Name} ({Id})";
}
