namespace LogicMonitor.Api.Test.Reports;

public class ReportTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllReportGroups()
	{
		var reportGroups = await LogicMonitorClient
			.GetAllAsync<ReportGroup>(CancellationToken);
		reportGroups.Should().NotBeNull();
		reportGroups.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetAllReports()
	{
		var reports = await LogicMonitorClient
			.GetAllAsync<ReportBase>(CancellationToken);
		reports.Should().NotBeNull();
		reports.Should().NotBeNullOrEmpty();
	}


	[Fact]
	public async Task RunReportById()
	{
		var response =
			await LogicMonitorClient.RunReportById(ReportId, CancellationToken);

		response.Should().NotBeNull();
		response.ResultUrl.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task CreateAndDeleteReport()
	{
		const string TemporaryReportName = "Temporary Report 5 - can delete";

		// Remove any existing temporary reports
		foreach (var existingReport in await LogicMonitorClient.GetAllAsync(new Filter<ReportBase>
		{
			FilterItems =
			[
				new Eq<ReportBase>(nameof(ReportBase.Name), TemporaryReportName)
			]
		}, CancellationToken)
		)
		{
			await LogicMonitorClient.DeleteAsync<ReportBase>(existingReport.Id, CancellationToken);
		}

		var sDate = DateTime.UtcNow.AddDays(-1);
		var eDate = sDate.AddHours(1);

		// Create it
		var report =
			await LogicMonitorClient.CreateAsync(new ReportCreationDto
			{
				Name = TemporaryReportName,
				Description = "Temporary Report - can delete",
				Format = "PDF",
				Type = "Dashboard",
				GroupId = 375,
				DateRange = $"{sDate:yyyy-MM-dd HH:mm} TO {eDate:yyyy-MM-dd HH:mm}",
				DashboardId = 205
			},
			CancellationToken);

		report.Should().NotBeNull();

		await LogicMonitorClient.DeleteAsync<ReportBase>(report.Id, CancellationToken);
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
		}, CancellationToken)
		)
		{
			await LogicMonitorClient
				.DeleteAsync(existingReportGroup, CancellationToken);
		}

		// Create it
		var reportGroup = await LogicMonitorClient.CreateAsync(new ReportGroupCreationDto
		{
			Name = "Test Name",
			Description = "Test Description"
		}, CancellationToken);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(reportGroup, CancellationToken);
	}
}
