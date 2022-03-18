namespace LogicMonitor.Api.Test.Settings;

public class SysOidsTests : TestWithOutput
{
	public SysOidsTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAll()
	{
		var snmpSysOidMaps = await LogicMonitorClient.GetAllAsync<SnmpSysOidMap>().ConfigureAwait(false);
		snmpSysOidMaps.Should().NotBeNull();
		Assert.All(snmpSysOidMaps, snmpSysOidMap => snmpSysOidMap.Oid.Should().NotBeNull());
		Assert.All(snmpSysOidMaps, snmpSysOidMap => snmpSysOidMap.Categories.Should().NotBeNull());
	}
}
