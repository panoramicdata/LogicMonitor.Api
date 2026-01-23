using LogicMonitor.Api.Attributes;
using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Test.Converters;

public class TolerantStringEnumConverterTests
{
	private static readonly JsonSerializerSettings SerializerSettings = new()
	{
		Converters = { new TolerantStringEnumConverter() }
	};

	#region Test Enums

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

	#endregion

	#region Primary EnumMember Value Tests

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

	#endregion

	#region EnumMemberAlias Tests

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

	#endregion

	#region Fallback Tests (DEBUG mode throws to alert developers of missing enum members)

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

	#endregion

	#region Integer Value Tests

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

	#endregion

	#region Nullable Tests

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

	#endregion

	#region Real-World LogicModuleType Tests

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
	public void ReadJson_LogicModuleType_UnknownValue_InDebugThrowsNotImplementedException()
	{
		var json = """{"value": "UNKNOWN_TYPE"}""";

		var act = () => JsonConvert.DeserializeObject<TestContainer<LogicModuleType>>(json, SerializerSettings);

#if DEBUG
		act.Should().Throw<NotImplementedException>()
			.WithMessage("*missing an enum member*");
#else
		var result = act();
		result.Should().NotBeNull();
		result!.Value.Should().Be(LogicModuleType.All);
#endif
	}

	#endregion

	#region CollectionMethod Tests

	[Fact]
	public void ReadJson_CollectionMethod_KnownValue_DeserializesCorrectly()
	{
		var json = """{"value": "snmp"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<CollectionMethod>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(CollectionMethod.Snmp);
	}

	[Fact]
	public void ReadJson_CollectionMethod_AwsCloudWatch_DeserializesCorrectly()
	{
		var json = """{"value": "awscloudwatch"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<CollectionMethod>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(CollectionMethod.AwsCloudWatch);
	}

	[Fact]
	public void ReadJson_CollectionMethod_IntegerValue_DeserializesCorrectly()
	{
		var json = """{"value": 71}""";

		var result = JsonConvert.DeserializeObject<TestContainer<CollectionMethod>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(CollectionMethod.Snmp);
	}

	#endregion

	#region ZoomPlanUsageType Tests

	[Fact]
	public void ReadJson_ZoomPlanUsageType_PlanBase_DeserializesCorrectly()
	{
		var json = """{"value": "plan_base"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<ZoomPlanUsageType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(ZoomPlanUsageType.PlanBase);
	}

	[Fact]
	public void ReadJson_ZoomPlanUsageType_PlanWebinar_DeserializesCorrectly()
	{
		var json = """{"value": "plan_webinar"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<ZoomPlanUsageType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(ZoomPlanUsageType.PlanWebinar);
	}

	#endregion

	#region ResourceGroupType Tests

	[Fact]
	public void ReadJson_ResourceGroupType_Normal_DeserializesCorrectly()
	{
		var json = """{"value": "Normal"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<ResourceGroupType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(ResourceGroupType.Normal);
	}

	[Fact]
	public void ReadJson_ResourceGroupType_IntegerAsString_DeserializesCorrectly()
	{
		// ResourceGroupType uses string values like "1" for some members
		var json = """{"value": "1"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<ResourceGroupType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(ResourceGroupType.Dynamic);
	}

	[Fact]
	public void ReadJson_ResourceGroupType_AwsStepFunctions_DeserializesCorrectly()
	{
		var json = """{"value": "AWS/StepFunctions"}""";

		var result = JsonConvert.DeserializeObject<TestContainer<ResourceGroupType>>(json, SerializerSettings);

		result.Should().NotBeNull();
		result!.Value.Should().Be(ResourceGroupType.AwsStepFunctions);
	}

	#endregion
}
