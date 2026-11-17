using System.Collections;
using System.Reflection;

namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Portal-free tests that every public model type in the library reads back what is written to it
/// and survives a JSON round trip unchanged.
/// </summary>
public partial class ModelAccessorTests
{

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

	private static readonly Dictionary<Type, object> _simpleSamples = new()
	{
		[typeof(string)] = "sample",
		[typeof(object)] = "sample",
		[typeof(bool)] = true,
		[typeof(int)] = 7,
		[typeof(long)] = 7L,
		[typeof(double)] = 1.5,
		[typeof(float)] = 1.5f,
		[typeof(decimal)] = 1.5m,
		[typeof(DateTime)] = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc),
		[typeof(DateTimeOffset)] = new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.Zero),
		[typeof(TimeSpan)] = TimeSpan.FromSeconds(90)
	};

	private static bool TryCreateSample(Type type, int depth, out object? sample)
	{
		type = Nullable.GetUnderlyingType(type) ?? type;
		if (_simpleSamples.TryGetValue(type, out sample))
		{
			return true;
		}

		if (type.IsEnum)
		{
			var values = Enum.GetValues(type);
			sample = values.GetValue(values.Length > 1 ? 1 : 0);
			return true;
		}

		return TryCreateComposite(type, depth, out sample);
	}
}
