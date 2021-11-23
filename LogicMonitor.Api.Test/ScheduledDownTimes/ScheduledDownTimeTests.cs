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
	public async void GetHistoricDeviceScheduledDownTimes()
	{
		// Device
		var testsdts =
			await LogicMonitorClient.GetDeviceHistorySdts(1053, default)
			.ConfigureAwait(false);

		// Device Group
		var deviceGroupHistorySdts =
			await LogicMonitorClient.GetDeviceGroupHistorySdts(1516, default)
			.ConfigureAwait(false);

		// Device
		var deviceHistorySdts =
			await LogicMonitorClient.GetDeviceHistorySdts(1765, default)
			.ConfigureAwait(false);

		// Device Data Source
		var deviceDataSourceHistorySdts =
			await LogicMonitorClient.GetDeviceDataSourceHistorySdts(1765, 98562, default)
			.ConfigureAwait(false);

		// Device Data Source Instance
		var deviceDataSourceInstanceHistorySdts =
			await LogicMonitorClient.GetDeviceDataSourceInstanceHistorySdts(1765, 98562, 244662832, default)
			.ConfigureAwait(false);

		// Website Group
		var websiteGroupHistorySdts =
			await LogicMonitorClient.GetWebsiteGroupHistorySdts(20, default)
			.ConfigureAwait(false);

		// Website
		var websiteHistorySdts =
			await LogicMonitorClient.GetWebsiteHistorySdts(350, default)
			.ConfigureAwait(false);
	}

	[Fact]
	public async void AddAndDeleteADeviceSdt()
	{
		var portalClient = LogicMonitorClient;
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
		var createdSdt = await portalClient.CreateAsync(sdtCreationDto).ConfigureAwait(false);
		Assert.Equal(initialComment, createdSdt.Comment);
		Assert.Equal(deviceId, createdSdt.DeviceId);

		// Check the re-fetched SDT looks right
		var refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		Assert.Equal(initialComment, refetchSdt.Comment);
		Assert.Equal(deviceId, refetchSdt.DeviceId);

		// Update
		const string newComment = "Yay";
		createdSdt.Comment = newComment;
		await portalClient.PutStringIdentifiedItemAsync(createdSdt).ConfigureAwait(false);

		// Check the re-fetched SDT looks right
		refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		Assert.Equal(newComment, refetchSdt.Comment);
		Assert.Equal(deviceId, refetchSdt.DeviceId);

		// Get all scheduled downtimes (we have created one, so at least that one should be there)
		var scheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
		}).ConfigureAwait(false);
		Assert.NotNull(scheduledDownTimes);
		Assert.NotEmpty(scheduledDownTimes);

		// Get them all individually
		foreach (var sdt in scheduledDownTimes)
		{
			var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
			Assert.Equal(sdt.Id, refetchedSdt.Id);
			Assert.Equal(sdt.DeviceId, refetchedSdt.DeviceId);
			Assert.Equal(sdt.Comment, refetchedSdt.Comment);
		}

		// Delete
		await portalClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
	}

	[Fact]
	public async void AddAndDeleteADeviceGroupSdt()
	{
		var portalClient = LogicMonitorClient;
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
		var createdSdt = await portalClient.CreateAsync(sdtCreationDto).ConfigureAwait(false);
		Assert.Equal(initialComment, createdSdt.Comment);
		Assert.Equal(deviceGroupId, createdSdt.DeviceGroupId);

		// Check the re-fetched SDT looks right
		var refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		Assert.Equal(initialComment, refetchSdt.Comment);
		Assert.Equal(deviceGroupId, refetchSdt.DeviceGroupId);

		// Update
		const string newComment = "Yay";
		createdSdt.Comment = newComment;
		await portalClient.PutStringIdentifiedItemAsync(createdSdt).ConfigureAwait(false);

		// Check the re-fetched SDT looks right
		refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		Assert.Equal(newComment, refetchSdt.Comment);
		Assert.Equal(deviceGroupId, refetchSdt.DeviceGroupId);

		// Get all scheduled downtimes (we have created one, so at least that one should be there)
		var scheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceGroupSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
		}).ConfigureAwait(false);
		Assert.NotNull(scheduledDownTimes);
		Assert.NotEmpty(scheduledDownTimes);

		// Get them all individually
		foreach (var sdt in scheduledDownTimes)
		{
			var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
			Assert.Equal(sdt.Id, refetchedSdt.Id);
			Assert.Equal(sdt.DeviceGroupId, refetchedSdt.DeviceGroupId);
			Assert.Equal(sdt.Comment, refetchedSdt.Comment);
		}

		// Delete
		await portalClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
	}

	[Fact]
	public async void AddAndDeleteACollectorSdt()
	{
		var portalClient = LogicMonitorClient;
		var collector = (await portalClient
			.GetAllAsync(new Filter<Collectors.Collector> { Take = 1 })
			.ConfigureAwait(false))
			.SingleOrDefault();
		Assert.NotNull(collector);
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
		Assert.Equal(initialComment, createdSdt.Comment);
		Assert.Equal(collectorId, createdSdt.CollectorId);

		// Check the re-fetched SDT looks right
		var refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		Assert.Equal(initialComment, refetchSdt.Comment);
		Assert.Equal(collectorId, refetchSdt.CollectorId);

		// Update
		const string newComment = "Yay";
		createdSdt.Comment = newComment;
		await portalClient.PutStringIdentifiedItemAsync(createdSdt).ConfigureAwait(false);

		// Check the re-fetched SDT looks right
		refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		Assert.Equal(newComment, refetchSdt.Comment);
		Assert.Equal(collectorId, refetchSdt.CollectorId);

		// Get all scheduled downtimes (we have created one, so at least that one should be there)
		var scheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "CollectorSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
		}).ConfigureAwait(false);
		Assert.NotNull(scheduledDownTimes);
		Assert.NotEmpty(scheduledDownTimes);

		// Get them all individually
		foreach (var sdt in scheduledDownTimes)
		{
			var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
			Assert.Equal(sdt.Id, refetchedSdt.Id);
			Assert.Equal(sdt.DeviceId, refetchedSdt.DeviceId);
			Assert.Equal(sdt.Comment, refetchedSdt.Comment);
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
		Assert.NotNull(deviceId);
		var filteredScheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(Type), "DeviceSDT"),
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.DeviceId), deviceId)
				}
		}
		).ConfigureAwait(false);
		Assert.NotNull(filteredScheduledDownTimes);

		// Get them all individually
		foreach (var sdt in filteredScheduledDownTimes)
		{
			var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
			Assert.Equal(sdt.Id, refetchedSdt.Id);
			Assert.Equal(deviceId, refetchedSdt.DeviceId);
			Assert.Equal(sdt.Comment, refetchedSdt.Comment);
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

		if (deviceGroup != null)
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
		Assert.NotNull(createdSdt);

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
		Assert.NotNull(createdSdt);

		// Clean up
		await LogicMonitorClient
			.DeleteAsync<ScheduledDownTime>(createdSdt.Id)
			.ConfigureAwait(false);
	}
}
