using LogicMonitor.Api.Attributes;
using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Test.Converters;

public partial class TolerantStringEnumConverterTests
{
	private static readonly JsonSerializerSettings SerializerSettings = new()
	{
		Converters = { new TolerantStringEnumConverter() }
	};

	[DataContract]
	private enum TestEnumWithUnknown
	{
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		[EnumMember(Value = "VALUE_ONE")]
		ValueOne,

		[EnumMember(Value = "VALUE_TWO")]
		ValueTwo
	}

	[DataContract]
	private enum TestEnumWithAll
	{
		[EnumMember(Value = "all")]
		All = 0,

		[EnumMember(Value = "OPTION_A")]
		OptionA,

		[EnumMember(Value = "OPTION_B")]
		OptionB
	}

	[DataContract]
	private enum TestEnumWithAlias
	{
		[EnumMember(Value = "all")]
		All = 0,

		[EnumMember(Value = "PRIMARY_VALUE")]
		[EnumMemberAlias("ALIAS_VALUE", "ANOTHER_ALIAS")]
		ValueWithAliases,

		[EnumMember(Value = "SIMPLE")]
		SimpleValue
	}

	private sealed class TestContainer<T>
	{
		[JsonProperty("value")]
		public T Value { get; set; } = default!;
	}

	[Fact]
	public void ReadJson_PrimaryEnumMemberValue_DeserializesCorrectly()
	{
		var json = """{"value": "VALUE_ONE"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<TestEnumWithUnknown>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithUnknown.ValueOne);
	}

	[Fact]
	public void ReadJson_AllAsPrimaryValue_DeserializesCorrectly()
	{
		var json = """{"value": "OPTION_A"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<TestEnumWithAll>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithAll.OptionA);
	}

	[Fact]
	public void ReadJson_PrimaryValueWithAliasAvailable_DeserializesCorrectly()
	{
		var json = """{"value": "PRIMARY_VALUE"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<TestEnumWithAlias>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithAlias.ValueWithAliases);
	}

	[Fact]
	public void ReadJson_FirstAliasValue_DeserializesCorrectly()
	{
		var json = """{"value": "ALIAS_VALUE"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<TestEnumWithAlias>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithAlias.ValueWithAliases);
	}

	[Fact]
	public void ReadJson_SecondAliasValue_DeserializesCorrectly()
	{
		var json = """{"value": "ANOTHER_ALIAS"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<TestEnumWithAlias>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithAlias.ValueWithAliases);
	}

	[Fact]
	public void ReadJson_UnknownStringValue_InDebugThrowsNotImplementedException()
	{
		var json = """{"value": "NONEXISTENT_VALUE"}""";

		var act = () => JsonConvert.DeserializeObject<TestContainer<TestEnumWithUnknown>>(json, SerializerSettings);

#if DEBUG
		act.Should().Throw<NotImplementedException>()
			.WithMessage("*missing an enum member*");
#else
		var result = act();
		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithUnknown.Unknown);
#endif
	}

	[Fact]
	public void ReadJson_UnknownStringValueWithAllDefault_InDebugThrowsNotImplementedException()
	{
		var json = """{"value": "NONEXISTENT_VALUE"}""";

		var act = () => JsonConvert.DeserializeObject<TestContainer<TestEnumWithAll>>(json, SerializerSettings);

#if DEBUG
		act.Should().Throw<NotImplementedException>()
			.WithMessage("*missing an enum member*");
#else
		var result = act();
		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithAll.All);
#endif
	}

	[Fact]
	public void ReadJson_ValidIntegerValue_DeserializesCorrectly()
	{
		var json = """{"value": 1}""";

		var result = JsonConvert.DeserializeObject<TestContainer<TestEnumWithUnknown>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithUnknown.ValueOne);
	}

	[Fact]
	public void ReadJson_InvalidIntegerValue_InDebugThrowsOrFallsBack()
	{
		var json = """{"value": 999}""";

		// Integer fallback doesn't have DEBUG behavior - it always falls back
		var result = JsonConvert.DeserializeObject<TestContainer<TestEnumWithUnknown>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithUnknown.Unknown);
	}

	[Fact]
	public void ReadJson_NullableWithValue_DeserializesCorrectly()
	{
		var json = """{"value": "VALUE_ONE"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<TestEnumWithUnknown?>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(TestEnumWithUnknown.ValueOne);
	}

	[Fact]
	public void ReadJson_NullableWithUnknownValue_ReturnsNull()
	{
		var json = """{"value": "NONEXISTENT"}""";

		// Nullable enums return null for unknown values (no DEBUG exception)
		var result = JsonConvert.DeserializeObject<TestContainer<TestEnumWithUnknown?>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().BeNull();
	}

	[Fact]
	public void ReadJson_LogicModuleType_SnmpSysOidMapPrimaryValue_DeserializesCorrectly()
	{
		var json = """{"value": "SNMP SysOID Map"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<LogicModuleType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(LogicModuleType.SnmpSysOIDMap);
	}

	[Fact]
	public void ReadJson_LogicModuleType_SnmpSysOidMapAliasValue_DeserializesCorrectly()
	{
		var json = """{"value": "SNMP_SYSOID_MAP"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<LogicModuleType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(LogicModuleType.SnmpSysOIDMap);
	}

	[Fact]
	public void ReadJson_LogicModuleType_DataSource_DeserializesCorrectly()
	{
		var json = """{"value": "DATASOURCE"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<LogicModuleType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(LogicModuleType.DataSource);
	}

	[Fact]
	public void ReadJson_LogicModuleType_DiagnosticSource_DeserializesCorrectly()
	{
		var json = """{"value": "DIAGNOSTICSOURCE"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<LogicModuleType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(LogicModuleType.DiagnosticSource);
	}


	[Fact]
	public void ReadJson_LogicModuleType_RemediationSource_DeserializesCorrectly()
	{
		// Issue #33: LogicMonitor sends REMEDIATIONSOURCE for the remote action modules - Kill Windows
		// Process, Restart Linux Device and similar. Confirmed against a live portal, where nine such
		// modules were silently landing on All. Note it is not ACTIONSOURCE, which is the name the
		// modules themselves suggest.
		var json = """{"value": "REMEDIATIONSOURCE"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<LogicModuleType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(LogicModuleType.RemediationSource);
	}
}
