namespace LogicMonitor.Api.Test.Settings;

public class SysOidsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAll()
	{
		var snmpSysOidMaps = await LogicMonitorClient.GetAllAsync<SnmpSysOidMap>(CancellationToken);
		snmpSysOidMaps.Should().NotBeNull();
		snmpSysOidMaps.Should().AllSatisfy(snmpSysOidMap => snmpSysOidMap.Oid.Should().NotBeNull());
		snmpSysOidMaps.Should().AllSatisfy(snmpSysOidMap => snmpSysOidMap.Categories.Should().NotBeNull());
	}
}
