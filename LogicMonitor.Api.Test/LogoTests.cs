namespace LogicMonitor.Api.Test;

public class LogoTests : TestWithOutput
{
	public LogoTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task Logos()
	{
		foreach (var imageType in Enum.GetValues(typeof(ImageType)).Cast<ImageType>())
		{
			var buffer = await LogicMonitorClient.GetImageByteArrayAsync(imageType).ConfigureAwait(false);
			var image = SixLabors.ImageSharp.Image.Load(buffer);
			Assert.True(image.Width > 0);
		}
	}
}
