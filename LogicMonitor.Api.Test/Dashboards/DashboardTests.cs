using LogicMonitor.Api.Test.Extensions;

namespace LogicMonitor.Api.Test.Dashboards;

public class DashboardTests : TestWithOutput
{
	private static readonly DateTimeOffset UtcNow = DateTimeOffset.UtcNow;

	public DashboardTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task GetBigNumberWidgetData_Succeeds()
	{
		// Multi-number
		var widgetData = await LogicMonitorClient
			.GetWidgetDataAsync(626, UtcNow.AddDays(-30), UtcNow, CancellationToken.None)
			.ConfigureAwait(false);
		widgetData.Should().NotBeNull();

		// Single-number
		var widgetData2 = await LogicMonitorClient
			.GetWidgetDataAsync(627, UtcNow.AddDays(-30), UtcNow, CancellationToken.None)
			.ConfigureAwait(false);
		widgetData2.Should().NotBeNull();
	}

	[Fact]
	public async Task GetSlaWidgetData_Succeeds()
	{
		var utcNow = DateTimeOffset.UtcNow;

		// Multi-number
		var widgetData = await LogicMonitorClient
			.GetWidgetDataAsync(631, utcNow.AddDays(-30), utcNow, CancellationToken.None)
			.ConfigureAwait(false);
		widgetData.Should().NotBeNull();
		widgetData.ResultList.Should().NotBeNull();
		widgetData.ResultList.Should().NotBeNullOrEmpty();

		// Single-number
		var widgetData2 = await LogicMonitorClient
			.GetWidgetDataAsync(540, utcNow.AddDays(-30), utcNow, CancellationToken.None)
			.ConfigureAwait(false);
		widgetData2.Should().NotBeNull();
		widgetData2.Availability.Should().NotBe(0);
		widgetData2.ResultList.Should().BeEmpty();
	}

	[Fact]
	public async Task Clone()
	{
		// This one has all the different widget types on
		var originalDashboard = await LogicMonitorClient
			.GetByNameAsync<Dashboard>("All Widgets", CancellationToken.None)
			.ConfigureAwait(false);

		var newDashboard = await LogicMonitorClient.CloneAsync(originalDashboard.Id, new DashboardCloneRequest
		{
			Name = "All widgets clone",
			Description = "I'm a clone and so is my wife.",
			DashboardGroupId = originalDashboard.DashboardGroupId,
			WidgetsConfig = originalDashboard.WidgetsConfig,
			WidgetsOrder = originalDashboard.WidgetsOrder
		}, CancellationToken.None).ConfigureAwait(false);

		var newDashboardRefetch = await LogicMonitorClient.
			GetAsync<Dashboard>(newDashboard.Id, CancellationToken.None)
			.ConfigureAwait(false);

		// Ensure that it is as expected
		newDashboardRefetch.Should().NotBeNull();

		// Delete the clone
		await LogicMonitorClient
			.DeleteAsync(newDashboard, cancellationToken: CancellationToken.None)
			.ConfigureAwait(false);
	}

	[Fact]
	public async Task Get()
	{
		// This one has all the different widget types on
		var dashboard = await LogicMonitorClient
			.GetByNameAsync<Dashboard>("All Widgets", CancellationToken.None)
			.ConfigureAwait(false);
		var widgets = await LogicMonitorClient
			.GetWidgetsByDashboardNameAsync("All Widgets", CancellationToken.None)
			.ConfigureAwait(false);
		dashboard.Should().NotBeNull();
		widgets.Should().NotBeNull();
		widgets.Should().HaveCount(20); // There are 20 different types of widget

		// Test each type

		// AlertsList
		var alertsListWidget = widgets.OfType<AlertWidget>().SingleOrDefault();
		alertsListWidget.Should().NotBeNull();
		string.IsNullOrWhiteSpace(alertsListWidget!.Theme).Should().BeFalse();
		alertsListWidget.AlertFilter.Should().NotBeNull();

		// Job Monitor
		var jobMonitorWidget = widgets.OfType<JobMonitorWidget>().SingleOrDefault();
		jobMonitorWidget.Should().NotBeNull();
		string.IsNullOrWhiteSpace(jobMonitorWidget!.DeviceDisplayName).Should().BeFalse();
		string.IsNullOrWhiteSpace(jobMonitorWidget.DeviceGroupDisplayName).Should().BeFalse();
		string.IsNullOrWhiteSpace(jobMonitorWidget.BatchJobName).Should().BeFalse();
		string.IsNullOrWhiteSpace(jobMonitorWidget.BatchJobId).Should().BeFalse();

		// Big Number
		var bigNumberWidget = widgets.OfType<BigNumberWidget>().FirstOrDefault();
		bigNumberWidget.Should().NotBeNull();
		bigNumberWidget!.BigNumberInfo.Should().NotBeNull();
		bigNumberWidget.BigNumberInfo.DataPoints.Should().NotBeNull();
		bigNumberWidget.BigNumberInfo.VirtualDataPoints.Should().NotBeNull();
		bigNumberWidget.BigNumberInfo.BigNumberItems.Should().NotBeNull();

		// Big Number
		var gaugeWidget = widgets.OfType<GaugeWidget>().SingleOrDefault();
		gaugeWidget.Should().NotBeNull();
		gaugeWidget!.DataPoint.Should().NotBeNull();
		gaugeWidget.DataPoint.DeviceGroupFullPath.Should().NotBeNull();
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
		customGraphWidget!.Theme.Should().NotBeNull();
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
		customGraphWidget.GraphInfo.DataPoints[0].DeviceGroupFullPath.Value.Should().NotBeNull();
		customGraphWidget.GraphInfo.DataPoints[0].DeviceGroupFullPath.IsGlob.Should().BeTrue();
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
		htmlWidget!.HtmlWidgetResources.Should().NotBeNull();
		htmlWidget.HtmlWidgetResources.Should().NotBeNullOrEmpty();
		htmlWidget.HtmlWidgetResources[0].Type.Should().NotBeNull();
		htmlWidget.HtmlWidgetResources[0].Url.Should().NotBeNull();

		// Map
		var googleMapWidget = widgets.OfType<GoogleMapWidget>().SingleOrDefault();
		googleMapWidget.Should().NotBeNull();
		googleMapWidget!.MapPoints.Should().NotBeNull();
		googleMapWidget.MapPoints.Should().NotBeNullOrEmpty();
		googleMapWidget.MapPoints[0].Should().BeOfType<DeviceMapPoint>();
		var deviceMapPoint = googleMapWidget.MapPoints[0] as DeviceMapPoint;
		deviceMapPoint.Should().NotBeNull();
		deviceMapPoint!.Type.Should().Be("device");
		deviceMapPoint.DeviceGroupFullPath.Should().NotBeNull();
		deviceMapPoint.DeviceDisplayName.Should().NotBeNull();
		deviceMapPoint.HasLocation.Should().BeTrue();

		// Device and Website NOC widgets (now combined)
		var nocWidgets = widgets.OfType<NocWidget>().ToList();
		nocWidgets.Should().HaveCount(2);
		var deviceNocWidget = nocWidgets[0];
		deviceNocWidget.Should().NotBeNull();
		deviceNocWidget.Items.Should().NotBeNull();
		deviceNocWidget.Items.Should().NotBeNullOrEmpty();
		deviceNocWidget.Items[0].DeviceGroupFullPath.Should().NotBeNull();
		deviceNocWidget.Items[0].DeviceDisplayName.Should().NotBeNull();
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
		pieChartWidget!.Info.Should().NotBeNull();
		pieChartWidget.Info.Title.Should().NotBeNull();
		pieChartWidget.Info.ShowLabelsAndLines.Should().BeTrue();
		pieChartWidget.Info.MaxVisibleSliceCount.Should().NotBe(0);
		pieChartWidget.Info.GroupRemainingAsOthers.Should().BeTrue();
		pieChartWidget.Info.DataPoints.Should().NotBeNull();
		pieChartWidget.Info.DataPoints.Should().NotBeNullOrEmpty();
		pieChartWidget.Info.DataPoints[0].DeviceGroupFullPath.Should().NotBeNull();
		pieChartWidget.Info.DataPoints[0].DeviceDisplayName.Should().NotBeNull();
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
		websiteIndividualStatusWidget!.WebsiteGroupId.Should().NotBe(0);
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
		deviceSlaStatusWidget!.Metrics.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics.Should().NotBeNullOrEmpty();
		deviceSlaStatusWidget.Metrics[0].DeviceGroupName.Should().NotBeNull();
		deviceSlaStatusWidget.Metrics[0].DeviceName.Should().NotBeNull();
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
		websiteSlaWidget!.Items.Should().NotBeNull();
		websiteSlaWidget.Items.Should().NotBeNullOrEmpty();
		websiteSlaWidget.Items[0].WebsiteGroupName.Should().NotBeNull();
		websiteSlaWidget.Items[0].WebsiteName.Should().NotBeNull();

		// TODO - Table widget

		var textWidget = widgets.OfType<TextWidget>().SingleOrDefault();
		textWidget.Should().NotBeNull();
		textWidget!.Html.Should().NotBeNull();
	}

	[Fact]
	public async Task GetDashboardsNoWidgets()
	{
		var dashboards = await LogicMonitorClient.GetAllAsync<Dashboard>(CancellationToken.None).ConfigureAwait(false);

		// Make sure that some are returned
		dashboards.Should().NotBeEmpty();

		// Make sure that all have Unique Ids
		dashboards.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}

	[Fact]
	public async Task GetDashboardsWithWidgets()
	{
		var dashboards = await LogicMonitorClient.GetAllAsync<Dashboard>(CancellationToken.None).ConfigureAwait(false);

		// Make sure that some are returned
		dashboards.Should().NotBeEmpty();

		// Make sure that all have Unique Ids
		dashboards.Select(c => c.Id).HasDuplicates().Should().BeFalse();
	}
}
