namespace LogicMonitor.Api.Test.Settings;

public class SysOidsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAll()
	{
		var snmpSysOidMaps = await LogicMonitorClient.GetAllAsync<SnmpSysOidMap>(default);
		snmpSysOidMaps.Should().NotBeNull();
		snmpSysOidMaps.Should().AllSatisfy(snmpSysOidMap => snmpSysOidMap.Oid.Should().NotBeNull());
		snmpSysOidMaps.Should().AllSatisfy(snmpSysOidMap => snmpSysOidMap.Categories.Should().NotBeNull());
	}
}
