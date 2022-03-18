namespace LogicMonitor.Api.Test.OpsNotes;

/// <summary>
/// !!!!!!
/// Tests here may fail if the user has never used OpsNotes before.
/// !!!!!!
/// </summary>
public class OpsNotesTests : TestWithOutput
{
	public OpsNotesTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetOpsNotes()
	{
		// Create an ops note
		var newOpsNote = await LogicMonitorClient.CreateAsync(new OpsNoteCreationDto
		{
			DateTimeUtcSeconds = DateTime.UtcNow.SecondsSinceTheEpoch(),
			Note = $"LogicMonitor.Api.Test run on {DateTime.UtcNow}",
			Tags = new List<OpsNoteTagCreationDto> { new OpsNoteTagCreationDto { Name = "LogicMonitor.Api" } }
		})
		.ConfigureAwait(false);

		await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

		var allOpsNotes = await LogicMonitorClient.GetAllAsync<OpsNote>().ConfigureAwait(false);

		// Make sure that some are returned
		allOpsNotes.Should().NotBeNullOrEmpty();
		allOpsNotes.Select(o => o.Id).Should().Contain(newOpsNote.Id);
	}

	[Fact]
	public async void AddRemoveOpsNote()
	{
		var device = await LogicMonitorClient.GetAsync<Device>(WindowsDeviceId).ConfigureAwait(false);
		var theEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		var utcNow = (int)(DateTime.UtcNow - theEpoch).TotalSeconds;
		var opsNoteCreationDto = new OpsNoteCreationDto
		{
			Note = "Test&@!\"£$%^&*()_+-=",
			DateTimeUtcSeconds = utcNow,
			Scopes = new List<OpsNoteScopeCreationDto>
			{
				new DeviceOpsNoteScopeCreationDto {DeviceId = device.Id}
			}
		};
		var createdOpsNote = await LogicMonitorClient.CreateAsync(opsNoteCreationDto).ConfigureAwait(false);

		// Ensure that this OpsNote has an ID set
		createdOpsNote.Id.Should().NotBeNull();
		string.IsNullOrWhiteSpace(createdOpsNote.Id).Should().BeFalse();

		// Wait 2 seconds
		await Task.Delay(5000).ConfigureAwait(false);

		// Make sure the opsNote is now present when listing opsNotes and that all properties match
		var refetchedOpsNote = await LogicMonitorClient.GetAsync<OpsNote>(createdOpsNote.Id).ConfigureAwait(false);
		refetchedOpsNote.Should().NotBeNull();
		refetchedOpsNote.Note.Should().Be(createdOpsNote.Note);
		refetchedOpsNote.HappenOnUtc.SecondsSinceTheEpoch().Should().Be(utcNow);
		//refetchedOpsNote.Tags.Select(t => t.Name).Should().Be(createdOpsNote.Tags.Select(t => t.Name));

		// Remove the test OpsNote - this takes some time
		await LogicMonitorClient.DeleteAsync<OpsNote>(createdOpsNote.Id).ConfigureAwait(false);

		// Wait 2 seconds
		await Task.Delay(2000).ConfigureAwait(false);

		// Make sure that it is gone
		refetchedOpsNote = await LogicMonitorClient.GetAsync<OpsNote>(createdOpsNote.Id).ConfigureAwait(false);
		refetchedOpsNote.Should().BeNull();
	}
}