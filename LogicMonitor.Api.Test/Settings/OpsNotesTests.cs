namespace LogicMonitor.Api.Test.Settings;

public class OpsNoteTests : TestWithOutput
{
	public OpsNoteTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetOpsNotes()
	{
		var allOpsNotes = await LogicMonitorClient.GetAllAsync<OpsNote>().ConfigureAwait(false);

		Assert.NotNull(allOpsNotes);
	}

	[Fact]
	public async void GetOpsNotesTags()
	{
		var allOpsNotesTags = await LogicMonitorClient.GetAllAsync(new Filter<OpsNoteTag>
		{
			Order = new Order<OpsNoteTag>
			{
				Direction = OrderDirection.Asc,
				Property = nameof(OpsNoteTag.Name)
			}
		}).ConfigureAwait(false);

		// Text should be set
		Assert.All(allOpsNotesTags, on => Assert.False(string.IsNullOrWhiteSpace(on.Name)));
	}
}
