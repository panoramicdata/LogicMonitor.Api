using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Dashboards;

public class DashboardTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task Clone()
	{
		// This one has all the different widget types on
		var originalDashboard = await LogicMonitorClient
			.GetByNameAsync<Dashboard>("All Widgets", default)
			.ConfigureAwait(true);

		originalDashboard ??= new();

		var newDashboard = await LogicMonitorClient.CloneAsync(originalDashboard.Id, new DashboardCloneRequest
		{
			Name = "All widgets clone",
			Description = "I'm a clone and so is my wife.",
			DashboardGroupId = originalDashboard.DashboardGroupId,
			//WidgetsConfig = originalDashboard.WidgetsConfig,
			WidgetsOrder = originalDashboard.WidgetsOrder
		}, default).ConfigureAwait(true);

		var newDashboardRefetch = await LogicMonitorClient.
			GetAsync<Dashboard>(newDashboard.Id, default)
			.ConfigureAwait(true);

		// Ensure that it is as expected
		newDashboardRefetch.Should().NotBeNull();

		// Delete the clone
		await LogicMonitorClient
			.DeleteAsync(newDashboard, cancellationToken: default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task Get()
	{
		// This one has all the different widget types on
		var dashboard = await LogicMonitorClient
			.GetByNameAsync<Dashboard>("All Widgets", default)
			.ConfigureAwait(true);
		var widgets = await LogicMonitorClient
			.GetWidgetsByDashboardNameAsync("All Widgets", default)
			.ConfigureAwait(true);
		dashboard.Should().NotBeNull();
		widgets.Should().NotBeNull();
		widgets.Should().HaveCount(20); // There are 20 different types of widget
		widgets ??= [];
		// Test each type

		// AlertsList
		var alertsListWidget = widgets.OfType<AlertWidget>().SingleOrDefault();
		alertsListWidget.Should().NotBeNull();
		alertsListWidget.Theme.Should().NotBeNullOrWhiteSpace();
		alertsListWidget.AlertFilter.Should().NotBeNull();

		// Job Monitor
		var jobMonitorWidget = widgets.OfType<JobMonitorWidget>().SingleOrDefault();
		jobMonitorWidget.Should().NotBeNull();
		jobMonitorWidget.ResourceDisplayName.Should().NotBeNullOrWhiteSpace();
		jobMonitorWidget.ResourceGroupDisplayName.Should().NotBeNullOrWhiteSpace();
		jobMonitorWidget.BatchJobName.Should().NotBeNullOrWhiteSpace();
		jobMonitorWidget.BatchJobId.Should().NotBeNullOrWhiteSpace();

		// Big Number
		var bigNumberWidget = widgets.OfType<BigNumberWidget>().FirstOrDefault();
		bigNumberWidget.Should().NotBeNull();
		bigNumberWidget.BigNumberInfo.Should().NotBeNull();
		bigNumberWidget.BigNumberInfo.DataPoints.Should().NotBeNull();
		bigNumberWidget.BigNumberInfo.VirtualDataPoints.Should().NotBeNull();
		bigNumberWidget.BigNumberInfo.BigNumberItems.Should().NotBeNull();

		// Big Number
		var gaugeWidget = widgets.OfType<GaugeWidget>().SingleOrDefault();
		gaugeWidget.Should().NotBeNull();
		gaugeWidget.DataPoint.Should().NotBeNull();
		gaugeWidget.DataPoint.ResourceGroupFullPath.Should().NotBeNull();
		gaugeWidget.DataPoint.DeviceDisplayName.Should().NotBeNull();
		gaugeWidget.DataPoint.DataSourceFullName.Should().NotBeNull();
		gaugeWidget.DataPoint.DataSourceId.Should().NotBe(0);
		gaugeWidget.DataPoint.DataSourceInstanceName.Should().NotBeNull();
		gaugeWidget.DataPoint.DataPointName.Should().NotBeNull();
		gaugeWidget.DataPoint.DataPointId.Should().NotBe(0);
		gaugeWidget.DataPoint.AggregateFunction.Should().NotBeNull();
		gaugeWidget.DataPoint.DataSeries.Should().NotBeNull();
		gaugeWidget.DataPoint.Rpn.Should().NotBeNull();

		// Custom Graph
		var customGraphWidget = widgets.OfType<CustomGraphWidget>().SingleOrDefault();
		customGraphWidget.Should().NotBeNull();
		customGraphWidget.Theme.Should().NotBeNull();
		customGraphWidget.GraphInfo.Should().NotBeNull();
		customGraphWidget.GraphInfo.VerticalLabel.Should().NotBeNull();
		customGraphWidget.GraphInfo.MaxValue.Should().NotBe(0);
		customGraphWidget.GraphInfo.ScaleUnit.Should().NotBe(0);
		customGraphWidget.GraphInfo.DataPoints.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints.Should().NotBeNullOrEmpty();
		customGraphWidget.GraphInfo.DataPoints[0].Id.Should().NotBe(0);
		customGraphWidget.GraphInfo.DataPoints[0].Name.Should().NotBeNull();
		((int)customGraphWidget.GraphInfo.DataPoints[0].ConsolidateFunction).Should().NotBe(0);
		customGraphWidget.GraphInfo.DataPoints[0].CustomGraphId.Should().NotBe(0);
		customGraphWidget.GraphInfo.DataPoints[0].DataPointId.Should().NotBe(0);
		customGraphWidget.GraphInfo.DataPoints[0].DataPointName.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].AggregateFunction.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].DataSourceId.Should().NotBe(0);
		customGraphWidget.GraphInfo.DataPoints[0].DataSourceFullName.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].DeviceDisplayName.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].DeviceDisplayName.Value.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].DeviceDisplayName.IsGlob.Should().BeTrue();
		customGraphWidget.GraphInfo.DataPoints[0].ResourceGroupFullPath.Value.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].ResourceGroupFullPath.IsGlob.Should().BeTrue();
		customGraphWidget.GraphInfo.DataPoints[0].DataSourceInstanceName.Value.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].DataSourceInstanceName.IsGlob.Should().BeTrue();
		customGraphWidget.GraphInfo.DataPoints[0].Display.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].Display.Option.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].Display.Legend.Should().NotBeNull();
		((int)customGraphWidget.GraphInfo.DataPoints[0].Display.Type).Should().NotBe(0);
		customGraphWidget.GraphInfo.DataPoints[0].Display.Color.Should().NotBeNull();
		customGraphWidget.GraphInfo.VirtualDataPoints.Should().NotBeNull();
		customGraphWidget.GraphInfo.GlobalConsolidateFunction.Should().NotBeNull();

		// Html
		var htmlWidget = widgets.OfType<HtmlWidget>().SingleOrDefault();
		htmlWidget.Should().NotBeNull();
		htmlWidget.HtmlWidgetResources.Should().NotBeNull();
		htmlWidget.HtmlWidgetResources.Should().NotBeNullOrEmpty();
		htmlWidget.HtmlWidgetResources[0].Type.Should().NotBeNull();
		htmlWidget.HtmlWidgetResources[0].Url.Should().NotBeNull();

		// Map
		var googleMapWidget = widgets.OfType<GoogleMapWidget>().SingleOrDefault();
		googleMapWidget.Should().NotBeNull();
		googleMapWidget.MapPoints.Should().NotBeNull();
		googleMapWidget.MapPoints.Should().NotBeNullOrEmpty();
		googleMapWidget.MapPoints[0].Should().BeOfType<ResourceMapPoint>();
		var deviceMapPoint = googleMapWidget.MapPoints[0] as ResourceMapPoint;
		deviceMapPoint.Should().NotBeNull();
		deviceMapPoint.Type.Should().Be("device");
		deviceMapPoint.ResourceGroupFullPath.Should().NotBeNull();
		deviceMapPoint.ResourceDisplayName.Should().NotBeNull();
		deviceMapPoint.HasLocation.Should().BeTrue();

		// Device and Website NOC widgets (now combined)
		var nocWidgets = widgets.OfType<NocWidget>().ToList();
		nocWidgets.Should().HaveCount(2);
		var deviceNocWidget = nocWidgets[0];
		deviceNocWidget.Should().NotBeNull();
		deviceNocWidget.Items.Should().NotBeNull();
		deviceNocWidget.Items.Should().NotBeNullOrEmpty();
		deviceNocWidget.Items[0].ResourceGroupFullPath.Should().NotBeNull();
		deviceNocWidget.Items[0].ResourceDisplayName.Should().NotBeNull();
		deviceNocWidget.Items[0].DataSourceDisplayName.Should().NotBeNull();
		deviceNocWidget.Items[0].DataPointName.Should().NotBeNull();
		deviceNocWidget.Items[0].GroupBy.Should().NotBeNull();

		var websiteNocWidget = nocWidgets.Last();
		websiteNocWidget.Should().NotBeNull();
		websiteNocWidget.Items.Should().NotBeNull();
		websiteNocWidget.Items.Should().NotBeNullOrEmpty();
		websiteNocWidget.Items[0].WebsiteGroupName.Should().NotBeNull();
		websiteNocWidget.Items[0].WebsiteName.Should().NotBeNull();
		websiteNocWidget.Items[0].GroupBy.Should().NotBeNull();

		// Pie Chart
		var pieChartWidget = widgets.OfType<PieChartWidget>().SingleOrDefault();
		pieChartWidget.Should().NotBeNull();
		pieChartWidget.Info.Should().NotBeNull();
		pieChartWidget.Info.Title.Should().NotBeNull();
		pieChartWidget.Info.ShowLabelsAndLines.Should().BeTrue();
		pieChartWidget.Info.MaxVisibleSliceCount.Should().NotBe(0);
		pieChartWidget.Info.GroupRemainingAsOthers.Should().BeTrue();
		pieChartWidget.Info.DataPoints.Should().NotBeNull();
		pieChartWidget.Info.DataPoints.Should().NotBeNullOrEmpty();
		pieChartWidget.Info.DataPoints[0].ResourceGroupFullPath.Should().NotBeNull();
		pieChartWidget.Info.DataPoints[0].ResourceDisplayName.Should().NotBeNull();
		pieChartWidget.Info.DataPoints[0].DataSourceFullName.Should().NotBeNull();
		pieChartWidget.Info.DataPoints[0].DataSourceId.Should().NotBe(0);
		pieChartWidget.Info.DataPoints[0].DataSourceInstanceName.Should().NotBeNull();
		pieChartWidget.Info.DataPoints[0].DataPointName.Should().NotBeNull();
		pieChartWidget.Info.DataPoints[0].DataPointId.Should().NotBe(0);
		pieChartWidget.Info.DataPoints[0].Name.Should().NotBeNull();
		pieChartWidget.Info.DataPoints[0].Top10.Should().BeFalse();
		pieChartWidget.Info.DataPoints[0].Aggregate.Should().BeTrue();
		pieChartWidget.Info.DataPoints[0].AggregateFunction.Should().NotBeNull();
		pieChartWidget.Info.VirtualDataPoints.Should().NotBeNull();
		pieChartWidget.Info.VirtualDataPoints.Should().BeEmpty();
		pieChartWidget.Info.Items.Should().NotBeNull();
		pieChartWidget.Info.Items.Should().NotBeNullOrEmpty();
		pieChartWidget.Info.Items[0].DataPointName.Should().NotBeNull();
		pieChartWidget.Info.Items[0].Legend.Should().NotBeNull();
		pieChartWidget.Info.Items[0].Color.Should().NotBeNull();

		var websiteIndividualStatusWidget = widgets.OfType<WebsiteIndividualStatusWidget>().SingleOrDefault();
		websiteIndividualStatusWidget.Should().NotBeNull();
		websiteIndividualStatusWidget.WebsiteGroupId.Should().NotBe(0);
		websiteIndividualStatusWidget.WebsiteId.Should().NotBe(0);
		websiteIndividualStatusWidget.GraphName.Should().NotBeNull();
		websiteIndividualStatusWidget.WebsiteName.Should().NotBeNull();
		websiteIndividualStatusWidget.Locations.Should().NotBeNull();
		websiteIndividualStatusWidget.Locations.Should().NotBeNullOrEmpty();
		websiteIndividualStatusWidget.IsInternal.Should().BeFalse();

		// Website overall status widget
		var websiteOverallStatusWidget = widgets.OfType<WebsiteOverallStatusWidget>().SingleOrDefault();
		websiteOverallStatusWidget.Should().NotBeNull();

		// Device SLA widget
		var deviceSlaStatusWidget = widgets.OfType<DeviceSlaWidget>().FirstOrDefault();
		deviceSlaStatusWidget.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics.Should().NotBeNullOrEmpty();
		deviceSlaStatusWidget.Metrics[0].ResourceGroupName.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics[0].ResourceDisplayName.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics[0].DataSourceId.Should().NotBe(0);
		deviceSlaStatusWidget.Metrics[0].DataSourceFullName.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics[0].DataSourceInstances.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics[0].Metric.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics[0].Threshold.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics[0].BottomLabel.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics[0].ExclusionSdtType.Should().NotBeNull();
		deviceSlaStatusWidget.DaysInWeek.Should().NotBeNull();
		deviceSlaStatusWidget.PeriodInOneDay.Should().NotBeNull();
		deviceSlaStatusWidget.UnmonitoredTimeType.Should().Be(0);
		deviceSlaStatusWidget.DisplayType.Should().Be(2);
		deviceSlaStatusWidget.BottomLabel.Should().NotBeNull();

		// website SLA widget
		var websiteSlaWidget = widgets.OfType<WebsiteSlaWidget>().SingleOrDefault();
		websiteSlaWidget.Should().NotBeNull();
		websiteSlaWidget.Items.Should().NotBeNull();
		websiteSlaWidget.Items.Should().NotBeNullOrEmpty();
		websiteSlaWidget.Items[0].WebsiteGroupName.Should().NotBeNull();
		websiteSlaWidget.Items[0].WebsiteName.Should().NotBeNull();

		// TODO - Table widget

		var textWidget = widgets.OfType<TextWidget>().SingleOrDefault();
		textWidget.Should().NotBeNull();
		textWidget.Html.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDashboardsNoWidgets()
	{
		var dashboards = await LogicMonitorClient
			.GetAllAsync<Dashboard>(default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		dashboards.Should().NotBeEmpty();

		// Make sure that all have Unique Ids
		dashboards.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task GetDashboardsWithWidgets()
	{
		var dashboards = await LogicMonitorClient
			.GetAllAsync<Dashboard>(default)
			.ConfigureAwait(true);

		// Make sure that some are returned
		dashboards.Should().NotBeEmpty();

		// Make sure that all have Unique Ids
		dashboards.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task CreateAndDeleteDashboard()
	{
		var fetchedDashboards = await LogicMonitorClient
			.GetChildDashboardsAsync(1, new Filter<Dashboard>(), default)
			.ConfigureAwait(true);

		var found = false;
		var foundBoard = new Dashboard();

		foreach (Dashboard dashboard in fetchedDashboards.Items)
		{
			if (dashboard.Description.Equals("Test dashboard - will be deleted", StringComparison.Ordinal))
			{
				found = true;
				foundBoard = dashboard;
			}
		}

		if (found)
		{
			await LogicMonitorClient
			.DeleteAsync($"dashboard/dashboards/{foundBoard.Id}", default)
			.ConfigureAwait(true);
		}

		var newDashboard = new DashboardCreationDto()
		{
			Owner = "test",
			Template = new(),
			GroupId = 1,
			Description = "Test dashboard - will be deleted",
			Shareable = true,
			GroupName = "panoramicdata",
			Name = "test dashboard"
		};

		await LogicMonitorClient
			.AddDashboardAsync(newDashboard, default)
			.ConfigureAwait(true);

		var refetchedDashboards = await LogicMonitorClient
			.GetChildDashboardsAsync(1, new Filter<Dashboard>(), default)
			.ConfigureAwait(true);

		found = false;
		foundBoard = new Dashboard();

		foreach (Dashboard dashboard in refetchedDashboards.Items)
		{
			if (dashboard.Description.Equals("Test dashboard - will be deleted", StringComparison.Ordinal))
			{
				found = true;
				foundBoard = dashboard;
			}
		}

		if (found)
		{
			await LogicMonitorClient
			.DeleteAsync($"dashboard/dashboards/{foundBoard.Id}", default)
			.ConfigureAwait(true);
		}

		found.Should().Be(true);
	}
}
