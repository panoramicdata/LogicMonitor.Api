namespace LogicMonitor.Api.Test.Reports;

public class ReportTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetAllReportGroups()
	{
		var reportGroups = await LogicMonitorClient
			.GetAllAsync<ReportGroup>(default)
			.ConfigureAwait(true);
		reportGroups.Should().NotBeNull();
		reportGroups.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllReports()
	{
		var reports = await LogicMonitorClient
			.GetAllAsync<ReportBase>(default)
			.ConfigureAwait(true);
		reports.Should().NotBeNull();
		reports.Should().NotBeNullOrEmpty();
	}


	[Fact]
	public async Task RunReportById()
	{
		var response =
			await LogicMonitorClient.RunReportById(ReportId, default)
			.ConfigureAwait(true);

		response.Should().NotBeNull();
		response.ResultUrl.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task CreateAndDeleteReport()
	{
		var sDate = DateTime.UtcNow.AddDays(-1);
		var eDate = sDate.AddHours(1);

		// Create it
		var report =
			await LogicMonitorClient.CreateAsync(new ReportCreationDto
			{
				Name = "Temporary Report 5 - can delete",
				Description = "Temporary Report - can delete",
				Format = "PDF",
				Type = "Dashboard",
				GroupId = 375,
				DateRange = $"{sDate.ToString("yyyy-MM-dd HH:mm")} TO {eDate.ToString("yyyy-MM-dd HH:mm")}",
				DashboardId = 205
			},
			default)
			.ConfigureAwait(true);

		report.Should().NotBeNull();

		await LogicMonitorClient.DeleteAsync<ReportBase>(report.Id, default).ConfigureAwait(true);
	}

	[Fact]
	public async Task CrudReportGroups()
	{
		// Delete it if it already exists
		foreach (var existingReportGroup in await LogicMonitorClient.GetAllAsync(new Filter<ReportGroup>
		{
			FilterItems =
			[
				new Eq<ReportGroup>(nameof(ReportGroup.Name), "Test Name")
			]
		}, default).ConfigureAwait(true)
		)
		{
			await LogicMonitorClient
				.DeleteAsync(existingReportGroup, cancellationToken: default)
				.ConfigureAwait(true);
		}

		// Create it
		var reportGroup = await LogicMonitorClient.CreateAsync(new ReportGroupCreationDto
		{
			Name = "Test Name",
			Description = "Test Description"
		}, default).ConfigureAwait(true);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(reportGroup, cancellationToken: default)
			.ConfigureAwait(true);
	}
}
