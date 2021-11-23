namespace LogicMonitor.Api.Test;

public class IsPropertyReadonlyTests
{
	/*
	 * Note that the following Unit Tests depend on the configuration in the classes, so if the read only state of a property changes, the tests will need updating
	 */

	[Theory]
	[InlineData("AlertingDisabledOn", typeof(Device))]
	[InlineData("AlertStatus", typeof(Device))]
	[InlineData("Acked", typeof(Collector))]
	public void IsPropertyReadonlyTest_ShouldReturnTrue(string propertyName, Type classType)
		=> Assert.True(LogicMonitorClient.IsPropertyReadOnly(propertyName, classType));

	[Theory]
	[InlineData("AlertStatusPriority", typeof(Device))]
	[InlineData("CanUseRemoteSession", typeof(Device))]
	[InlineData("AckComment", typeof(Collector))]
	public void IsPropertyReadonlyTest_ShouldReturnFalse(string propertyName, Type classType)
		=> Assert.False(LogicMonitorClient.IsPropertyReadOnly(propertyName, classType));

	[Theory]
	[InlineData("lastRawDataTime", typeof(Device))]
	[InlineData("lastDataTime", typeof(Device))]
	[InlineData("ackedOn", typeof(Collector))]
	public void IsPropertyReadonlyTest_PreferJsonName_ShouldReturnTrue(string propertyName, Type classType)
		=> Assert.True(LogicMonitorClient.IsPropertyReadOnly(propertyName, classType, true));

	[Theory]
	[InlineData("customProperties", typeof(Device))]
	[InlineData("canUseRemoteSession", typeof(Device))]
	[InlineData("backupAgentId", typeof(Collector))]
	public void IsPropertyReadonlyTest_PreferJsonName_ShouldReturnFalse(string propertyName, Type classType)
		=> Assert.False(LogicMonitorClient.IsPropertyReadOnly(propertyName, classType, true));

	[Fact]
	public void IsPropertyReadonlyTest_NonExistentProperty_ShouldThrowException()
	{
		var exception = Record.Exception(() => Assert.False(LogicMonitorClient.IsPropertyReadOnly("xxx", typeof(Device))));
		Assert.NotNull(exception);
		Assert.IsType<PropertyNotFoundException>(exception);
		Assert.Equal("Could not find property on Device with name xxx.", exception.Message);
	}
}
