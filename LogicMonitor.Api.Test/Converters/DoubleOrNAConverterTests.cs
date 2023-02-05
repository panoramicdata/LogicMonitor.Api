using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

public class DoubleOrNAConverterTests : TestWithOutput
{
	public DoubleOrNAConverterTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public void ReadJson_DataIsThreeValidDoubles_DeserializesCorrectly()
	{
		var json = """
		{
			"colorName": "Auto",
			"std": 13.280015315021577,
			"min": 23.1096750656,
			"max": 84.1421054,
			"avg": 50.12764521375363,
			"visible": true,
			"color": "EB5E66",
			"legend": "internet.google.com ABCDEFGy",
			"type": "Line",
			"useYMax": false,
			"description": null,
			"label": "UPLINKMBS_71355760_0__",
			"decimal": 0,
			"data": [
				1.25,
				1.205,
				1.2005
			]
		}
		""";

		var result = JsonConvert.DeserializeObject<CustomGraphWidgetDataLine>(json);

		result.Should().NotBeNull();
		result!.Data[0].Should().Be(1.25);
		result!.Data[1].Should().Be(1.205);
		result!.Data[2].Should().Be(1.2005);
	}

	[Fact]
	public void ReadJson_DataIsThreeValidDoublesPlusOneNAString_DeserializesCorrectly()
	{
		var json = """
		{
			"colorName": "Auto",
			"std": 13.280015315021577,
			"min": 23.1096750656,
			"max": 84.1421054,
			"avg": 50.12764521375363,
			"visible": true,
			"color": "EB5E66",
			"legend": "internet.google.com ABCDEFGy",
			"type": "Line",
			"useYMax": false,
			"description": null,
			"label": "UPLINKMBS_71355760_0__",
			"decimal": 0,
			"data": [
				1.25,
				1.205,
				1.2005,
				"N/A"
			]
		}
		""";

		var result = JsonConvert.DeserializeObject<CustomGraphWidgetDataLine>(json);

		result.Should().NotBeNull();
		result!.Data[0].Should().Be(1.25);
		result!.Data[1].Should().Be(1.205);
		result!.Data[2].Should().Be(1.2005);
		result!.Data[3].Should().Be(double.MinValue);
	}

	[Fact]
	public void ReadJson_FourNAStrings_DeserializesCorrectly()
	{
		var json = """
		{
			"colorName": "Auto",
			"std": 13.280015315021577,
			"min": 23.1096750656,
			"max": 84.1421054,
			"avg": 50.12764521375363,
			"visible": true,
			"color": "EB5E66",
			"legend": "internet.google.com ABCDEFGy",
			"type": "Line",
			"useYMax": false,
			"description": null,
			"label": "UPLINKMBS_71355760_0__",
			"decimal": 0,
			"data": [
				"N/A",
				"N/A",
				"N/A",
				"N/A"
			]
		}
		""";

		var result = JsonConvert.DeserializeObject<CustomGraphWidgetDataLine>(json);

		result.Should().NotBeNull();
		result!.Data[0].Should().Be(double.MinValue);
		result!.Data[1].Should().Be(double.MinValue);
		result!.Data[2].Should().Be(double.MinValue);
		result!.Data[3].Should().Be(double.MinValue);
	}
}
