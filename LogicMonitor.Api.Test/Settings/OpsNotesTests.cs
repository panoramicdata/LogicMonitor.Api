namespace LogicMonitor.Api.Test.Settings;

public class OpsNoteTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	[Fact]
	public async Task GetOpsNotes()
	{
		var allOpsNotes = await LogicMonitorClient
			.GetAllAsync<OpsNote>(default)
			.ConfigureAwait(true);

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
		}, default).ConfigureAwait(true);

		// Text should be set
		allOpsNotesTags.Should().AllSatisfy(on => on.Name.Should().NotBeNullOrWhiteSpace());
	}
}
