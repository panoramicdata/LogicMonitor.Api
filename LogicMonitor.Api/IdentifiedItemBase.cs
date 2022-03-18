namespace LogicMonitor.Api;

/// <summary>
/// An entity with a LogicMonitor Id
/// </summary>
[DataContract]
public abstract class IdentifiedItemBase<T>
{
	/// <summary>
	/// LogicMonitor Id
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "id")]
	public T Id { get; set; }

	/// <summary>
	/// Equals override
	/// </summary>
	/// <param name="obj">The comparison object</param>
	/// <returns>True if equal</returns>
	public override bool Equals(object obj)
		=> obj is not null
			&& GetType() == obj.GetType()
			&& Id.Equals(((IdentifiedItemBase<T>)obj).Id);

	/// <summary>
	/// Hashcode override
	/// </summary>
	/// <returns>The hashcode</returns>
	public override int GetHashCode() => Id switch
	{
		int intId => intId * 23,
		string stringId => stringId.GetHashCode(),
		_ => throw new NotSupportedException($"Only int and string are supported for {nameof(IdentifiedItemBase<T>)}")
	};
}
