namespace LogicMonitor.Api.Test.Settings;

public class OpsNoteTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetOpsNotes()
	{
		var allOpsNotes = await LogicMonitorClient
			.GetAllAsync<OpsNote>(CancellationToken);

		allOpsNotes.Should().NotBeNull();
	}

	[Fact]
	public async Task GetOpsNotesTags()
	{
		var allOpsNotesTags = await LogicMonitorClient.GetAllAsync(new Filter<OpsNoteTag>
		{
			Order = new Order<OpsNoteTag>
			{
				Direction = OrderDirection.Asc,
				Property = nameof(OpsNoteTag.Name)
			}
		}, CancellationToken);

		// Text should be set
		allOpsNotesTags.Should().AllSatisfy(on => on.Name.Should().NotBeNullOrWhiteSpace());
	}
}
