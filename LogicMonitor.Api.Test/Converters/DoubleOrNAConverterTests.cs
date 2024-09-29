using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.Converters;

public class DoubleOrNAConverterTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
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
		result.Data.Should().HaveElementAt(0, 1.25);
		result.Data.Should().HaveElementAt(1, 1.205);
		result.Data.Should().HaveElementAt(2, 1.2005);
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
		result.Data.Should().HaveElementAt(0, 1.25);
		result.Data.Should().HaveElementAt(1, 1.205);
		result.Data.Should().HaveElementAt(2, 1.2005);
		result.Data.Should().HaveElementAt(3, double.MinValue);
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
		result.Data.Should().HaveElementAt(0, double.MinValue);
		result.Data.Should().HaveElementAt(1, double.MinValue);
		result.Data.Should().HaveElementAt(2, double.MinValue);
		result.Data.Should().HaveElementAt(3, double.MinValue);
	}
}
