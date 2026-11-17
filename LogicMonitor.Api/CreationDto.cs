namespace LogicMonitor.Api;

/// <summary>
///    A base class from which all CreationDtos must derive
/// </summary>
/// <typeparam name="T">the type of the created item</typeparam>
public abstract class CreationDto<T>
{
	internal static Type CreatedItemType => typeof(T);
}
