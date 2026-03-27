using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

public class NumberOrStringConverterTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public void WhenCalculatedThresholdIsJsonString_DeserializesCorrectly()
	{
		var json = """{"calculatedThreshold": "0.0"}""";
		var result = JsonConvert.DeserializeObject<Collector>(json);
		result.Should().NotBeNull();
		result!.CalculatedThreshold.Should().Be("0.0");
	}

	[Fact]
	public void WhenCalculatedThresholdIsJsonInteger_DeserializesCorrectly()
	{
		var json = """{"calculatedThreshold": 0}""";
		var result = JsonConvert.DeserializeObject<Collector>(json);
		result.Should().NotBeNull();
		result!.CalculatedThreshold.Should().Be("0");
	}

	[Fact]
	public void WhenCalculatedThresholdIsJsonFloat_DeserializesCorrectly()
	{
		var json = """{"calculatedThreshold": 1.5}""";
		var result = JsonConvert.DeserializeObject<Collector>(json);
		result.Should().NotBeNull();
		result!.CalculatedThreshold.Should().Be("1.5");
	}

	[Fact]
	public void WhenCalculatedThresholdIsJsonNull_DeserializesCorrectly()
	{
		var json = """{"calculatedThreshold": null}""";
		var result = JsonConvert.DeserializeObject<Collector>(json);
		result.Should().NotBeNull();
		result!.CalculatedThreshold.Should().BeEmpty();
	}
}
