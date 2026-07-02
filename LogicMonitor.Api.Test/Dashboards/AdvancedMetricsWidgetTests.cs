using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Dashboards;

/// <summary>
/// Tests for the Advanced Metrics Widget (LMQL-driven).
/// </summary>
public class AdvancedMetricsWidgetTests(ITestOutputHelper iTestOutputHelper, Fixture fixture)
	: TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	// A minimal, syntactically plausible LMQL query used in CRUD tests.
	// Update this value once you have verified correct LMQL syntax against your portal.
	private const string SampleLmqlQuery = "SELECT dataPoint FROM resources LIMIT 10";

	// ── Unit tests (no portal required) ──────────────────────────────────────

	[Fact]
	public void AdvancedMetricsWidget_Deserializes_FromJson()
	{
		const string json = """
			{
				"type": "advancedMetrics",
				"id": 42,
				"name": "My Advanced Metrics",
				"dashboardId": 7,
				"lmql": "SELECT dataPoint FROM resources LIMIT 10",
				"displaySettings": {}
			}
			""";

		var widget = JsonConvert.DeserializeObject<Widget>(json);

		widget.Should().BeOfType<AdvancedMetricsWidget>();
		var adv = (AdvancedMetricsWidget)widget!;
		adv.Id.Should().Be(42);
		adv.Name.Should().Be("My Advanced Metrics");
		adv.DashboardId.Should().Be(7);
		adv.LmqlQuery.Should().Be("SELECT dataPoint FROM resources LIMIT 10");
	}

	[Fact]
	public void AdvancedMetricsWidget_TypeString_IsCaseInsensitive()
	{
		// The converter calls ToLowerInvariant(), so mixed-case should still work.
		const string json = """
			{
				"type": "AdvancedMetrics",
				"id": 1,
				"name": "Case Test",
				"dashboardId": 1,
				"lmql": "SELECT x FROM y"
			}
			""";

		var widget = JsonConvert.DeserializeObject<Widget>(json);

		widget.Should().BeOfType<AdvancedMetricsWidget>();
	}

	[Fact]
	public void AdvancedMetricsWidgetCreationDto_Type_IsCorrect()
	{
		var dto = new AdvancedMetricsWidgetCreationDto { LmqlQuery = SampleLmqlQuery };
		dto.Type.Should().Be("advancedMetrics");
	}

	// ── Integration tests (require portal) ───────────────────────────────────

	[Fact]
	public async Task CrudAdvancedMetricsWidget_Succeeds()
	{
		const string widgetName = "IntegrationTest-AdvancedMetrics";

		// Clean up any leftover widget from a prior run
		var existing = (await LogicMonitorClient
			.GetWidgetListAsync(new Filter<Widget>(), CancellationToken))
			.Items
			.FirstOrDefault(w => w.Name == widgetName);

		if (existing is not null)
		{
			await LogicMonitorClient.DeleteWidgetAsync(existing.Id, CancellationToken);
		}

		// Create
		var newWidget = new AdvancedMetricsWidget
		{
			Name = widgetName,
			Description = "Integration test — Advanced Metrics Widget",
			Type = "advancedMetrics",
			DashboardId = TestDashboardId,
			LmqlQuery = SampleLmqlQuery,
			UpdateIntervalMinutes = 5,
			Timescale = "30days",
			Theme = "newBorderBlue",
		};

		var created = await LogicMonitorClient.SaveNewWidgetAsync(newWidget, CancellationToken);

		try
		{
			created.Should().BeOfType<AdvancedMetricsWidget>("the portal must echo back an AdvancedMetricsWidget — if this fails with a NotSupportedException the actual type string may differ from 'advancedMetrics'");
			var createdAdv = (AdvancedMetricsWidget)created;
			createdAdv.Name.Should().Be(widgetName);
			createdAdv.DashboardId.Should().Be(TestDashboardId);
			createdAdv.LmqlQuery.Should().NotBeNullOrWhiteSpace();

			// Fetch back by id
			var fetched = await LogicMonitorClient.GetWidgetByIdAsync(created.Id, CancellationToken);

			fetched.Should().BeOfType<AdvancedMetricsWidget>();
			var fetchedAdv = (AdvancedMetricsWidget)fetched;
			fetchedAdv.Name.Should().Be(widgetName);
			fetchedAdv.LmqlQuery.Should().NotBeNullOrWhiteSpace();

			// Update (patch the query)
			fetchedAdv.LmqlQuery = SampleLmqlQuery + " OFFSET 0";
			await LogicMonitorClient.PatchWidgetByIdAsync(fetchedAdv.Id, fetchedAdv, CancellationToken);

			var updated = (AdvancedMetricsWidget)await LogicMonitorClient
				.GetWidgetByIdAsync(fetchedAdv.Id, CancellationToken);
			updated.LmqlQuery.Should().Contain("OFFSET");
		}
		finally
		{
			if (created?.Id > 0)
			{
				await LogicMonitorClient.DeleteWidgetAsync(created.Id, CancellationToken);
			}
		}
	}

	[Fact]
	public async Task GetAllWidgets_ContainsAdvancedMetricsWidget_WhenPresentOnDashboard()
	{
		// This test checks that the "All Widgets" dashboard can be fully retrieved
		// (i.e. WidgetConverter handles advancedMetrics without throwing) if such a
		// widget has been manually added to the dashboard.
		var widgets = await LogicMonitorClient
			.GetWidgetsByDashboardNameAsync("All Widgets", CancellationToken);

		widgets.Should().NotBeNull();

		var advancedMetricsWidgets = widgets!.OfType<AdvancedMetricsWidget>().ToList();
		Logger.LogInformation(
			"Found {Count} AdvancedMetricsWidget(s) on the 'All Widgets' dashboard.",
			advancedMetricsWidgets.Count);

		// If the "All Widgets" dashboard has one, assert its query is populated.
		foreach (var w in advancedMetricsWidgets)
		{
			w.LmqlQuery.Should().NotBeNullOrWhiteSpace(
				"every AdvancedMetricsWidget should have an LMQL query");
		}
	}
}
