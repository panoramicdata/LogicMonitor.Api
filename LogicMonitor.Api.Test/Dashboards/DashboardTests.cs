using LogicMonitor.Api.Dashboards;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Dashboards;

public class DashboardTests : TestWithOutput
{
	private static readonly DateTimeOffset UtcNow = DateTimeOffset.UtcNow;

	public DashboardTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetBigNumberWidgetData_Succeeds()
	{
		// Multi-number
		var widgetData = await LogicMonitorClient.GetWidgetDataAsync(626, UtcNow.AddDays(-30), UtcNow).ConfigureAwait(false);
		Assert.NotNull(widgetData);

		// Single-number
		var widgetData2 = await LogicMonitorClient.GetWidgetDataAsync(627, UtcNow.AddDays(-30), UtcNow).ConfigureAwait(false);
		Assert.NotNull(widgetData2);
	}

	[Fact]
	public async void GetSlaWidgetData_Succeeds()
	{
		var utcNow = DateTimeOffset.UtcNow;

		// Multi-number
		var widgetData = await LogicMonitorClient.GetWidgetDataAsync(631, utcNow.AddDays(-30), utcNow).ConfigureAwait(false);
		Assert.NotNull(widgetData);
		Assert.NotNull(widgetData.ResultList);
		Assert.NotEmpty(widgetData.ResultList);

		// Single-number
		var widgetData2 = await LogicMonitorClient.GetWidgetDataAsync(540, utcNow.AddDays(-30), utcNow).ConfigureAwait(false);
		Assert.NotNull(widgetData2);
		Assert.NotEqual(0, widgetData2.Availability);
		Assert.Null(widgetData2.ResultList);
	}

	[Fact]
	public async void Clone()
	{
		// This one has all the different widget types on
		var originalDashboard = await LogicMonitorClient.GetByNameAsync<Dashboard>("All Widgets").ConfigureAwait(false);

		var newDashboard = await LogicMonitorClient.CloneAsync(originalDashboard.Id, new DashboardCloneRequest
		{
			Name = "All widgets clone",
			Description = "I'm a clone and so if my wife.",
			DashboardGroupId = originalDashboard.DashboardGroupId,
			WidgetsConfig = originalDashboard.WidgetsConfig,
			WidgetsOrder = originalDashboard.WidgetsOrder
		}).ConfigureAwait(false);

		var newDashboardRefetch = await LogicMonitorClient.
			GetAsync<Dashboard>(newDashboard.Id)
			.ConfigureAwait(false);

		// Ensure that it is as expected
		Assert.NotNull(newDashboardRefetch);

		// Delete the clone
		await LogicMonitorClient
			.DeleteAsync(newDashboard)
			.ConfigureAwait(false);
	}

	[Fact]
	public async void Get()
	{
		// This one has all the different widget types on
		var dashboard = await LogicMonitorClient.GetByNameAsync<Dashboard>("All Widgets").ConfigureAwait(false);
		var widgets = await LogicMonitorClient.GetWidgetsByDashboardNameAsync("All Widgets").ConfigureAwait(false);
		Assert.NotNull(dashboard);
		Assert.NotNull(widgets);
		Assert.Equal(19, widgets.Count); // There are 24 different types of widget

		// Test each type

		// AlertsList
		var alertsListWidget = widgets.OfType<AlertWidget>().SingleOrDefault();
		Assert.NotNull(alertsListWidget);
		Assert.False(string.IsNullOrWhiteSpace(alertsListWidget.Theme));
		Assert.NotNull(alertsListWidget.AlertFilter);

		// Job Monitor
		var jobMonitorWidget = widgets.OfType<JobMonitorWidget>().SingleOrDefault();
		Assert.NotNull(jobMonitorWidget);
		Assert.False(string.IsNullOrWhiteSpace(jobMonitorWidget.DeviceDisplayName));
		Assert.False(string.IsNullOrWhiteSpace(jobMonitorWidget.DeviceGroupDisplayName));
		Assert.False(string.IsNullOrWhiteSpace(jobMonitorWidget.BatchJobName));
		Assert.False(string.IsNullOrWhiteSpace(jobMonitorWidget.BatchJobId));

		// Big Number
		var bigNumberWidget = widgets.OfType<BigNumberWidget>().FirstOrDefault();
		Assert.NotNull(bigNumberWidget);
		Assert.NotNull(bigNumberWidget.BigNumberInfo);
		Assert.NotNull(bigNumberWidget.BigNumberInfo.DataPoints);
		Assert.NotNull(bigNumberWidget.BigNumberInfo.VirtualDataPoints);
		Assert.NotNull(bigNumberWidget.BigNumberInfo.BigNumberItems);

		// Big Number
		var gaugeWidget = widgets.OfType<GaugeWidget>().SingleOrDefault();
		Assert.NotNull(gaugeWidget);
		Assert.NotNull(gaugeWidget.DataPoint);
		Assert.NotNull(gaugeWidget.DataPoint.DeviceGroupFullPath);
		Assert.NotNull(gaugeWidget.DataPoint.DeviceDisplayName);
		Assert.NotNull(gaugeWidget.DataPoint.DataSourceFullName);
		Assert.NotEqual(0, gaugeWidget.DataPoint.DataSourceId);
		Assert.NotNull(gaugeWidget.DataPoint.DataSourceInstanceName);
		Assert.NotNull(gaugeWidget.DataPoint.DataPointName);
		Assert.NotEqual(0, gaugeWidget.DataPoint.DataPointId);
		Assert.NotNull(gaugeWidget.DataPoint.AggregateFunction);
		Assert.NotNull(gaugeWidget.DataPoint.DataSeries);
		Assert.NotNull(gaugeWidget.DataPoint.Rpn);

		// Custom Graph
		var customGraphWidget = widgets.OfType<CustomGraphWidget>().SingleOrDefault();
		Assert.NotNull(customGraphWidget);
		Assert.NotNull(customGraphWidget.Theme);
		Assert.NotNull(customGraphWidget.GraphInfo);
		Assert.NotNull(customGraphWidget.GraphInfo.VerticalLabel);
		Assert.NotEqual(0, customGraphWidget.GraphInfo.MaxValue);
		Assert.NotEqual(0, customGraphWidget.GraphInfo.ScaleUnit);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints);
		Assert.NotEmpty(customGraphWidget.GraphInfo.DataPoints);
		Assert.NotEqual(0, customGraphWidget.GraphInfo.DataPoints[0].Id);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].Name);
		Assert.NotEqual(0, (int)customGraphWidget.GraphInfo.DataPoints[0].ConsolidateFunction);
		Assert.NotEqual(0, customGraphWidget.GraphInfo.DataPoints[0].CustomGraphId);
		Assert.NotEqual(0, customGraphWidget.GraphInfo.DataPoints[0].DataPointId);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].DataPointName);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].AggregateFunction);
		Assert.NotEqual(0, customGraphWidget.GraphInfo.DataPoints[0].DataSourceId);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].DataSourceFullName);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].DeviceDisplayName);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].DeviceDisplayName.Value);
		Assert.True(customGraphWidget.GraphInfo.DataPoints[0].DeviceDisplayName.IsGlob);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].DeviceGroupFullPath.Value);
		Assert.True(customGraphWidget.GraphInfo.DataPoints[0].DeviceGroupFullPath.IsGlob);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].DataSourceInstanceName.Value);
		Assert.True(customGraphWidget.GraphInfo.DataPoints[0].DataSourceInstanceName.IsGlob);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].Display);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].Display.Option);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].Display.Legend);
		Assert.NotEqual(0, (int)customGraphWidget.GraphInfo.DataPoints[0].Display.Type);
		Assert.NotNull(customGraphWidget.GraphInfo.DataPoints[0].Display.Color);
		Assert.NotNull(customGraphWidget.GraphInfo.VirtualDataPoints);
		Assert.NotNull(customGraphWidget.GraphInfo.GlobalConsolidateFunction);

		// Html
		var htmlWidget = widgets.OfType<HtmlWidget>().SingleOrDefault();
		Assert.NotNull(htmlWidget);
		Assert.NotNull(htmlWidget.HtmlWidgetResources);
		Assert.NotEmpty(htmlWidget.HtmlWidgetResources);
		Assert.NotNull(htmlWidget.HtmlWidgetResources[0].Type);
		Assert.NotNull(htmlWidget.HtmlWidgetResources[0].Url);

		// Map
		var googleMapWidget = widgets.OfType<GoogleMapWidget>().SingleOrDefault();
		Assert.NotNull(googleMapWidget);
		Assert.NotNull(googleMapWidget.MapPoints);
		Assert.NotEmpty(googleMapWidget.MapPoints);
		Assert.True(googleMapWidget.MapPoints[0] is DeviceMapPoint);
		var deviceMapPoint = googleMapWidget.MapPoints[0] as DeviceMapPoint;
		Assert.NotNull(deviceMapPoint);
		Assert.Equal("device", deviceMapPoint.Type);
		Assert.NotNull(deviceMapPoint.DeviceGroupFullPath);
		Assert.NotNull(deviceMapPoint.DeviceDisplayName);
		Assert.True(deviceMapPoint.HasLocation);

		// Device and Website NOC widgets (now combined)
		var nocWidgets = widgets.OfType<NocWidget>().ToList();
		Assert.Equal(2, nocWidgets.Count);
		var deviceNocWidget = nocWidgets[0];
		Assert.NotNull(deviceNocWidget);
		Assert.NotNull(deviceNocWidget.Items);
		Assert.NotEmpty(deviceNocWidget.Items);
		Assert.NotNull(deviceNocWidget.Items[0].DeviceGroupFullPath);
		Assert.NotNull(deviceNocWidget.Items[0].DeviceDisplayName);
		Assert.NotNull(deviceNocWidget.Items[0].DataSourceDisplayName);
		Assert.NotNull(deviceNocWidget.Items[0].DataPointName);
		Assert.NotNull(deviceNocWidget.Items[0].GroupBy);

		var websiteNocWidget = nocWidgets.Last();
		Assert.NotNull(websiteNocWidget);
		Assert.NotNull(websiteNocWidget.Items);
		Assert.NotEmpty(websiteNocWidget.Items);
		Assert.NotNull(websiteNocWidget.Items[0].WebsiteGroupName);
		Assert.NotNull(websiteNocWidget.Items[0].WebsiteName);
		Assert.NotNull(websiteNocWidget.Items[0].GroupBy);

		// Pie Chart
		var pieChartWidget = widgets.OfType<PieChartWidget>().SingleOrDefault();
		Assert.NotNull(pieChartWidget);
		Assert.NotNull(pieChartWidget.Info);
		Assert.NotNull(pieChartWidget.Info.Title);
		Assert.True(pieChartWidget.Info.ShowLabelsAndLines);
		Assert.NotEqual(0, pieChartWidget.Info.MaxVisibleSliceCount);
		Assert.True(pieChartWidget.Info.GroupRemainingAsOthers);
		Assert.NotNull(pieChartWidget.Info.DataPoints);
		Assert.NotEmpty(pieChartWidget.Info.DataPoints);
		Assert.NotNull(pieChartWidget.Info.DataPoints[0].DeviceGroupFullPath);
		Assert.NotNull(pieChartWidget.Info.DataPoints[0].DeviceDisplayName);
		Assert.NotNull(pieChartWidget.Info.DataPoints[0].DataSourceFullName);
		Assert.NotEqual(0, pieChartWidget.Info.DataPoints[0].DataSourceId);
		Assert.NotNull(pieChartWidget.Info.DataPoints[0].DataSourceInstanceName);
		Assert.NotNull(pieChartWidget.Info.DataPoints[0].DataPointName);
		Assert.NotEqual(0, pieChartWidget.Info.DataPoints[0].DataPointId);
		Assert.NotNull(pieChartWidget.Info.DataPoints[0].Name);
		Assert.False(pieChartWidget.Info.DataPoints[0].Top10);
		Assert.True(pieChartWidget.Info.DataPoints[0].Aggregate);
		Assert.NotNull(pieChartWidget.Info.DataPoints[0].AggregateFunction);
		Assert.NotNull(pieChartWidget.Info.VirtualDataPoints);
		Assert.Empty(pieChartWidget.Info.VirtualDataPoints);
		Assert.NotNull(pieChartWidget.Info.Items);
		Assert.NotEmpty(pieChartWidget.Info.Items);
		Assert.NotNull(pieChartWidget.Info.Items[0].DataPointName);
		Assert.NotNull(pieChartWidget.Info.Items[0].Legend);
		Assert.NotNull(pieChartWidget.Info.Items[0].Color);

		var websiteIndividualStatusWidget = widgets.OfType<WebsiteIndividualStatusWidget>().SingleOrDefault();
		Assert.NotNull(websiteIndividualStatusWidget);
		Assert.NotEqual(0, websiteIndividualStatusWidget.WebsiteGroupId);
		Assert.NotEqual(0, websiteIndividualStatusWidget.WebsiteId);
		Assert.NotNull(websiteIndividualStatusWidget.GraphName);
		Assert.NotNull(websiteIndividualStatusWidget.WebsiteName);
		Assert.NotNull(websiteIndividualStatusWidget.Locations);
		Assert.NotEmpty(websiteIndividualStatusWidget.Locations);
		Assert.False(websiteIndividualStatusWidget.IsInternal);

		// Website overall status widget
		var websiteOverallStatusWidget = widgets.OfType<WebsiteOverallStatusWidget>().SingleOrDefault();
		Assert.NotNull(websiteOverallStatusWidget);
		//Assert.NotNull(websiteOverallStatusWidget.SelectedWebsites);
		//Assert.NotEmpty(websiteOverallStatusWidget.SelectedWebsites);
		//Assert.Equal(0, websiteOverallStatusWidget.SelectedWebsites[0].WebsiteGroupId);
		//Assert.NotNull(websiteOverallStatusWidget.SelectedWebsites[0].WebsiteGroupName);
		//Assert.False(websiteOverallStatusWidget.SelectedWebsites[0].AreAllChosen);

		// Device SLA widget
		var deviceSlaStatusWidget = widgets.OfType<DeviceSlaWidget>().FirstOrDefault();
		Assert.NotNull(deviceSlaStatusWidget);
		Assert.NotNull(deviceSlaStatusWidget.Metrics);
		Assert.NotEmpty(deviceSlaStatusWidget.Metrics);
		Assert.NotNull(deviceSlaStatusWidget.Metrics[0].DeviceGroupName);
		Assert.NotNull(deviceSlaStatusWidget.Metrics[0].DeviceName);
		Assert.NotEqual(0, deviceSlaStatusWidget.Metrics[0].DataSourceId);
		Assert.NotNull(deviceSlaStatusWidget.Metrics[0].DataSourceFullName);
		Assert.NotNull(deviceSlaStatusWidget.Metrics[0].DataSourceInstances);
		Assert.NotNull(deviceSlaStatusWidget.Metrics[0].Metric);
		Assert.NotNull(deviceSlaStatusWidget.Metrics[0].Threshold);
		// Assert.NotNull(deviceSlaStatusWidget.Metrics[0].UnitLabel);
		Assert.NotNull(deviceSlaStatusWidget.Metrics[0].BottomLabel);
		Assert.NotNull(deviceSlaStatusWidget.Metrics[0].ExclusionSdtType);
		Assert.NotNull(deviceSlaStatusWidget.DaysInWeek);
		Assert.NotNull(deviceSlaStatusWidget.PeriodInOneDay);
		Assert.Equal(0, deviceSlaStatusWidget.UnmonitoredTimeType);
		Assert.Equal(2, deviceSlaStatusWidget.DisplayType);
		// Assert.NotNull(deviceSlaStatusWidget.UnitLabel);
		Assert.NotNull(deviceSlaStatusWidget.BottomLabel);

		// website SLA widget
		var websiteSlaWidget = widgets.OfType<WebsiteSlaWidget>().SingleOrDefault();
		Assert.NotNull(websiteSlaWidget);
		Assert.NotNull(websiteSlaWidget.Items);
		Assert.NotEmpty(websiteSlaWidget.Items);
		Assert.NotNull(websiteSlaWidget.Items[0].WebsiteGroupName);
		Assert.NotNull(websiteSlaWidget.Items[0].WebsiteName);

		// TODO - Table widget

		var textWidget = widgets.OfType<TextWidget>().SingleOrDefault();
		Assert.NotNull(textWidget);
		Assert.NotNull(textWidget.Html);
	}
}
