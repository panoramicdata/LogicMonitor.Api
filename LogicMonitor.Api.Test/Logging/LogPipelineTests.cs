namespace LogicMonitor.Api.Test.Logging;

public class LogPipelineTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllLogPipelines_Succeeds()
	{
		var pipelines = await LogicMonitorClient.GetAllAsync<LogPipeline>(CancellationToken);

		pipelines.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllLogPipelineProcessors_Succeeds()
	{
		var processors = await LogicMonitorClient.GetAllAsync<LogPipelineProcessor>(CancellationToken);

		processors.Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllLogQueryGroups_Succeeds()
	{
		var groups = await LogicMonitorClient.GetAllAsync<LogQueryGroup>(CancellationToken);

		groups.Should().NotBeNullOrEmpty();

		var queries = await LogicMonitorClient.GetLogQueriesByGroupIdAsync(groups[0].Id, CancellationToken);
		queries.Items.Should().NotBeNull();
	}

	[Fact]
	public async Task GetLogQueryGroupsByGroupType_Succeeds()
	{
		var page = await LogicMonitorClient.GetLogQueryGroupsByGroupTypeAsync("PRIVATE", false, CancellationToken);

		page.Items.Should().NotBeNull();
		page.Items.Should().AllSatisfy(g => g.GroupType.Should().Be("PRIVATE"));
	}

	[Fact]
	public async Task LogQueryGroup_CreateGetDelete_Succeeds()
	{
		var name = $"LMApiTest_{Guid.NewGuid():N}"[..18];
		var created = await LogicMonitorClient.CreateAsync(
			new LogQueryGroupCreationDto { Name = name, Description = "Created by LogicMonitor.Api.Test", GroupType = "PRIVATE" },
			CancellationToken);
		try
		{
			var fetched = await LogicMonitorClient.GetAsync<LogQueryGroup>(created.Id, CancellationToken);
			fetched.Name.Should().Be(name);
		}
		finally
		{
			await LogicMonitorClient.DeleteAsync(created, CancellationToken);
		}
	}

	// A 401 here would mean a raw JSON request body was signed wrongly
	[Fact]
	public async Task MoveLogQueries_ToMissingGroup_IsAuthenticatedAndRejected()
	{
		var act = () => LogicMonitorClient.MoveLogQueriesAsync(int.MaxValue, [], CancellationToken);

		var exception = (await act.Should().ThrowAsync<LogicMonitorApiException>()).Which;
		TestOutputHelper.WriteLine($"{exception.HttpStatusCode}: {exception.Message}");
		exception.HttpStatusCode.Should().NotBe(System.Net.HttpStatusCode.Unauthorized);
	}

	[Fact]
	public async Task GetAllTrackedQueryGroups_Succeeds()
	{
		var groups = await LogicMonitorClient.GetAllAsync<TrackedQueryGroup>(CancellationToken);

		groups.Should().NotBeNull();
	}

	[Fact]
	public async Task TrackedQueryGroup_CreateGetDelete_Succeeds()
	{
		var name = $"LMApiTest_{Guid.NewGuid():N}"[..18];
		var created = await LogicMonitorClient.CreateAsync(
			new TrackedQueryGroupCreationDto { Name = name, Description = "Created by LogicMonitor.Api.Test" },
			CancellationToken);
		try
		{
			var fetched = await LogicMonitorClient.GetAsync<TrackedQueryGroup>(created.Id, CancellationToken);
			fetched.Name.Should().Be(name);
		}
		finally
		{
			await LogicMonitorClient.DeleteAsync(created, CancellationToken);
		}
	}
}
