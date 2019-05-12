using LogicMonitor.Api.Filters;
using LogicMonitor.Api.OpsNotes;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class OpsNoteTests : TestWithOutput
	{
		public OpsNoteTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetOpsNotes()
		{
			var allOpsNotes = await PortalClient.GetAllAsync<OpsNote>().ConfigureAwait(false);

			Assert.NotNull(allOpsNotes);
		}

		[Fact]
		public async void GetOpsNotesTags()
		{
			var allOpsNotesTags = await PortalClient.GetAllAsync(new Filter<OpsNoteTag>
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
}