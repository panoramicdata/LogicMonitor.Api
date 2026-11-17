using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Resources;

public partial class ResourceGroupTests
{

	[Fact]
	public async Task GetResourceGroupsByParentId()
	{
		var resourceGroup = await LogicMonitorClient
			.GetAsync<ResourceGroup>(1, CancellationToken);

		// Make sure that some are returned
		resourceGroup.SubGroups.Should().NotBeNullOrEmpty();

		// Make sure that all children have Unique Ids
		resourceGroup.SubGroups.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	/// <summary>
	/// Get a ResourceGroup that has a + character in the name (must be escaped)
	/// </summary>
	[Fact]
	public async Task GetResourceGroupByFullPathWithPlusInName()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(ResourceGroupFullPath, CancellationToken) ?? throw new InvalidOperationException();

		resourceGroup.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAwsExternalId()
	{
		var awsId = await LogicMonitorClient
			.GetExternalIdAsync(CancellationToken);
		awsId.Should().NotBeNull();
	}

	[Fact]
	public async Task GetResourceGroupSDTs()
	{
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(ResourceGroupFullPath, CancellationToken) ?? throw new InvalidOperationException();
		var sdts = await LogicMonitorClient
			.GetResourceGroupSdtsAsync(resourceGroup.Id, new Filter<ScheduledDownTime>(), CancellationToken);
		sdts.Should().NotBeNull();
	}

	[Fact]
	public async Task GetResourceGroupAlerts()
	{
		var alerts = await LogicMonitorClient
			.GetResourceGroupAlertsAsync(1, cancellationToken: CancellationToken);
		alerts.Items.Should().NotBeEmpty();
	}
}
