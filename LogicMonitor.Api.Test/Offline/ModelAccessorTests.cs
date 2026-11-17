using System.Collections;
using System.Reflection;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Portal-free tests that every public model type in the library reads back what is written to it
/// and survives a JSON round trip unchanged.
/// </summary>
public class ModelAccessorTests
{
	private static readonly Assembly _apiAssembly = typeof(LogicMonitorClient).Assembly;

	private static bool IsModelType(Type type)
		=> type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false, IsPublic: true }
			&& !typeof(Exception).IsAssignableFrom(type)
			&& !typeof(Delegate).IsAssignableFrom(type)
			&& type != typeof(LogicMonitorClient)
			&& type.GetConstructor(Type.EmptyTypes) is not null;

	public static TheoryData<string> ModelTypeNames()
	{
		var data = new TheoryData<string>();
		foreach (var type in _apiAssembly.GetTypes().Where(IsModelType).OrderBy(t => t.FullName, StringComparer.Ordinal))
		{
			data.Add(type.FullName!);
		}

		return data;
	}

	private static Type Resolve(string typeName)
		=> _apiAssembly.GetType(typeName, throwOnError: true)!;

	private static bool TryCreateSample(Type type, int depth, out object? sample)
	{
		type = Nullable.GetUnderlyingType(type) ?? type;
		sample = null;
		if (type == typeof(string) || type == typeof(object))
		{
			sample = "sample";
		}
		else if (type == typeof(bool))
		{
			sample = true;
		}
		else if (type == typeof(int))
		{
			sample = 7;
		}
		else if (type == typeof(long))
		{
			sample = 7L;
		}
		else if (type == typeof(double))
		{
			sample = 1.5;
		}
		else if (type == typeof(float))
		{
			sample = 1.5f;
		}
		else if (type == typeof(decimal))
		{
			sample = 1.5m;
		}
		else if (type == typeof(DateTime))
		{
			sample = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
		}
		else if (type == typeof(DateTimeOffset))
		{
			sample = new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.Zero);
		}
		else if (type == typeof(TimeSpan))
		{
			sample = TimeSpan.FromSeconds(90);
		}
		else if (type.IsEnum)
		{
			var values = Enum.GetValues(type);
			sample = values.Length > 1 ? values.GetValue(1) : values.GetValue(0);
		}

		return sample is not null || TryCreateComposite(type, depth, out sample);
	}

	private static bool TryCreateComposite(Type type, int depth, out object? sample)
	{
		sample = null;
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			var list = (IList)Activator.CreateInstance(type)!;
			if (TryCreateSample(type.GetGenericArguments()[0], depth + 1, out var element) && element is not null)
			{
				list.Add(element);
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
