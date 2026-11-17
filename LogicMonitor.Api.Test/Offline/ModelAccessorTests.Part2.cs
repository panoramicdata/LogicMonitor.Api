using System.Collections;
using System.Reflection;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Portal-free tests that every public model type in the library reads back what is written to it
/// and survives a JSON round trip unchanged.
/// </summary>
public partial class ModelAccessorTests
{

	private static bool TryCreateComposite(Type type, int depth, out object? sample)
	{
		sample = null;
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			var list = Activator.CreateInstance(type)!;
			if (TryCreateSample(type.GetGenericArguments()[0], depth + 1, out var element) && element is not null)
			{
				type.GetMethod(nameof(List<object>.Add))!.Invoke(list, [element]);
			}

			sample = list;
			return true;
		}

		if (depth < 2 && type is { IsClass: true, IsAbstract: false, IsGenericType: false } && type.GetConstructor(Type.EmptyTypes) is not null)
		{
			sample = Activator.CreateInstance(type);
			return true;
		}

		return false;
	}

	private static IEnumerable<PropertyInfo> WritableProperties(Type type)
		=> type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(p => p.GetIndexParameters().Length == 0 && p.CanRead && p.SetMethod is { IsPublic: true });

	// These getters parse or index into a value that is only valid once the caller has set it.
	private static bool ThrowsOnDefaultInstance(PropertyInfo property)
		=> (property.Name == "DashboardId" && property.DeclaringType!.Name.StartsWith("WidgetCreationDto", StringComparison.Ordinal))
			|| (property.Name == "Color" && property.DeclaringType == typeof(Line));

	[Theory]
	[MemberData(nameof(ModelTypeNames))]
	public void DefaultInstances_CanBeReadWithoutThrowing(string typeName)
	{
		var type = Resolve(typeName);
		var instance = Activator.CreateInstance(type)!;

		var act = () =>
		{
			foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetIndexParameters().Length == 0 && p.CanRead && !ThrowsOnDefaultInstance(p)))
			{
				_ = property.GetValue(instance);
			}

			_ = instance.ToString();
		};

		act.Should().NotThrow();
	}
}
