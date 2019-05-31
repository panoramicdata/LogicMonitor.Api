using LogicMonitor.Api.Data;
using LogicMonitor.Api.Time;
using LogicMonitor.Api.Websites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Websites
{
	public class WebsiteTests : TestWithOutput
	{
		private static WebsiteCreationDto GetWebsiteCreationDto(int websiteGroupId, string name)
			=> new WebsiteCreationDto
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
							MatchType = MatchType.Plain,
							HttpMethod = "get",
							Url = "https://www.google.com/",
							HttpVersion = "1.1",
							Description = "Step 1",
							Enable = true,
							FollowRedirection = true,
							Name = "Step 1",
							Timeout = 5,
							HttpSchema = HttpSchema.Https,
						}
					}
			};

		public WebsiteTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetAllWebsites()
		{
			var websites = await PortalClient.GetAllAsync<Website>().ConfigureAwait(false);

			// Services should be returned
			Assert.NotNull(websites);
			Assert.NotEmpty(websites);
		}

		[Fact]
		public async void GetWebsiteByName()
		{
			var website = await PortalClient.GetByNameAsync<Website>("reportmagic.net HTTPS").ConfigureAwait(false);

			// One service should be returned
			Assert.NotNull(website);
		}

		[Fact]
		public async void GetWebsiteByNamePing()
		{
			var website = await PortalClient.GetByNameAsync<Website>("reportmagic.net Ping").ConfigureAwait(false);
			// One website should be returned
			Assert.NotNull(website);
		}

		[Fact]
		public async void GetWebsiteGraphData()
		{
			var website = await PortalClient.GetByNameAsync<Website>("reportmagic.net HTTPS").ConfigureAwait(false);

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
			var graphData = await PortalClient.GetGraphDataAsync(graphDataRequest).ConfigureAwait(false);
			Assert.NotNull(graphData);
			Assert.NotEmpty(graphData.Lines);
			var firstLine = graphData.Lines.FirstOrDefault();
			Assert.NotNull(firstLine);
			Assert.NotNull(firstLine.Data);
			var firstData = firstLine.Data.FirstOrDefault();
			Assert.NotNull(firstData);
		}

		[Fact]
		public async void GetWebsiteGroupByFullPath()
		{
			var websiteGroup0 = await PortalClient.GetWebsiteGroupByFullPathAsync(string.Empty).ConfigureAwait(false);
			Assert.NotNull(websiteGroup0);
			Assert.Equal(string.Empty, websiteGroup0.FullPath);
			var websiteGroup1 = await PortalClient.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath).ConfigureAwait(false);
			Assert.NotNull(websiteGroup1);
			var websiteGroup2 = await PortalClient.GetWebsiteGroupByFullPathAsync(WebsiteGroupFullPath).ConfigureAwait(false);
			Assert.NotNull(websiteGroup2);
			Assert.NotEqual(websiteGroup0.Id, websiteGroup2.Id);
		}

		[Fact]
		public async void GetWebsiteGroupById()
		{
			var websiteGroup = await PortalClient.GetAsync<WebsiteGroup>(1).ConfigureAwait(false);

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
			var websiteMonitorCheckpoints = await PortalClient.GetAllAsync<WebsiteMonitorCheckpoint>().ConfigureAwait(false);

			// Some website folders should be returned
			Assert.True(websiteMonitorCheckpoints.Count > 0);

			//// Website folders should have unique Ids
			//Assert.False(websiteMonitorCheckpoints.Select(c => c.Id).HasDuplicates());
		}

		[Fact]
		public async void CrudWebsiteGroupsAndWebsites()
		{
			// Ensure it doesn't exist
			var oldWebsiteGroup = await PortalClient.GetWebsiteGroupByFullPathAsync("Test Name").ConfigureAwait(false);

			if (oldWebsiteGroup != null)
			{
				await PortalClient.DeleteAsync<WebsiteGroup>(oldWebsiteGroup.Id).ConfigureAwait(false);
			}

			// Create it
			var websiteGroup = await PortalClient.CreateAsync(new WebsiteGroupCreationDto
			{
				Name = "Test Name",
				Description = "Test Description",
				IsAlertingDisabled = false,
				IsMonitoringDisabled = false,
				ParentGroupFullPath = "",
				ParentId = "1",
				Properties = new List<Property>
					{new Property {Name = "name", Value = "value"}},
				TestLocation = new TestLocation { All = true, SmgIds = new List<int> { 1, 2, 4, 3, 5, 6 } }
			}).ConfigureAwait(false);

			try
			{
				// Create the website
				var website = await PortalClient
					.CreateAsync(GetWebsiteCreationDto(websiteGroup.Id, nameof(CrudWebsiteGroupsAndWebsites)))
					.ConfigureAwait(false);

				// Wait to ensure that it was created
				await Task.Delay(1000).ConfigureAwait(false);

				// Delete it
				await PortalClient
					.DeleteAsync<Website>(website.Id)
					.ConfigureAwait(false);
			}
			finally
			{
				// Delete it again
				await PortalClient.DeleteAsync(websiteGroup).ConfigureAwait(false);
			}
		}

		[Fact]
		public async void SetWebsiteCustomProperty()
		{
			var website =
				await PortalClient.GetByNameAsync<Website>(nameof(SetWebsiteCustomProperty)).ConfigureAwait(false)
				?? await PortalClient.CreateAsync(GetWebsiteCreationDto(1, nameof(SetWebsiteCustomProperty))).ConfigureAwait(false);

			const string propertyName = "blah";
			const string value1 = "test1";
			const string value2 = "test2";

			// Set it to an expected value
			await PortalClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value1).ConfigureAwait(false);
			var deviceProperties = await PortalClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			var actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Set it to a different value
			await PortalClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value2).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Set it to null (delete it)
			await PortalClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);

			// This should fail as there is nothing to delete
			var deletionException = await Record.ExceptionAsync(async () => await PortalClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<LogicMonitorApiException>(deletionException);

			var updateException = await Record.ExceptionAsync(async () => await PortalClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Update).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(updateException);

			var createNullException = await Record.ExceptionAsync(async () => await PortalClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Create).ConfigureAwait(false)).ConfigureAwait(false);
			Assert.IsType<InvalidOperationException>(createNullException);

			// Create one without checking
			await PortalClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value1, SetPropertyMode.Create).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value1);
			Assert.Equal(1, actual);

			// Update one without checking
			await PortalClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, value2, SetPropertyMode.Update).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName && dp.Value == value2);
			Assert.Equal(1, actual);

			// Delete one without checking
			await PortalClient.SetWebsiteCustomPropertyAsync(website.Id, propertyName, null, SetPropertyMode.Delete).ConfigureAwait(false);
			deviceProperties = await PortalClient.GetWebsitePropertiesAsync(website.Id).ConfigureAwait(false);
			actual = deviceProperties.Count(dp => dp.Name == propertyName);
			Assert.Equal(0, actual);

			await PortalClient.DeleteAsync(website).ConfigureAwait(false);
		}
	}
}