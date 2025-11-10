using SkiaSharp;

namespace LogicMonitor.Api.Test.Settings;

public class LogoTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task Logos()
	{
		foreach (var imageType in Enum.GetValues<ImageType>().Cast<ImageType>())
		{
			var buffer = await LogicMonitorClient.GetImageByteArrayAsync(imageType, CancellationToken);
			using var image = SKBitmap.Decode(buffer);
			image.Should().NotBeNull();
			image.Width.Should().BePositive();
		}
	}
}
