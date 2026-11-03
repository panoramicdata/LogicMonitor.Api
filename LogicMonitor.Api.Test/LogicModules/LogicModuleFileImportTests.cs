namespace LogicMonitor.Api.Test.LogicModules;

// Invalid content means nothing is imported; a 401 here would mean the multipart body was signed wrongly
public class LogicModuleFileImportTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Theory]
	[InlineData(false, "{not json")]
	[InlineData(true, "<not xml")]
	public async Task ImportLogicModuleFile_InvalidContent_IsAuthenticatedAndRejected(bool isXml, string content)
	{
		var act = () => LogicMonitorClient.ImportLogicModuleFileAsync(LogicModuleType.DataSource, content, isXml, null, null, CancellationToken);

		var exception = (await act.Should().ThrowAsync<LogicMonitorApiException>()).Which;
		TestOutputHelper.WriteLine($"{exception.HttpStatusCode}: {exception.Message}");
		exception.HttpStatusCode.Should().NotBe(System.Net.HttpStatusCode.Unauthorized);
		exception.HttpStatusCode.Should().NotBe(System.Net.HttpStatusCode.Forbidden);
	}

	[Fact]
	public Task ImportLogicModuleFile_XmlForPropertySource_IsNotSupported()
	{
		var act = () => LogicMonitorClient.ImportLogicModuleFileAsync(LogicModuleType.PropertySource, "<x/>", true, null, null, CancellationToken);

		return act.Should().ThrowAsync<NotSupportedException>();
	}
}
