namespace LogicMonitor.Api.Test.Settings;

public class SysOidsTests : TestWithOutput
{
	public SysOidsTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAll()
	{
		var snmpSysOidMaps = await LogicMonitorClient.GetAllAsync<SnmpSysOidMap>(CancellationToken.None).ConfigureAwait(false);
		snmpSysOidMaps.Should().NotBeNull();
		snmpSysOidMaps.Should().AllSatisfy(snmpSysOidMap => snmpSysOidMap.Oid.Should().NotBeNull());
		snmpSysOidMaps.Should().AllSatisfy(snmpSysOidMap => snmpSysOidMap.Categories.Should().NotBeNull());
	}
}
