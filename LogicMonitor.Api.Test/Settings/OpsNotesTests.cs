namespace LogicMonitor.Api.Test.Settings;

public class OpsNoteTests : TestWithOutput
{
	public OpsNoteTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetOpsNotes()
	{
		var allOpsNotes = await LogicMonitorClient.GetAllAsync<OpsNote>(default).ConfigureAwait(false);

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
		}, default).ConfigureAwait(false);

		// Text should be set
		allOpsNotesTags.Should().AllSatisfy(on => string.IsNullOrWhiteSpace(on.Name).Should().BeFalse());
	}
}
