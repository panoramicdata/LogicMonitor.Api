namespace LogicMonitor.Api.Test.Websites;

public class WebsiteTests : TestWithOutput
{
	private const string ExpectedAlertExpression = "< 90 60 30";

	private static WebsiteCreationDto GetWebsiteCreationDto(int websiteGroupId, string name)
		=> new()
		{
			WebsiteGroupId = websiteGroupId.ToString(),
			Name = name,
			Description = "Description",
			PollingIntervalMinutes = 1.ToString(),
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

	public WebsiteTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAllWebsites()
	{
		var websites = await LogicMonitorClient.GetAllAsync<Website>().ConfigureAwait(false);

		// Services should be returned
		websites.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void GetWebsiteByName_Exists_Succeeds()
	{
		var website = await LogicMonitorClient.GetByNameAsync<Website>(WebsiteName).ConfigureAwait(false);

		// One service should be returned
		website.Should().NotBeNull();
	}

	[Fact]
	public async void GetWebsiteByName_DoesNotExist_ReturnsNull()
	{
		var website = await LogicMonitorClient.GetByNameAsync<Website>("DoesNotExist").ConfigureAwait(false);
		// Null should be returned
		website.Should().BeNull();
	}

	[Fact]
	public async void GetWebsiteGraphData()
	{
		var website = await LogicMonitorClient.GetByNameAsync<Website>(WebsiteName).ConfigureAwait(false);

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
		var graphData = await LogicMonitorClient.GetGraphDataAsync(graphDataRequest).ConfigureAwait(false);
		graphData.Should().NotBeNull();
		graphData.Lines.Should().NotBeNullOrEmpty();
		var firstLine = graphData.Lines.FirstOrDefault();
		firstLine.Should().NotBeNull();
		firstLine.Data.Should().NotBeNull();
		var firstData = firstLine.Data.FirstOrDefault();
		firstData.Should().NotBeNull();
	}

	[Fact]
	public async void GetWebsiteAlertsWithFilterAsync()
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
		var website = await LogicMonitorClient.GetByNameAsync<Website>(WebsiteName).ConfigureAwait(false);
		website.Should().NotBeNull();

		// Get the Alerts
		var allAlerts = await LogicMonitorClient.GetWebsiteAlertsByIdAsync(website.Id, alertFilter, default).ConfigureAwait(false);
		allAlerts.Should().NotBeNull();
	}

	[Fact]
	public async void GetWebsiteGroupByFullPath()
	{
		var websiteGroup0 = await LogicMonitorClient.GetWebsiteGroupByFullPathAsync(string.Empty).ConfigureAwait(false);
		websiteGroup0.Should().NotBeNull();
		websiteGroup0.FullPath.Should().Be(string.Empty);
		var websiteGroup1 = await LogicMonitorClient.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath).ConfigureAwait(false);
		websiteGroup1.Should().NotBeNull();
		var websiteGroup2 = await LogicMonitorClient.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath).ConfigureAwait(false);
		websiteGroup2.Should().NotBeNull();
		websiteGroup0.Id.Should().NotBe(websiteGroup2.Id);
	}

	[Fact]
	public async void GetWebsiteGroupById()
	{
		var websiteGroup = await LogicMonitorClient.GetAsync<WebsiteGroup>(1).ConfigureAwait(false);

		Assert.NotNull(websiteGroup);
		Assert.NotNull(websiteGroup.ChildWebsiteGroups);
		Assert.True(websiteGroup.ChildWebsiteGroups.Count > 0);
		Assert.Equal(0, websiteGroup.ParentId);
		Assert.Equal(1, websiteGroup.Id);
		Assert.False(string.IsNullOrWhiteSpace(websiteGroup.Name));
		Assert.False(websiteGroup.FullPath == null);
	}

	[Fact]
	public async void SetWebsiteMonitorCheckpoints()
	{
		var websiteMonitorCheckpoints = await LogicMonitorClient.GetAllAsync<WebsiteMonitorCheckpoint>().ConfigureAwait(false);

		// Some website folders should be returned
		Assert.True(websiteMonitorCheckpoints.Count > 0);

		//// Website folders should have unique Ids
		//Assert.False(websiteMonitorCheckpoints.Select(c => c.Id).HasDuplicates());
	}

	[Fact]
	public async void CrudWebsiteGroupsAndWebsites()
	{
		// Ensure the website group doesn't exist
		var oldWebsiteGroup = await LogicMonitorClient.GetWebsiteGroupByFullPathAsync("Test Name").ConfigureAwait(false);
		if (oldWebsiteGroup != null)
		{
			await LogicMonitorClient.DeleteAsync<WebsiteGroup>(oldWebsiteGroup.Id).ConfigureAwait(false);
		}

		// Ensure the website doesn't exist
		var oldWebsites = await LogicMonitorClient.GetAllAsync(new Filter<Website>
		{
			FilterItems = new List<FilterItem<Website>>
				{
					new Eq<Website>(nameof(Website.Name), nameof(CrudWebsiteGroupsAndWebsites))
				}
		}).ConfigureAwait(false);
		foreach (var oldWebsite in oldWebsites)
		{
			await LogicMonitorClient.DeleteAsync<Website>(oldWebsite.Id).ConfigureAwait(false);
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
					{new Property {Name = "name", Value = "value"}},
			//TestLocation = new TestLocation { All = true, SmgIds = new List<int> { 1, 2, 4, 3, 5, 6 } }
		}).ConfigureAwait(false);

		try
		{
			// Create the website
			var website = await LogicMonitorClient
				.CreateAsync(GetWebsiteCreationDto(websiteGroup.Id, nameof(CrudWebsiteGroupsAndWebsites)))
				.ConfigureAwait(false);
			website.Should().NotBeNull(); ;
			websiteGroup.Id.Should().Equals(website.WebsiteGroupId);
			website.TriggerSslExpirationAlerts.Should().BeTrue();
			ExpectedAlertExpression.Should().Be(website.AlertExpression);

			// Wait to ensure that it was created
			await Task.Delay(1000)
				.ConfigureAwait(false);

			// Delete it
			await LogicMonitorClient
				.DeleteAsync<Website>(website.Id)
				.ConfigureAwait(false);
		}
		finally
		{
			// Delete it again
			await LogicMonitorClient
				.DeleteAsync(websiteGroup)
				.ConfigureAwait(false);
		}
	}

	[Fact]
	public async void SetWebsiteCustomProperty()
	{
		var website = await LogicMonitorClient.CreateAsync(GetWebsiteCreationDto(16, nameof(SetWebsiteCustomProperty))).ConfigureAwait(false);
		try
		{
			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value1).ConfigureAwait(false);
			var deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Set it to a different value
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value2).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Set it to null (delete it)
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<LogicMonitorApiException>(deletionException);

			var updateException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Update).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(updateException);

			var createNullException = await Record.ExceptionAsync(async () => await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Create).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(createNullException);

			// Create one without checking
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value1, SetPropertyMode.Create).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Update one without checking
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value2, SetPropertyMode.Update).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Delete one without checking
			await LogicMonitorClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false);
			deviceProperties = await LogicMonitorClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);
		}
		finally
		{
			await LogicMonitorClient.DeleteAsync(website).ConfigureAwait(false);
		}
	}
}
