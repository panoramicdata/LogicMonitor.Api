using LogicMonitor.Api.Data;
using System.Drawing;
using Xunit;

namespace LogicMonitor.Api.Test;

public class LineTests
{
	[Fact]
	public void CheckLineColor()
	{
		var line1 = new Line { Color = Color.Blue };
		var line2 = new Line { Color = Color.Blue };

		Assert.Equal(line1.Color, line2.Color);
	}
}
