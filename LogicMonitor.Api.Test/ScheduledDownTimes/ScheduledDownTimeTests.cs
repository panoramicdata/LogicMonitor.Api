namespace LogicMonitor.Api.Test.ScheduledDownTimes;

public class ScheduledDownTimeTests : TestWithOutput
{
	public ScheduledDownTimeTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetDeviceScheduledDownTimes()
	{
		var sdts = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceSDT"),
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.IsEffective), false),
				}
		}).ConfigureAwait(false);
		Assert.All(sdts, sdt => Assert.False(sdt.IsEffective));
	}

	[Fact]
	public async void GetWebsiteGroupScheduledDownTimes_UsingSubUrl_Succeeds()
	{
		// Ensure that a website group SDT exists with an unusual comment
		var commentGuid = Guid.NewGuid();
		var initialComment = $"ABC {commentGuid} DEF";
		var websiteGroupId = 1;
		var sdtCreationDto = new WebsiteGroupScheduledDownTimeCreationDto(1)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddDays(7).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient
			.CreateAsync(sdtCreationDto)
			.ConfigureAwait(false);
		createdSdt.Comment.Should().Be(initialComment);
		createdSdt.WebsiteGroupId.Should().Be(websiteGroupId);

		var subUrl = $"sdt/sdts?filter=type:\"WebsiteGroupSDT\",comment~\"{commentGuid}\"";

		var scheduledDownTimes = await LogicMonitorClient
			.GetAllAsync<ScheduledDownTime>(subUrl)
			.ConfigureAwait(false);
		scheduledDownTimes.Should().AllSatisfy(sdt =>
		{
			sdt.Comment.Should().Contain(commentGuid.ToString());
			sdt.Type.Should().Be(ScheduledDownTimeType.WebsiteGroup);
		});

		await LogicMonitorClient
			.DeleteAsync<ScheduledDownTime>(createdSdt.Id)
			.ConfigureAwait(false);
	}

	[Fact]
	public async void GetHistoricDeviceScheduledDownTimes()
	{
		// Device
		var testsdts =
			await LogicMonitorClient.GetDeviceHistorySdts(1053, default)
			.ConfigureAwait(false);
		testsdts.Should().NotBeNull();

		// Device Group
		var _deviceGroupHistorySdts =
			await LogicMonitorClient.GetDeviceGroupHistorySdts(1516, default)
			.ConfigureAwait(false);
		_deviceGroupHistorySdts.Should().NotBeNull();

		// Device
		var deviceHistorySdts =
			await LogicMonitorClient.GetDeviceHistorySdts(1765, default)
			.ConfigureAwait(false);
		deviceHistorySdts.Should().NotBeNull();

		// Device Data Source
		var deviceDataSourceHistorySdts =
			await LogicMonitorClient.GetDeviceDataSourceHistorySdts(1765, 98562, default)
			.ConfigureAwait(false);
		deviceDataSourceHistorySdts.Should().NotBeNull();

		// Device Data Source Instance
		var deviceDataSourceInstanceHistorySdts =
			await LogicMonitorClient.GetDeviceDataSourceInstanceHistorySdts(1765, 98562, 244662832, default)
			.ConfigureAwait(false);
		deviceDataSourceInstanceHistorySdts.Should().NotBeNull();

		// Website Group
		var websiteGroupHistorySdts =
			await LogicMonitorClient.GetWebsiteGroupHistorySdts(20, default)
			.ConfigureAwait(false);
		websiteGroupHistorySdts.Should().NotBeNull();

		// Website
		var websiteHistorySdts =
			await LogicMonitorClient.GetWebsiteHistorySdts(350, default)
			.ConfigureAwait(false);
		websiteHistorySdts.Should().NotBeNull();
	}

	[Fact]
	public async void AddAndDeleteADeviceSdt()
	{
		// var device = await portalClient.GetDeviceByDisplayNameAsync(portalConfig.WindowsDeviceDisplayName);
		const string initialComment = "Woo";
		var deviceId = WindowsDeviceId;
		var sdtCreationDto = new DeviceScheduledDownTimeCreationDto(deviceId)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddDays(7).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient
			.CreateAsync(sdtCreationDto)
			.ConfigureAwait(false);
		createdSdt.Comment.Should().Be(initialComment);
		createdSdt.DeviceId.Should().Be(deviceId);

		// Check the re-fetched SDT looks right
		var refetchSdt = await LogicMonitorClient
			.GetAsync<ScheduledDownTime>(createdSdt.Id)
			.ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(initialComment);
		refetchSdt.DeviceId.Should().Be(deviceId);

		// Update
		const string newComment = "Yay";
		createdSdt.Comment = newComment;
		await LogicMonitorClient
			.PutStringIdentifiedItemAsync(createdSdt)
			.ConfigureAwait(false);

		// Check the re-fetched SDT looks right
		refetchSdt = await LogicMonitorClient
			.GetAsync<ScheduledDownTime>(createdSdt.Id)
			.ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(newComment);
		refetchSdt.DeviceId.Should().Be(deviceId);

		// Get all scheduled downtimes (we have created one, so at least that one should be there)
		var scheduledDownTimes = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
		}).ConfigureAwait(false);
		scheduledDownTimes.Should().NotBeNullOrEmpty();

		// Get them all individually
		foreach (var sdt in scheduledDownTimes)
		{
			var refetchedSdt = await LogicMonitorClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
			refetchedSdt.Id.Should().Be(sdt.Id);
			refetchedSdt.DeviceId.Should().Be(sdt.DeviceId);
			refetchedSdt.Comment.Should().Be(sdt.Comment);
		}

		// Delete
		await LogicMonitorClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
	}

	[Fact]
	public async void AddAndDeleteADeviceGroupSdt()
	{
		const string initialComment = "Woo";
		const int deviceGroupId = 1; // The root
		var sdtCreationDto = new DeviceGroupScheduledDownTimeCreationDto(deviceGroupId)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddDays(7).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient.CreateAsync(sdtCreationDto).ConfigureAwait(false);
		createdSdt.Comment.Should().Be(initialComment);
		createdSdt.DeviceGroupId.Should().Be(deviceGroupId);

		// Check the re-fetched SDT looks right
		var refetchSdt = await LogicMonitorClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(initialComment);
		refetchSdt.DeviceGroupId.Should().Be(deviceGroupId);

		// Update
		const string newComment = "Yay";
		createdSdt.Comment = newComment;
		await LogicMonitorClient.PutStringIdentifiedItemAsync(createdSdt).ConfigureAwait(false);

		// Check the re-fetched SDT looks right
		refetchSdt = await LogicMonitorClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(newComment);
		refetchSdt.DeviceGroupId.Should().Be(deviceGroupId);

		// Get all scheduled downtimes (we have created one, so at least that one should be there)
		var scheduledDownTimes = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceGroupSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
		}).ConfigureAwait(false);
		scheduledDownTimes.Should().NotBeNullOrEmpty();

		// Get them all individually
		foreach (var sdt in scheduledDownTimes)
		{
			var refetchedSdt = await LogicMonitorClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
			refetchedSdt.Id.Should().Be(sdt.Id);
			refetchedSdt.DeviceGroupId.Should().Be(sdt.DeviceGroupId);
			refetchedSdt.Comment.Should().Be(sdt.Comment);
		}

		// Delete
		await LogicMonitorClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
	}

	[Fact]
	public async void AddAndDeleteACollectorSdt()
	{
		var portalClient = LogicMonitorClient;
		var collector = (await portalClient
			.GetAllAsync(new Filter<Collector> { Take = 1 })
			.ConfigureAwait(false))
			.SingleOrDefault();
		collector.Should().NotBeNull();
		const string initialComment = "Woo";
		var collectorId = collector.Id;
		var sdtCreationDto = new CollectorScheduledDownTimeCreationDto(collectorId)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddDays(7).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await portalClient.CreateAsync(sdtCreationDto).ConfigureAwait(false);
		createdSdt.Comment.Should().Be(initialComment);
		createdSdt.CollectorId.Should().Be(collectorId);

		// Check the re-fetched SDT looks right
		var refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(initialComment);
		refetchSdt.CollectorId.Should().Be(collectorId);

		// Update
		const string newComment = "Yay";
		createdSdt.Comment = newComment;
		await portalClient.PutStringIdentifiedItemAsync(createdSdt).ConfigureAwait(false);

		// Check the re-fetched SDT looks right
		refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		refetchSdt.Comment.Should().Be(newComment);
		refetchSdt.CollectorId.Should().Be(collectorId);

		// Get all scheduled downtimes (we have created one, so at least that one should be there)
		var scheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "CollectorSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
		}).ConfigureAwait(false);
		scheduledDownTimes.Should().NotBeNull();
		scheduledDownTimes.Should().NotBeNullOrEmpty();

		// Get them all individually
		foreach (var sdt in scheduledDownTimes)
		{
			var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
			refetchedSdt.Id.Should().Be(sdt.Id);
			refetchedSdt.DeviceId.Should().Be(sdt.DeviceId);
			refetchedSdt.Comment.Should().Be(sdt.Comment);
		}

		// Delete
		await portalClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
	}

	[Fact]
	public async void GetScheduledDownTimesFilteredByDevice()
	{
		var portalClient = LogicMonitorClient;

		var allScheduledDownTimes = await portalClient.GetAllAsync<ScheduledDownTime>().ConfigureAwait(false);

		var deviceId = allScheduledDownTimes.Find(sdt => sdt.Type == ScheduledDownTimeType.Device)?.DeviceId;
		deviceId.Should().NotBeNull();
		var filteredScheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(Type), "DeviceSDT"),
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.DeviceId), deviceId)
				}
		}
		).ConfigureAwait(false);
		filteredScheduledDownTimes.Should().NotBeNull();

		// Get them all individually
		foreach (var sdt in filteredScheduledDownTimes)
		{
			var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
			refetchedSdt.Id.Should().Be(sdt.Id);
			refetchedSdt.DeviceId.Should().Be(deviceId);
			refetchedSdt.Comment.Should().Be(sdt.Comment);
		}
	}

	[Fact]
	public async void CreatePingDataSourceSdtOnEmptyDeviceGroup()
	{
		const string deviceGroupName = "CreatePingDataSourceSdtOnEmptyDeviceGroupUnitTest";

		// Ensure DeviceGroup is NOT present
		var deviceGroup = await LogicMonitorClient
			.GetDeviceGroupByFullPathAsync(deviceGroupName)
			.ConfigureAwait(false);

		if (deviceGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(deviceGroup)
				.ConfigureAwait(false);
		}

		// Create DeviceGroup
		deviceGroup = await LogicMonitorClient
			.CreateAsync(new DeviceGroupCreationDto
			{
				ParentId = "1",
				Name = deviceGroupName
			})
			.ConfigureAwait(false);

		// Get ping DataSource
		var dataSource = (await LogicMonitorClient
			.GetAllAsync(new Filter<DataSource>
			{
				FilterItems = new List<FilterItem<DataSource>>
				{
						new Eq<DataSource>(nameof(DataSource.Name), "Ping")
				}
			})
			.ConfigureAwait(false))
			.SingleOrDefault();

		// Create Scheduled Downtime
		var sdtCreationDto = new DeviceGroupScheduledDownTimeCreationDto(deviceGroup.Id)
		{
			Comment = "Created by Unit Test",
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddDays(7).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime,
			DataSourceId = dataSource.Id,
			DataSourceName = dataSource.Name
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient
			.CreateAsync(sdtCreationDto)
			.ConfigureAwait(false);
		createdSdt.Should().NotBeNull();

		// Clean up
		await LogicMonitorClient
			.DeleteAsync<ScheduledDownTime>(createdSdt.Id)
			.ConfigureAwait(false);

		// Remove the device group
		await LogicMonitorClient
			.DeleteAsync(deviceGroup)
			.ConfigureAwait(false);
	}

	[Fact]
	public async void CreatePingDataSourceInstanceSdtOnEmptyDeviceGroup()
	{
		// Get ping DataSource
		var dataSource = (await LogicMonitorClient
			.GetAllAsync(new Filter<DataSource>
			{
				FilterItems = new List<FilterItem<DataSource>>
				{
						new Eq<DataSource>(nameof(DataSource.Name), "Ping")
				}
			})
			.ConfigureAwait(false))
			.Single();

		var deviceDataSource = await LogicMonitorClient
			.GetDeviceDataSourceByDeviceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id)
			.ConfigureAwait(false);

		var deviceDataSourceInstance = (await LogicMonitorClient
			.GetAllDeviceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id)
			.ConfigureAwait(false))
			.Single();

		// Create Scheduled Downtime
		var sdtCreationDto = new DeviceDataSourceInstanceScheduledDownTimeCreationDto(WindowsDeviceId, deviceDataSourceInstance.Id)
		{
			Comment = "Created by Unit Test",
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddDays(7).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		// Check the created SDT looks right
		var createdSdt = await LogicMonitorClient
			.CreateAsync(sdtCreationDto)
			.ConfigureAwait(false);
		createdSdt.Should().NotBeNull();

		// Clean up
		await LogicMonitorClient
			.DeleteAsync<ScheduledDownTime>(createdSdt.Id)
			.ConfigureAwait(false);
	}
}
