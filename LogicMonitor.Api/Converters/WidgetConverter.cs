namespace LogicMonitor.Api.Converters;

internal class WidgetConverter : JsonCreationConverter<Widget>
{
	protected override Widget Create(Type objectType, JObject jObject)
	{
		var type = jObject["type"]?.Value<string>()?.ToLowerInvariant();
		return type switch
		{
			"alert" => new AlertWidget(),
			"bignumber" => new BigNumberWidget(),
			"cgraph" => new CustomGraphWidget(),
			"devicenoc" => new ResourceNocWidget(),
			"devicesla" => new ResourceSlaWidget(),
			"flash" => new FlashWidget(),
			"gauge" => new GaugeWidget(),
			"gmap" => new GoogleMapWidget(),
			"html" => new HtmlWidget(),
			"batchjob" => new JobMonitorWidget(),
			"netflow" => new NetflowWidget(),
			"ngraph" => new NGraphWidget(),
			"noc" => new NocWidget(),
			"ograph" => new OverviewGraphWidget(),
			"piechart" => new PieChartWidget(),
			"websiteindividualstatus" => new WebsiteIndividualStatusWidget(),
			"swebsitenoc" => new WebsiteNocWidget(),
			"websitesla" => new WebsiteSlaWidget(),
			"websiteoverview" => new WebsiteOverviewWidget(),
			"websiteoverallstatus" => new WebsiteOverallStatusWidget(),
			"savedmap" => new SavedMapWidget(),
			"sgraph" => new WebsiteGraphWidget(),
			"table" => new TableWidget(),
			"text" => new TextWidget(),
			"netflowgraph" => new NetflowGraphWidget(),
			"groupnetflow" => new GroupNetflowWidget(),
			"dynamictable" => new DynamicTableWidget(),
			"devicestatus" => new ResourceStatusWidget(),
			_ => throw new NotSupportedException($"{nameof(WidgetConverter)}.cs needs updating to include {type} widgets."),
		};
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		=> throw new NotSupportedException();
}
