namespace LogicMonitor.Api.Test.Attributes;

public class IsPropertyReadonlyTests
{
	/*
	 * Note that the following Unit Tests depend on the configuration in the classes, so if the read only state of a property changes, the tests will need updating
	 */

	[Theory]
	[InlineData("AlertingDisabledOn", typeof(Resource))]
	[InlineData("AlertStatus", typeof(Resource))]
	[InlineData("Acked", typeof(Collector))]
	public void IsPropertyReadonlyTest_ShouldReturnTrue(string propertyName, Type classType)
		=> LogicMonitorClient.IsPropertyReadOnly(propertyName, classType).Should().BeTrue();

	[Theory]
	[InlineData("AlertStatusPriority", typeof(Resource))]
	[InlineData("CanUseRemoteSession", typeof(Resource))]
	[InlineData("AckComment", typeof(Collector))]
	public void IsPropertyReadonlyTest_ShouldReturnFalse(string propertyName, Type classType)
		=> LogicMonitorClient.IsPropertyReadOnly(propertyName, classType).Should().BeFalse();

	[Theory]
	[InlineData("lastRawDataTime", typeof(Resource))]
	[InlineData("lastDataTime", typeof(Resource))]
	[InlineData("ackedOn", typeof(Collector))]
	public void IsPropertyReadonlyTest_PreferJsonName_ShouldReturnTrue(string propertyName, Type classType)
		=> LogicMonitorClient.IsPropertyReadOnly(propertyName, classType, true).Should().BeTrue();

	[Theory]
	[InlineData("customProperties", typeof(Resource))]
	[InlineData("canUseRemoteSession", typeof(Resource))]
	[InlineData("backupAgentId", typeof(Collector))]
	public void IsPropertyReadonlyTest_PreferJsonName_ShouldReturnFalse(string propertyName, Type classType)
		=> LogicMonitorClient.IsPropertyReadOnly(propertyName, classType, true).Should().BeFalse();

	[Fact]
	public void IsPropertyReadonlyTest_NonExistentProperty_ShouldThrowException()
	{
		var exception = Record.Exception(() => LogicMonitorClient.IsPropertyReadOnly("xxx", typeof(Resource)).Should().BeFalse());
		exception.Should().NotBeNull();
		exception.Should().BeOfType<PropertyNotFoundException>();
		exception.Message.Should().Be("Could not find property on Resource with name xxx.");
	}
}
