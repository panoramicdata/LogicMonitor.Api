namespace LogicMonitor.Api.Test.ScheduledDownTimes;

public class ScheduledDownTimeTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetResourceScheduledDownTimes()
	{
		var sdts = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems =
			[
				new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), ScheduledDownTimeType.Resource),
				new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.IsEffective), false),
			]
		}, CancellationToken);
		sdts.Should().NotBeNull();
		sdts.Should().AllSatisfy(sdt => sdt.Type.Should().Be(ScheduledDownTimeType.Resource));
		sdts.Should().AllSatisfy(sdt => sdt.IsEffective.Should().BeFalse());
	}

	[Fact]
	public async Task GetWebsiteGroupScheduledDownTimes_UsingSubUrl_Succeeds()
	{
		// Ensure that a website group SDT exists with an unusual comment
		var commentGuid = Guid.NewGuid();
		var initialComment = $"ABC {commentGuid} DEF";
		var websiteGroupId = 1;
		var sdtCreationDto = new WebsiteGroupScheduledDownTimeCreationDto(1)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		ScheduledDownTime? createdSdt = null;
		try
		{
			// Check the created SDT looks right
			createdSdt = await LogicMonitorClient
				.CreateAsync(sdtCreationDto, CancellationToken);
			createdSdt.Comment.Should().Be(initialComment);
			createdSdt.WebsiteGroupId.Should().Be(websiteGroupId);

			var subUrl = $"sdt/sdts?filter=type:\"WebsiteGroupSDT\",comment~\"{commentGuid}\"";

			var scheduledDownTimes = await LogicMonitorClient
				.GetAllAsync<ScheduledDownTime>(subUrl, CancellationToken);

			scheduledDownTimes.Should().AllSatisfy(sdt =>
			{
				sdt.Comment.Should().Contain(commentGuid.ToString());
				sdt.Type.Should().Be(ScheduledDownTimeType.WebsiteGroup);
			});
		}
		finally
		{
			// Clean up the effects of the test
			if (createdSdt is not null)
			{
				await LogicMonitorClient
					.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: default)
					;
			}
		}
	}

	[Fact]
	public async Task GetHistoricDeviceScheduledDownTimes()
	{
		// Device
		var testsdts =
			await LogicMonitorClient.GetResourceHistorySdtsAsync(1053, CancellationToken);
		testsdts.Should().NotBeNull();

		// ResourceGroup - currently throws a permission denied error
		//var _deviceGroupHistorySdts =
		//	await LogicMonitorClient.GetDeviceGroupHistorySdts(1516, CancellationToken)
		//	;
		//_deviceGroupHistorySdts.Should().NotBeNull();

		// Device
		var deviceHistorySdts =
			await LogicMonitorClient.GetResourceHistorySdtsAsync(1765, CancellationToken);
		deviceHistorySdts.Should().NotBeNull();

		// Device Data Source
		var deviceDataSourceHistorySdts =
			await LogicMonitorClient.GetResourceDataSourceHistorySdtsAsync(1765, 98562, CancellationToken);
		deviceDataSourceHistorySdts.Should().NotBeNull();

		// Device Data Source Instance
		var deviceDataSourceInstanceHistorySdts =
			await LogicMonitorClient.GetResourceDataSourceInstanceHistorySdtsAsync(1765, 98562, 244662832, CancellationToken);
		deviceDataSourceInstanceHistorySdts.Should().NotBeNull();

		// Website Group
		var websiteGroupHistorySdts =
			await LogicMonitorClient.GetAllSdtListByWebsiteGroupIdAsync(20, new(), CancellationToken);
		websiteGroupHistorySdts.Should().NotBeNull();

		// Website
		var website = await LogicMonitorClient
			.GetByNameAsync<Website>(WebsiteName, CancellationToken);
		website.Should().NotBeNull();
		var websiteHistorySdts =
			await LogicMonitorClient.GetSdtHistoryByWebsiteIdAsync(website.Id, new(), CancellationToken);
		websiteHistorySdts.Should().NotBeNull();
	}

	[Fact]
	public async Task AddAndDeleteADeviceSdt()
	{
		const string initialComment = "LogicMonitor.Api unit tests - AddAndDeleteADeviceSdt initial comment";
		var deviceId = WindowsDeviceId;
		var sdtCreationDto = new ResourceScheduledDownTimeCreationDto(deviceId)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		ScheduledDownTime? createdSdt = null;
		try
		{
			// Check the created SDT looks right
			createdSdt = await LogicMonitorClient
				.CreateAsync(sdtCreationDto, CancellationToken);
			createdSdt.Comment.Should().Be(initialComment);
			createdSdt.ResourceId.Should().Be(deviceId);

			// Check the re-fetched SDT looks right
			var refetchSdt = await LogicMonitorClient
				.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken);
			refetchSdt.Comment.Should().Be(initialComment);
			refetchSdt.ResourceId.Should().Be(deviceId);

			// Update
			const string newComment = "LogicMonitor.Api unit tests - AddAndDeleteADeviceSdt new comment";
			createdSdt.Comment = newComment;
			await LogicMonitorClient
				.PutStringIdentifiedItemAsync(createdSdt, CancellationToken);

			// Check the re-fetched SDT looks right
			refetchSdt = await LogicMonitorClient
				.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken);
			refetchSdt.Comment.Should().Be(newComment);
			refetchSdt.ResourceId.Should().Be(deviceId);

			// Get all scheduled downtimes (we have created one, so at least that one should be there)
			var scheduledDownTimes = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
			{
				FilterItems =
				[
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "ResourceSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				]
			}, CancellationToken);
			scheduledDownTimes.Should().NotBeNullOrEmpty();

			// Get them all individually
			foreach (var sdt in scheduledDownTimes)
			{
				var refetchedSdt = await LogicMonitorClient
					.GetAsync<ScheduledDownTime>(sdt.Id, CancellationToken);
				refetchedSdt.Id.Should().Be(sdt.Id);
				refetchedSdt.ResourceId.Should().Be(sdt.ResourceId);
				refetchedSdt.Comment.Should().Be(sdt.Comment);
			}
		}
		finally
		{
			// Clean up the effects of the test
			if (createdSdt is not null)
			{
				// Delete
				await LogicMonitorClient
					.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: default)
					;
			}
		}
	}

	//[Fact]
	//public async Task AddAndDeleteAResourceGroupSdt()
	//{
	//	const string initialComment = "LogicMonitor.Api unit tests - AddAndDeleteAResourceGroupSdt initial comment";
	//	var resourceGroupId = SdtResourceGroupId;
	//	var sdtCreationDto = new ResourceGroupScheduledDownTimeCreationDto(resourceGroupId)
	//	{
	//		Comment = initialComment,
	//		StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
	//		EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
	//		RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
	//	};

	//	ScheduledDownTime? createdSdt = null;
	//	try
	//	{
	//		// Check the created SDT looks right
	//		createdSdt = await LogicMonitorClient
	//			.CreateAsync(sdtCreationDto, CancellationToken)
	//			;
	//		createdSdt.Comment.Should().Be(initialComment);
	//		createdSdt.DeviceGroupId.Should().Be(resourceGroupId);

	//		// Check the re-fetched SDT looks right
	//		var refetchSdt = await LogicMonitorClient
	//			.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken)
	//			;
	//		refetchSdt.Comment.Should().Be(initialComment);
	//		refetchSdt.DeviceGroupId.Should().Be(resourceGroupId);

	//		// Update
	//		const string newComment = "LogicMonitor.Api unit tests - AddAndDeleteAResourceGroupSdt new comment";
	//		createdSdt.Comment = newComment;
	//		await LogicMonitorClient
	//			.PutStringIdentifiedItemAsync(createdSdt, CancellationToken)
	//			;

	//		// Check the re-fetched SDT looks right
	//		refetchSdt = await LogicMonitorClient
	//			.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken)
	//			;
	//		refetchSdt.Comment.Should().Be(newComment);
	//		refetchSdt.DeviceGroupId.Should().Be(resourceGroupId);

	//		// Get all scheduled downtimes (we have created one, so at least that one should be there)
	//		var scheduledDownTimes = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
	//		{
	//			FilterItems =
	//			[
	//				new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "ResourceGroupSDT"),
	//				new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
	//			]
	//		}, CancellationToken);
	//		scheduledDownTimes.Should().NotBeNullOrEmpty();

	//		// Get them all individually
	//		foreach (var sdt in scheduledDownTimes)
	//		{
	//			var refetchedSdt = await LogicMonitorClient
	//				.GetAsync<ScheduledDownTime>(sdt.Id, CancellationToken)
	//				;
	//			refetchedSdt.Id.Should().Be(sdt.Id);
	//			refetchedSdt.DeviceGroupId.Should().Be(sdt.DeviceGroupId);
	//			refetchedSdt.Comment.Should().Be(sdt.Comment);
	//		}
	//	}
	//	finally
	//	{
	//		// Clean up the effects of the test
	//		if (createdSdt is not null)
	//		{
	//			// Delete
	//			await LogicMonitorClient
	//				.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: default)
	//				;
	//		}
	//	}
	//}

	[Fact]
	public async Task AddAndDeleteACollectorSdt()
	{
		var portalClient = LogicMonitorClient;
		var collector = (await portalClient
			.GetAllAsync(new Filter<Collector> { Take = 1 }, CancellationToken)
			)
			.SingleOrDefault();
		collector.Should().NotBeNull();
		const string initialComment = "LogicMonitor.Api unit tests - AddAndDeleteACollectorSdt initial comment";
		var collectorId = collector.Id;
		var sdtCreationDto = new CollectorScheduledDownTimeCreationDto(collectorId)
		{
			Comment = initialComment,
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		ScheduledDownTime? createdSdt = null;
		try
		{
			// Check the created SDT looks right
			createdSdt = await portalClient
				.CreateAsync(sdtCreationDto, CancellationToken);
			createdSdt.Comment.Should().Be(initialComment);
			createdSdt.CollectorId.Should().Be(collectorId);

			// Check the re-fetched SDT looks right
			var refetchSdt = await portalClient
				.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken);
			refetchSdt.Comment.Should().Be(initialComment);
			refetchSdt.CollectorId.Should().Be(collectorId);

			// Update
			const string newComment = "LogicMonitor.Api unit tests - AddAndDeleteACollectorSdt new comment";
			createdSdt.Comment = newComment;
			await portalClient.PutStringIdentifiedItemAsync(createdSdt, CancellationToken);

			// Check the re-fetched SDT looks right
			refetchSdt = await portalClient
				.GetAsync<ScheduledDownTime>(createdSdt.Id, CancellationToken);
			refetchSdt.Comment.Should().Be(newComment);
			refetchSdt.CollectorId.Should().Be(collectorId);

			// Get all scheduled downtimes (we have created one, so at least that one should be there)
			var scheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
			{
				FilterItems =
				[
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "CollectorSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				]
			}, CancellationToken);
			scheduledDownTimes.Should().NotBeNull();
			scheduledDownTimes.Should().NotBeNullOrEmpty();

			// Get them all individually
			foreach (var sdt in scheduledDownTimes)
			{
				var refetchedSdt = await portalClient
					.GetAsync<ScheduledDownTime>(sdt.Id, CancellationToken);
				refetchedSdt.Id.Should().Be(sdt.Id);
				refetchedSdt.ResourceId.Should().Be(sdt.ResourceId);
				refetchedSdt.Comment.Should().Be(sdt.Comment);
			}
		}
		finally
		{
			// Clean up the effects of the test
			if (createdSdt is not null)
			{
				// Delete
				await portalClient
					.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: default)
					;
			}
		}
	}

	[Fact]
	public async Task GetScheduledDownTimesFilteredByDevice()
	{

		var allScheduledDownTimes = await LogicMonitorClient.GetAllAsync<ScheduledDownTime>(CancellationToken);

		var deviceScheduledDownTime = allScheduledDownTimes.Find(sdt => sdt.Type == ScheduledDownTimeType.Resource);
		deviceScheduledDownTime.Should().NotBeNull();
		var resourceId = deviceScheduledDownTime.ResourceId;

		var filteredScheduledDownTimes = await LogicMonitorClient.GetAllAsync(new Filter<ScheduledDownTime>
		{
			FilterItems =
			[
				new Eq<ScheduledDownTime>(nameof(Type), "DeviceSDT"),
				new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.ResourceId), resourceId!)
			]
		}, default
		);
		filteredScheduledDownTimes.Should().NotBeNull();

		// Get them all individually
		foreach (var sdt in filteredScheduledDownTimes)
		{
			var refetchedSdt = await LogicMonitorClient
				.GetAsync<ScheduledDownTime>(sdt.Id, CancellationToken);
			refetchedSdt.Id.Should().Be(sdt.Id);
			refetchedSdt.ResourceId.Should().Be(resourceId);
			refetchedSdt.Comment.Should().Be(sdt.Comment);
		}
	}

	[Fact]
	public async Task CreatePingDataSourceSdtOnEmptyDeviceGroup()
	{
		const string resourceGroupName = "CreatePingDataSourceSdtOnEmptyDeviceGroupUnitTest";

		// Ensure DeviceGroup is NOT present
		var resourceGroup = await LogicMonitorClient
			.GetResourceGroupByFullPathAsync(resourceGroupName, CancellationToken);

		if (resourceGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync(resourceGroup, cancellationToken: default)
				;
		}

		// Create DeviceGroup
		resourceGroup = await LogicMonitorClient
			.CreateAsync(new ResourceGroupCreationDto
			{
				ParentId = "1",
				Name = resourceGroupName
			}, CancellationToken);

		// Get ping DataSource
		var dataSource = (await LogicMonitorClient
			.GetAllAsync(new Filter<DataSource>
			{
				FilterItems =
				[
						new Eq<DataSource>(nameof(DataSource.Name), "Ping")
				]
			}, CancellationToken)
			)
			.SingleOrDefault();
		dataSource.Should().NotBeNull();

		// Create Scheduled Downtime
		var sdtCreationDto = new ResourceGroupScheduledDownTimeCreationDto(resourceGroup.Id)
		{
			Comment = "Created by Unit Test",
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime,
			DataSourceId = dataSource.Id,
			DataSourceName = dataSource.Name
		};

		ScheduledDownTime? createdSdt = null;
		try
		{
			// Check the created SDT looks right
			createdSdt = await LogicMonitorClient
				.CreateAsync(sdtCreationDto, CancellationToken);
			createdSdt.Should().NotBeNull();
		}
		finally
		{
			if (createdSdt is not null)
			{
				// Clean up
				await LogicMonitorClient
					.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: default)
					;
			}
		}

		// Remove the ResourceGroup
		await LogicMonitorClient
			.DeleteAsync(resourceGroup, cancellationToken: default)
			;
	}

	[Fact]
	public async Task CreatePingDataSourceInstanceSdtOnEmptyDeviceGroup()
	{
		// Get ping DataSource
		var dataSource = (await LogicMonitorClient
			.GetAllAsync(new Filter<DataSource>
			{
				FilterItems =
				[
					new Eq<DataSource>(nameof(DataSource.Name), "Ping")
				]
			}, CancellationToken)
			)
			.Single();

		var deviceDataSource = await LogicMonitorClient
			.GetResourceDataSourceByResourceIdAndDataSourceIdAsync(WindowsDeviceId, dataSource.Id, CancellationToken);

		var deviceDataSourceInstance = (await LogicMonitorClient
			.GetAllResourceDataSourceInstancesAsync(WindowsDeviceId, deviceDataSource.Id, new(), cancellationToken: default)
			)
			.Single();

		// Create Scheduled Downtime
		var sdtCreationDto = new ResourceDataSourceInstanceScheduledDownTimeCreationDto(WindowsDeviceId, deviceDataSourceInstance.Id)
		{
			Comment = "Created by Unit Test",
			StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
			EndDateTimeEpochMs = DateTime.UtcNow.AddMinutes(10).MillisecondsSinceTheEpoch(),
			RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
		};

		ScheduledDownTime? createdSdt = null;
		try
		{
			// Check the created SDT looks right
			createdSdt = await LogicMonitorClient
				.CreateAsync(sdtCreationDto, CancellationToken);
			createdSdt.Should().NotBeNull();

		}
		finally
		{
			// Clean up
			if (createdSdt is not null)
			{
				await LogicMonitorClient
					.DeleteAsync<ScheduledDownTime>(createdSdt.Id, cancellationToken: default)
					;
			}
		}
	}

	[Fact]
	public async Task GetResourceGroupSdts()
	{
		var groupSDTsList = await LogicMonitorClient
			.GetResourceGroupHistorySdtsAsync(1950, CancellationToken);

		groupSDTsList.Should().NotBeNull();
	}

	[Fact]
	public async Task GetHistoricWebsiteGroupSdt()
	{
		var websiteGroupId = 1;

		var websiteGroupSdts = await LogicMonitorClient
			.GetSdtHistoryListByWebsiteGroupIdAsync(websiteGroupId, new Filter<ScheduledDownTimeHistory>(), CancellationToken);

		websiteGroupSdts.Should().NotBeNull();
	}
}
