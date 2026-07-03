namespace LogicMonitor.Api.Test.Dashboards;

/// <summary>
/// Lifecycle tests for every widget type, run against a dashboard inside the
/// fixture-managed "NugetTest" root group.  The dashboard is created fresh per
/// theory row and deleted in DisposeAsync, so a failed widget creation never
/// leaks a stale dashboard.  The root group itself is swept by the Fixture.
/// </summary>
public class DashboardLifecycleTests(ITestOutputHelper iTestOutputHelper, Fixture fixture)
	: TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>, IAsyncLifetime
{
	private Dashboard? _dashboard;

	public async ValueTask InitializeAsync()
	{
		_dashboard = await LogicMonitorClient.CreateAsync(new DashboardCreationDto
		{
			Name = "NugetTest-WidgetLifecycle",
			Description = "Widget lifecycle test dashboard — safe to delete",
			GroupId = NugetDashboardGroupId,
		}, CancellationToken.None);
	}

	public async ValueTask DisposeAsync()
	{
		if (_dashboard is not null)
		{
			await LogicMonitorClient.DeleteAsync<Dashboard>(_dashboard.Id, CancellationToken.None);
		}

		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Returns one minimal widget instance per supported widget type.
	/// DashboardId is set to 0 here; the test patches it before creation.
	/// </summary>
	public static TheoryData<string, Widget> GetAllWidgetTypes() => new()
	{
		{ "advancedMetrics", new AdvancedMetricsWidget { Type = "advancedMetrics", LmqlQuery = "SELECT dataPoint FROM resources LIMIT 10" } },
		{ "alert",           new AlertWidget           { Type = "alert" } },
		{ "bigNumber",       new BigNumberWidget        { Type = "bigNumber" } },
		{ "cgraph",          new CustomGraphWidget      { Type = "cgraph" } },
		{ "deviceNoc",       new ResourceNocWidget      { Type = "deviceNoc" } },
		{ "deviceSLA",       new ResourceSlaWidget      { Type = "deviceSLA" } },
		{ "flash",           new FlashWidget            { Type = "flash" } },
		{ "gauge",           new GaugeWidget            { Type = "gauge" } },
		{ "gmap",            new GoogleMapWidget        { Type = "gmap" } },
		{ "html",            new HtmlWidget             { Type = "html" } },
		{ "batchjob",        new JobMonitorWidget       { Type = "batchjob" } },
		{ "netflow",         new NetflowWidget          { Type = "netflow" } },
		{ "ngraph",          new NGraphWidget           { Type = "ngraph" } },
		{ "noc",             new NocWidget              { Type = "noc" } },
		{ "ograph",          new OverviewGraphWidget    { Type = "ograph" } },
		{ "pieChart",        new PieChartWidget         { Type = "pieChart" } },
		{ "websiteIndividualStatus", new WebsiteIndividualStatusWidget { Type = "websiteIndividualStatus" } },
		{ "swebsitenoc",     new WebsiteNocWidget       { Type = "swebsitenoc" } },
		{ "websiteSLA",      new WebsiteSlaWidget       { Type = "websiteSLA" } },
		{ "websiteOverview", new WebsiteOverviewWidget  { Type = "websiteOverview" } },
		{ "websiteOverallStatus", new WebsiteOverallStatusWidget { Type = "websiteOverallStatus" } },
		{ "savedMap",        new SavedMapWidget         { Type = "savedMap" } },
		{ "sgraph",          new WebsiteGraphWidget     { Type = "sgraph" } },
		{ "table",           new TableWidget            { Type = "table" } },
		{ "text",            new TextWidget             { Type = "text",  Html = "Lifecycle test" } },
		{ "netflowgraph",    new NetflowGraphWidget     { Type = "netflowgraph" } },
		{ "groupNetflow",    new GroupNetflowWidget     { Type = "groupNetflow" } },
		{ "dynamicTable",    new DynamicTableWidget     { Type = "dynamicTable" } },
		{ "deviceStatus",    new ResourceStatusWidget   { Type = "deviceStatus" } },
	};

	[Theory]
	[MemberData(nameof(GetAllWidgetTypes))]
	public async Task WidgetLifecycle_CreateDelete_Succeeds(string typeKey, Widget widget)
	{
		widget.DashboardId = _dashboard!.Id;
		widget.Name = $"NugetTest-{typeKey}";

		var created = await LogicMonitorClient.SaveNewWidgetAsync(widget, CancellationToken);

		try
		{
			created.Should().NotBeNull();
			created.GetType().Should().Be(widget.GetType(),
				$"portal must echo back a {widget.GetType().Name} — if NotSupportedException fires, the converter is missing '{typeKey}'");
			created.Name.Should().Be(widget.Name);
			created.DashboardId.Should().Be(_dashboard.Id);
		}
		finally
		{
			if (created?.Id > 0)
			{
				await LogicMonitorClient.DeleteWidgetAsync(created.Id, CancellationToken);
			}
		}
	}
}
