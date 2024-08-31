namespace LogicMonitor.Api.Test.Settings;

public class LogoTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task Logos()
	{
		foreach (var imageType in Enum.GetValues(typeof(ImageType)).Cast<ImageType>())
		{
			var buffer = await LogicMonitorClient.GetImageByteArrayAsync(imageType, default);
			var image = SixLabors.ImageSharp.Image.Load(buffer);
			image.Width.Should().BePositive();
		}
	}
}
