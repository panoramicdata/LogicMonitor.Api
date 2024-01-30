using LogicMonitor.Api.Test.Extensions;
using System.Globalization;

namespace LogicMonitor.Api.Test.Websites;

public class WebsiteTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	private const string ExpectedAlertExpression = "< 90 60 30";

	[Fact]
	public async Task GetAllWebsites()
	{
		var websites = await LogicMonitorClient
			.GetAllAsync<Website>(default)
			.ConfigureAwait(true);

		// Services should be returned
		websites.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetWebsiteByName_Exists_Succeeds()
	{
		var website = await LogicMonitorClient
			.GetByNameAsync<Website>(WebsiteName, default)
			.ConfigureAwait(true);

		// One service should be returned
		website.Should().NotBeNull();
	}

	[Fact]
	public async Task GetWebsiteByName_DoesNotExist_ReturnsNull()
	{
		var website = await LogicMonitorClient
			.GetByNameAsync<Website>("DoesNotExist", default)
			.ConfigureAwait(true);
		// Null should be returned
		website.Should().BeNull();
	}

	[Fact]
	public async Task GetWebsiteGraphData()
	{
		var website = await LogicMonitorClient
			.GetByNameAsync<Website>(WebsiteName, default)
			.ConfigureAwait(true);
		website ??= new();
		var endDateTime = DateTime.UtcNow;
		var startDateTime = endDateTime.AddMonths(-1);
		var graphDataRequest = new WebsiteOverviewGraphDataRequest
		{
			WebsiteGraphType = WebsiteGraphType.OverallStatus,
			WebsiteId = website.Id,
			StartDateTime = startDateTime,
			EndDateTime = endDateTime,
			TimePeriod = TimePeriod.Zoom
		};
		var graphData = await LogicMonitorClient
			.GetGraphDataAsync(graphDataRequest, default)
			.ConfigureAwait(true);
		graphData.Should().NotBeNull();
		graphData.Lines.Should().NotBeNullOrEmpty();
		var firstLine = graphData.Lines.FirstOrDefault();
		firstLine.Should().NotBeNull();
		firstLine ??= new();
		firstLine.Data.Should().NotBeNull();
		var firstData = firstLine.Data.FirstOrDefault();
		firstData.Should().NotBeNull();
	}

	[Fact]
	public async Task GetWebsiteAlertsWithFilterAsync()
	{
		var alertFilter = new AlertFilter
		{
			IncludeCleared = true,
			Levels =
				 [
					 AlertLevel.Warning,
					 AlertLevel.Error,
					 AlertLevel.Critical
				 ]
		};

		// Get the Website
		var website = await LogicMonitorClient
			.GetByNameAsync<Website>(WebsiteName, default)
			.ConfigureAwait(true);
		website.Should().NotBeNull();
		website ??= new();

		// Get the Alerts
		var allAlerts = await LogicMonitorClient
			.GetWebsiteAlertsByIdAsync(website.Id, alertFilter, default)
			.ConfigureAwait(true);
		allAlerts.Should().NotBeNull();
	}

	[Fact]
	public async Task SetWebsiteMonitorCheckpoints()
	{
		var websiteMonitorCheckpoints = await LogicMonitorClient
			.GetAllAsync<WebsiteMonitorCheckpoint>(default)
			.ConfigureAwait(true);

		// Some website folders should be returned
		websiteMonitorCheckpoints.Should().NotBeNullOrEmpty();

		//// Website folders should have unique Ids
		((websiteMonitorCheckpoints.Select(c => c.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async Task CrudWebsiteGroupsAndWebsites()
	{
		// Ensure the website doesn't exist
		var oldWebsites = await LogicMonitorClient.GetAllAsync(new Filter<Website>
		{
			FilterItems =
			[
				new Eq<Website>(nameof(Website.Name), nameof(CrudWebsiteGroupsAndWebsites))
			]
		}, default)
			.ConfigureAwait(true);
		foreach (var oldWebsite in oldWebsites)
		{
			await LogicMonitorClient
				.DeleteAsync<Website>(oldWebsite.Id, cancellationToken: default)
				.ConfigureAwait(true);
		}

		// Ensure the website group doesn't exist
		var oldWebsiteGroup = await LogicMonitorClient
			.GetWebsiteGroupByFullPathAsync("Test Name", default)
			.ConfigureAwait(true);
		if (oldWebsiteGroup is not null)
		{
			await LogicMonitorClient
				.DeleteAsync<WebsiteGroup>(oldWebsiteGroup.Id, cancellationToken: default)
				.ConfigureAwait(true);
		}


		// Create it
		var websiteGroup = await LogicMonitorClient.CreateAsync(new WebsiteGroupCreationDto
		{
			Name = "Test Name",
			Description = "Test Description",
			IsAlertingDisabled = false,
			IsMonitoringDisabled = false,
			ParentGroupFullPath = "",
			ParentId = "1",
			Properties =
			[
				new EntityProperty { Name = "name", Value = "value" }
			]
			//TestLocation = new TestLocation { All = true, SmgIds = new List<int> { 1, 2, 4, 3, 5, 6 } }
		}, default).ConfigureAwait(true);

		try
		{
			// Create the website
			var website = await LogicMonitorClient
				.CreateAsync(GetWebsiteCreationDto(websiteGroup.Id, nameof(CrudWebsiteGroupsAndWebsites)), default)
				.ConfigureAwait(true);
			website.Should().NotBeNull();
			try
			{
				websiteGroup.Id.Should().Be(website.GroupId);

				// Wait to ensure that it was created
				await Task.Delay(1000)
					.ConfigureAwait(true);
			}
			finally
			{
				// Delete it
				await LogicMonitorClient
					.DeleteAsync<Website>(website.Id, cancellationToken: default)
					.ConfigureAwait(true);
			}
		}
		finally
		{
			// Delete it again

			await LogicMonitorClient
				.DeleteAsync(websiteGroup, cancellationToken: default)
				.ConfigureAwait(true);
		}
	}

	[Fact]
	public async Task SetWebsiteCustomProperty()
	{
		// Make sure the website does not already exist
		var website = await LogicMonitorClient
			.GetByNameAsync<Website>(
				nameof(SetWebsiteCustomProperty),
				default
			)
			.ConfigureAwait(true)
			?? await LogicMonitorClient.CreateAsync(
				GetWebsiteCreationDto(16, nameof(SetWebsiteCustomProperty)),
				default
				).ConfigureAwait(true);

		try
		{
			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await LogicMonitorClient
				.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value1, cancellationToken: default)
				.ConfigureAwait(true);
			var deviceProperties = await LogicMonitorClient
				.GetWebsitePropertiesAsync(website.Id, default)
				.ConfigureAwait(true);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Set it to a different value
			await LogicMonitorClient
				.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value2, cancellationToken: default)
				.ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetWebsitePropertiesAsync(website.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Set it to null (delete it)
			await LogicMonitorClient
				.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, cancellationToken: default)
				.ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetWebsitePropertiesAsync(website.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Delete, default).ConfigureAwait(true)).ConfigureAwait(true);
			deletionException.Should().BeOfType<LogicMonitorApiException>();

			var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Update, default).ConfigureAwait(true)).ConfigureAwait(true);
			updateException.Should().BeOfType<InvalidOperationException>();

			var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Create, default).ConfigureAwait(true)).ConfigureAwait(true);
			createNullException.Should().BeOfType<InvalidOperationException>();

			// Create one without checking
			await LogicMonitorClient
				.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value1, SetPropertyMode.Create, default)
				.ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetWebsitePropertiesAsync(website.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Update one without checking
			await LogicMonitorClient
				.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value2, SetPropertyMode.Update, default)
				.ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetWebsitePropertiesAsync(website.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete one without checking
			await LogicMonitorClient
				.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Delete, default)
				.ConfigureAwait(true);
			deviceProperties = await LogicMonitorClient
				.GetWebsitePropertiesAsync(website.Id, default)
				.ConfigureAwait(true);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);
		}
		finally
		{
			await LogicMonitorClient
				.DeleteAsync(website, cancellationToken: default)
				.ConfigureAwait(true);
		}


	}

	[Fact]
	public async Task GetWebsiteSdts()
	{
		var websiteList = await LogicMonitorClient
			.GetAllAsync<Website>(default)
			.ConfigureAwait(true);

		var sdts = await LogicMonitorClient
			.GetWebsiteSdtListAsync(websiteList[0].Id, new Filter<ScheduledDownTime>(), default)
			.ConfigureAwait(true);
		sdts.Should().NotBeNull();
	}

	private static WebsiteCreationDto GetWebsiteCreationDto(int websiteGroupId, string name)
	=> new()
	{
		WebsiteGroupId = websiteGroupId.ToString(CultureInfo.InvariantCulture),
		Name = name,
		Description = "Description",
		PollingIntervalMinutes = 1.ToString(CultureInfo.InvariantCulture),
		Type = WebsiteType.WebCheck,
		HttpSchema = HttpSchema.Https,
		WebsiteProperties =
			[
						new EntityProperty
						{
							Name = "test",
							Value = "test"
						}
],
		Domain = "www.google.com",
		Steps =
			[
						new WebCheckStep
						{
							MatchType = Api.Websites.MatchType.Plain,
							HttpSchema = HttpSchema.Https,
							HttpMethod = "get",
							Url = "/",
							HttpVersion = "1.1",
							Description = "Step 1",
							Enable = true,
							FollowRedirection = true,
							Name = "Step 1",
							Timeout = 5,
							UseDefaultRoot = true
						}
			],
		TriggerSslExpirationAlerts = true,
		AlertExpression = ExpectedAlertExpression,
		IndividualAlertLevel = Level.Warning,
		OverallAlertLevel = Level.Warning
	};
}
