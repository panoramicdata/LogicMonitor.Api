using SixLabors.ImageSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test
{
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
				var image = Image.Load(buffer);
				Assert.True(image.Width > 0);
			}
		}
	}
}