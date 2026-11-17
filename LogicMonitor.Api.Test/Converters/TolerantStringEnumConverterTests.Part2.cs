using LogicMonitor.Api.Attributes;
using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Test.Converters;

public partial class TolerantStringEnumConverterTests
{

	[Fact]
	public void ReadJson_LogicModuleType_UnknownValue_InDebugThrowsNotImplementedException()
	{
		var json = """{"value": "UNKNOWN_TYPE"}""";

		var act = () => JsonConvert.DeserializeObject<TestContainer<LogicModuleType>>(json, SerializerSettings);

#if DEBUG
		act.Should().Throw<NotImplementedException>()
			.WithMessage("*missing an enum member*");
#else
		// Issue #33: this used to resolve to All - the value that means "every type" when querying -
		// so a type the client did not model was indistinguishable from a deliberate no-filter. The
		// converter already preferred a member named Unknown; LogicModuleType simply did not have one.
		var result = act();
		result.Should().NotBeNull();
		result!.Value.Should().Be(LogicModuleType.Unknown);
		result.Value.Should().NotBe(LogicModuleType.All);
#endif
	}

	[Fact]
	public void ReadJson_IntegrationMetadata_LogicModuleTypeSnmpSysOidMapAlias_DeserializesCorrectly()
	{
		// Reproduces the real DatamartClient payload shape at 'installationMetadata.logicModuleType'.
		// IntegrationMetadata.LogicModuleType must NOT carry its own [JsonConverter(typeof(StringEnumConverter))],
		// or that member-level attribute silently overrides the enum's TolerantStringEnumConverter.
		var json = """{"logicModuleType": "SNMP_SYSOID_MAP"}""";

		var result = JsonConvert.DeserializeObject<IntegrationMetadata>(json);

		result.Should().NotBeNull();
		result!.LogicModuleType.Should().Be(LogicModuleType.SnmpSysOIDMap);
	}

	[Fact]
	public void ReadJson_IntegrationMetadata_LogicModuleTypeSnmpSysOidMapPrimaryValue_DeserializesCorrectly()
	{
		// The LogicMonitor API has been observed returning BOTH "SNMP SysOID Map" and "SNMP_SYSOID_MAP"
		// for this same field across different endpoints/points in time (see commit history of
		// LogicModuleType.cs - the enum value flip-flopped between the two several times before the
		// alias mechanism was introduced). Both forms must deserialize cleanly through IntegrationMetadata.
		var json = """{"logicModuleType": "SNMP SysOID Map"}""";

		var result = JsonConvert.DeserializeObject<IntegrationMetadata>(json);

		result.Should().NotBeNull();
		result!.LogicModuleType.Should().Be(LogicModuleType.SnmpSysOIDMap);
	}

	[Fact]
	public void ReadJson_IntegrationMetadata_LogicModuleTypePrimaryValue_DeserializesCorrectly()
	{
		var json = """{"logicModuleType": "DATASOURCE"}""";

		var result = JsonConvert.DeserializeObject<IntegrationMetadata>(json);

		result.Should().NotBeNull();
		result!.LogicModuleType.Should().Be(LogicModuleType.DataSource);
	}

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
}
