using System.Collections;
using System.Reflection;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Portal-free tests that every public model type in the library reads back what is written to it
/// and survives a JSON round trip unchanged.
/// </summary>
public partial class ModelAccessorTests
{

	private static IEnumerable<PropertyInfo> WritableProperties(Type type)
		=> type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(p => p.GetIndexParameters().Length == 0 && p.CanRead && p.SetMethod is { IsPublic: true });

	[Theory]
	[MemberData(nameof(ModelTypeNames))]
	public void WrittenProperties_ReadBackWhatWasWritten(string typeName)
	{
		var type = Resolve(typeName);
		var instance = Activator.CreateInstance(type)!;

		foreach (var property in WritableProperties(type))
		{
			if (!TryCreateSample(property.PropertyType, 0, out var sample))
			{
				continue;
			}

			property.SetValue(instance, sample);
			var readBack = property.GetValue(instance);

			if (sample is string or ValueType)
			{
				readBack.Should().Be(sample, $"{typeName}.{property.Name}");
			}
			else
			{
				readBack.Should().NotBeNull($"{typeName}.{property.Name}");
			}
		}
	}

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
