namespace LogicMonitor.Api.Test.Dashboards;

public class WidgetTests(ITestOutputHelper iTestOutputHelper) : TestWithOutput(iTestOutputHelper)
{
	private static readonly DateTimeOffset UtcNow = DateTimeOffset.UtcNow;

	[Fact]
	public async Task GetBigNumberWidgetData_Succeeds()
	{
		// Multi-number
		var widgetData = await LogicMonitorClient
			.GetWidgetDataAsync(626, UtcNow.AddDays(-30), UtcNow, default)
			.ConfigureAwait(true);
		widgetData.Should().NotBeNull();

		// Single-number
		var widgetData2 = await LogicMonitorClient
			.GetWidgetDataAsync(627, UtcNow.AddDays(-30), UtcNow, default)
			.ConfigureAwait(true);
		widgetData2.Should().NotBeNull();
	}

	[Fact]
	public async Task GetSlaWidgetData_Succeeds()
	{
		var utcNow = DateTimeOffset.UtcNow;

		// Multi-number
		var widgetData = await LogicMonitorClient
			.GetWidgetDataAsync(631, utcNow.AddDays(-30), utcNow, default)
			.ConfigureAwait(true);
		widgetData.Should().NotBeNull();
		widgetData.ResultList.Should().NotBeNull();
		widgetData.ResultList.Should().NotBeNullOrEmpty();

		// Single-number
		var widgetData2 = await LogicMonitorClient
			.GetWidgetDataAsync(540, utcNow.AddDays(-30), utcNow, default)
			.ConfigureAwait(true);
		widgetData2.Should().NotBeNull();
		widgetData2.Availability.Should().NotBe(0);
		widgetData2.ResultList.Should().BeEmpty();
	}

	[Fact]
	public async Task GetWidgets()
	{

		var widgets = await LogicMonitorClient
			.GetWidgetListAsync(new(), default)
			.ConfigureAwait(true);

		widgets.Should().NotBeNull();

		widgets.Items.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetWidgetById()
	{
		var widget = (await LogicMonitorClient
			.GetWidgetListAsync(new Filter<Widget>(), default)
			.ConfigureAwait(true)).Items[0];

		var getWidget = await LogicMonitorClient
			.GetWidgetByIdAsync(widget.Id, default)
			.ConfigureAwait(true);

		getWidget.Name.Should().Be(widget.Name);
	}


	[Fact]
	public async Task GetAndSaveWidgetById()
	{
		var widget = (await LogicMonitorClient
			.GetWidgetListAsync(new Filter<Widget>(), default)
			.ConfigureAwait(true)).Items[0];

		var getWidget = await LogicMonitorClient
			.GetWidgetByIdAsync(widget.Id, default)
			.ConfigureAwait(true);

		getWidget.Name.Should().Be(widget.Name);

		await LogicMonitorClient
			.PutAsync(getWidget, default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task GetWidgetDataById()
	{
		var widget = (await LogicMonitorClient
			.GetWidgetListAsync(new Filter<Widget>(), default)
			.ConfigureAwait(true)).Items[0];

		var widgetData = await LogicMonitorClient
			.GetWidgetDataByIdAsync(widget.Id, default)
			.ConfigureAwait(true);

		widgetData.Should().NotBeNull();
	}

	[Fact]
	public async Task SaveWidget()
	{
		var widget = new HtmlWidget()
		{
			DashboardId = TestDashboardId,
			Name = "test",
			Description = "test widget",
			Theme = "newBorderBlue",
			UpdateIntervalMinutes = 5,
			Type = "html",
			Timescale = "string",
			HtmlWidgetResources = [new HtmlWidgetResource()
			{
				Type = "html",
				Url = "string"
			}
			]
		};

		var createdWidget = await LogicMonitorClient
			.SaveNewWidgetAsync(widget, default)
			.ConfigureAwait(true);

		var getWidget = (HtmlWidget)await LogicMonitorClient
			.GetWidgetByIdAsync(createdWidget.Id, default)
			.ConfigureAwait(true);

		var patchedWidget = new HtmlWidget()
		{
			DashboardId = getWidget.DashboardId,
			Name = getWidget.Name,
			Description = "Updated test widget",
			Theme = getWidget.Theme,
			UpdateIntervalMinutes = getWidget.UpdateIntervalMinutes,
			Type = getWidget.Type,
			Timescale = getWidget.Timescale,
			HtmlWidgetResources = getWidget.HtmlWidgetResources
		};

		await LogicMonitorClient
			.PatchWidgetByIdAsync(getWidget.Id, patchedWidget, default)
			.ConfigureAwait(true);

		getWidget = (HtmlWidget)await LogicMonitorClient
			.GetWidgetByIdAsync(createdWidget.Id, default)
			.ConfigureAwait(true);

		await LogicMonitorClient
			.DeleteWidgetAsync(createdWidget.Id, default)
			.ConfigureAwait(true);

		getWidget.Name.Should().Be("test");
		getWidget.Description.Should().Be("Updated test widget");
	}
}
