namespace LogicMonitor.Api.Test.Reports;

public class ReportTests : TestWithOutput
{
	public ReportTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllReportGroups()
	{
		var reportGroups = await LogicMonitorClient
			.GetAllAsync<ReportGroup>(default)
			.ConfigureAwait(false);
		reportGroups.Should().NotBeNull();
		reportGroups.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllReports()
	{
		var reports = await LogicMonitorClient
			.GetAllAsync<ReportBase>(default)
			.ConfigureAwait(false);
		reports.Should().NotBeNull();
		reports.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task CrudReportGroups()
	{
		// Delete it if it already exists
		foreach (var existingReportGroup in await LogicMonitorClient.GetAllAsync(new Filter<ReportGroup>
		{
			FilterItems = new List<FilterItem<ReportGroup>>
					{
						new Eq<ReportGroup>(nameof(ReportGroup.Name), "Test Name")
					}
		}, default).ConfigureAwait(false)
		)
		{
			await LogicMonitorClient.DeleteAsync(existingReportGroup, cancellationToken: default).ConfigureAwait(false);
		}

		// Create it
		var reportGroup = await LogicMonitorClient.CreateAsync(new ReportGroupCreationDto
		{
			Name = "Test Name",
			Description = "Test Description"
		}, default).ConfigureAwait(false);

		// Delete it again
		await LogicMonitorClient.DeleteAsync(reportGroup, cancellationToken: default).ConfigureAwait(false);
	}
}
