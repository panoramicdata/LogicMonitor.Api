using System.Globalization;

namespace LogicMonitor.Api.Test.Websites;

public class WebsiteTests : TestWithOutput
{
	private const string ExpectedAlertExpression = "< 90 60 30";

	public WebsiteTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetAllWebsites()
	{
		var websites = await LogicMonitorClient.GetAllAsync<Website>(CancellationToken.None).ConfigureAwait(false);

		// Services should be returned
		websites.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetWebsiteByName_Exists_Succeeds()
	{
		var website = await LogicMonitorClient.GetByNameAsync<Website>(WebsiteName, CancellationToken.None).ConfigureAwait(false);

		// One service should be returned
		website.Should().NotBeNull();
	}

	[Fact]
	public async Task GetWebsiteByName_DoesNotExist_ReturnsNull()
	{
		var website = await LogicMonitorClient.GetByNameAsync<Website>("DoesNotExist", CancellationToken.None).ConfigureAwait(false);
		// Null should be returned
		website.Should().BeNull();
	}

	[Fact]
	public async Task GetWebsiteGraphData()
	{
		var website = await LogicMonitorClient
			.GetByNameAsync<Website>(WebsiteName, CancellationToken.None)
			.ConfigureAwait(false);

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
		var graphData = await LogicMonitorClient.GetGraphDataAsync(graphDataRequest, CancellationToken.None).ConfigureAwait(false);
		graphData.Should().NotBeNull();
		graphData.Lines.Should().NotBeNullOrEmpty();
		var firstLine = graphData.Lines.FirstOrDefault();
		firstLine.Should().NotBeNull();
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
			Levels = new List<AlertLevel>
				 {
					 AlertLevel.Warning,
					 AlertLevel.Error,
					 AlertLevel.Critical
				 }
		};

		// Get the Website
		var website = await LogicMonitorClient.GetByNameAsync<Website>(WebsiteName, CancellationToken.None).ConfigureAwait(false);
		website.Should().NotBeNull();

		// Get the Alerts
		var allAlerts = await LogicMonitorClient.GetWebsiteAlertsByIdAsync(website.Id, alertFilter, default).ConfigureAwait(false);
		allAlerts.Should().NotBeNull();
	}

	[Fact]
	public async Task SetWebsiteMonitorCheckpoints()
	{
		var websiteMonitorCheckpoints = await LogicMonitorClient.GetAllAsync<WebsiteMonitorCheckpoint>(CancellationToken.None).ConfigureAwait(false);

		// Some website folders should be returned
		websiteMonitorCheckpoints.Should().NotBeNullOrEmpty();

		//// Website folders should have unique Ids
		//((websiteMonitorCheckpoints.Select(c => c.Id).HasDuplicates())).Should().BeFalse();
	}

	[Fact]
	public async Task CrudWebsiteGroupsAndWebsites()
	{
		// Ensure the website doesn't exist
		var oldWebsites = await LogicMonitorClient.GetAllAsync(new Filter<Website>
		{
			FilterItems = new List<FilterItem<Website>>
				{
					new Eq<Website>(nameof(Website.Name), nameof(CrudWebsiteGroupsAndWebsites))
				}
		}, CancellationToken.None).ConfigureAwait(false);
		foreach (var oldWebsite in oldWebsites)
		{
			await LogicMonitorClient.DeleteAsync<Website>(oldWebsite.Id, cancellationToken: CancellationToken.None).ConfigureAwait(false);
		}

		// Ensure the website group doesn't exist
		var oldWebsiteGroup = await LogicMonitorClient.GetWebsiteGroupByFullPathAsync("Test Name", CancellationToken.None).ConfigureAwait(false);
		if (oldWebsiteGroup is not null)
		{
			await LogicMonitorClient.DeleteAsync<WebsiteGroup>(oldWebsiteGroup.Id, cancellationToken: CancellationToken.None).ConfigureAwait(false);
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
			Properties = new List<Property>
			{
				new Property {Name = "name", Value = "value"}
			}
			//TestLocation = new TestLocation { All = true, SmgIds = new List<int> { 1, 2, 4, 3, 5, 6 } }
		}, CancellationToken.None).ConfigureAwait(false);

		try
		{
			// Create the website
			var website = await LogicMonitorClient
				.CreateAsync(GetWebsiteCreationDto(websiteGroup.Id, nameof(CrudWebsiteGroupsAndWebsites)), CancellationToken.None)
				.ConfigureAwait(false);
			website.Should().NotBeNull();
			try
			{
				websiteGroup.Id.Should().Be(website.WebsiteGroupId);
				website.TriggerSslExpirationAlerts.Should().BeTrue();
				ExpectedAlertExpression.Should().Be(website.AlertExpression);

				// Wait to ensure that it was created
				await Task.Delay(1000)
					.ConfigureAwait(false);
			}
			finally
			{
				// Delete it
				await LogicMonitorClient
					.DeleteAsync<Website>(website.Id, cancellationToken: CancellationToken.None)
					.ConfigureAwait(false);
			}
		}
		finally
		{
			// Delete it again

			await LogicMonitorClient
				.DeleteAsync(websiteGroup, cancellationToken: CancellationToken.None)
				.ConfigureAwait(false);
		}
	}

	[Fact]
	public async Task SetWebsiteCustomProperty()
	{
		// Make sure the website does not already exist
		var website = await LogicMonitorClient
			.GetByNameAsync<Website>(
				nameof(SetWebsiteCustomProperty),
				CancellationToken.None
			)
			.ConfigureAwait(false)
			?? await LogicMonitorClient.CreateAsync(
				GetWebsiteCreationDto(16, nameof(SetWebsiteCustomProperty)),
				CancellationToken.None
				).ConfigureAwait(false);

		try
		{
			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value1, cancellationToken: CancellationToken.None).ConfigureAwait(false);
			var deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id, CancellationToken.None).ConfigureAwait(false);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Set it to a different value
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value2, cancellationToken: CancellationToken.None).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id, CancellationToken.None).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Set it to null (delete it)
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, cancellationToken: CancellationToken.None).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id, CancellationToken.None).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Delete, CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);
			deletionException.Should().BeOfType<LogicMonitorApiException>();

			var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Update, CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);
			updateException.Should().BeOfType<InvalidOperationException>();

			var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Create, CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);
			createNullException.Should().BeOfType<InvalidOperationException>();

			// Create one without checking
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value1, SetPropertyMode.Create, CancellationToken.None).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id, CancellationToken.None).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			actual.Should().Be(1);

			// Update one without checking
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value2, SetPropertyMode.Update, CancellationToken.None).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id, CancellationToken.None).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			actual.Should().Be(1);

			// Delete one without checking
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Delete, CancellationToken.None).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id, CancellationToken.None).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			actual.Should().Be(0);
		}
		finally
		{
			await LogicMonitorClient
				.DeleteAsync(website, cancellationToken: CancellationToken.None)
				.ConfigureAwait(false);
		}


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
		WebsiteProperties = new List<Property>
			{
						new Property
						{
							Name = "test",
							Value = "test"
						}
},
		Domain = "www.google.com",
		Steps = new List<WebsiteStep>
			{
						new WebsiteStep
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
			},
		TriggerSslExpirationAlerts = true,
		AlertExpression = ExpectedAlertExpression,
		IndividualAlertLevel = Level.Warning,
		OverallAlertLevel = Level.Warning
	};
}
