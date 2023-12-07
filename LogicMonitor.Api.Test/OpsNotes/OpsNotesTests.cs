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
	public async Task GetOpsNotes()
	{
		// Create an ops note
		var newOpsNote = await LogicMonitorClient.CreateAsync(new OpsNoteCreationDto
		{
			DateTimeUtcSeconds = DateTime.UtcNow.SecondsSinceTheEpoch(),
			Note = $"LogicMonitor.Api.Test run on {DateTime.UtcNow}",
			Tags = [new OpsNoteTagCreationDto { Name = "LogicMonitor.Api" }]
		}, default)
		.ConfigureAwait(false);

		await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

		var allOpsNotes = await LogicMonitorClient
			.GetAllAsync<OpsNote>(default)
			.ConfigureAwait(false);

		// Make sure that some are returned
		allOpsNotes.Should().NotBeNullOrEmpty();
		allOpsNotes.Select(o => o.Id).Should().Contain(newOpsNote.Id);
	}

	[Theory]
	[InlineData(typeof(DeviceOpsNoteScopeCreationDto))]
	[InlineData(typeof(WebsiteOpsNoteScopeCreationDto))]
	[InlineData(typeof(WebsiteGroupOpsNoteScopeCreationDto))]
	[InlineData(typeof(DeviceGroupOpsNoteScopeCreationDto))]
	public async Task AddRemoveOpsNote(Type t)
	{
		if (t is null)
		{
			throw new ArgumentNullException(nameof(t));
		}

		var device = await LogicMonitorClient
			.GetAsync<Device>(WindowsDeviceId, default)
			.ConfigureAwait(false);

		var website = await LogicMonitorClient
			.GetByNameAsync<Website>(WebsiteName, default)
			.ConfigureAwait(false);

		website ??= new();

		var theEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		var utcNow = (int)(DateTime.UtcNow - theEpoch).TotalSeconds;
		var opsNoteCreationDto = new OpsNoteCreationDto
		{
			Note = "Test&@!\"£$%^&*()_+-=",
			DateTimeUtcSeconds = utcNow,
			Scopes =
			[
				t.Name switch
				{
					nameof(DeviceOpsNoteScopeCreationDto) => new DeviceOpsNoteScopeCreationDto {DeviceId = device.Id},
					nameof(DeviceGroupOpsNoteScopeCreationDto) => new DeviceGroupOpsNoteScopeCreationDto {DeviceGroupId = device.DeviceGroupIdsString.Split(',').Select(int.Parse).First()},
					nameof(WebsiteOpsNoteScopeCreationDto) => new WebsiteOpsNoteScopeCreationDto {WebsitesId = website.Id},
					nameof(WebsiteGroupOpsNoteScopeCreationDto) => new WebsiteGroupOpsNoteScopeCreationDto {WebsiteGroupId = website.GroupId},
					_ => throw new NotSupportedException($"Unexpected type {t.Name}")
				}
			]
		};
		var createdOpsNote = await LogicMonitorClient.CreateAsync(opsNoteCreationDto, default).ConfigureAwait(false);

		// Ensure that this OpsNote has an ID set
		createdOpsNote.Id.Should().NotBeNull();
		string.IsNullOrWhiteSpace(createdOpsNote.Id).Should().BeFalse();

		// Wait 2 seconds
		await Task.Delay(5000).ConfigureAwait(false);

		// Make sure the opsNote is now present when listing opsNotes and that all properties match
		var refetchedOpsNote = await LogicMonitorClient.GetAsync<OpsNote>(createdOpsNote.Id, default).ConfigureAwait(false);
		refetchedOpsNote.Should().NotBeNull();
		refetchedOpsNote.Note.Should().Be(createdOpsNote.Note);
		refetchedOpsNote.HappenOnUtc.SecondsSinceTheEpoch().Should().Be(utcNow);
		refetchedOpsNote.Tags.Select(t => t.Name).Should().Equal(createdOpsNote.Tags.Select(t => t.Name));

		// Remove the test OpsNote - this takes some time
		await LogicMonitorClient.DeleteAsync<OpsNote>(createdOpsNote.Id, cancellationToken: default).ConfigureAwait(false);

		// Wait 2 seconds
		await Task.Delay(2000).ConfigureAwait(false);

		// Make sure that it is gone
		var operation = async () => await LogicMonitorClient.GetAsync<OpsNote>(createdOpsNote.Id, cancellationToken: default).ConfigureAwait(false);
		await operation
			.Should()
			.ThrowAsync<LogicMonitorApiException>()
			.ConfigureAwait(false);
	}
}