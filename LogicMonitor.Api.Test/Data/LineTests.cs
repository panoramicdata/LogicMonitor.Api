using System.Drawing;

namespace LogicMonitor.Api.Test.Data;

public class LineTests
{
	[Fact]
	public void CheckLineColor()
	{
		var line1 = new Line { Color = Color.Blue };
		var line2 = new Line { Color = Color.Blue };

		line2.Color.Should().Be(line1.Color);
	}
}
