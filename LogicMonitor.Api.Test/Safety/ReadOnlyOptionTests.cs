namespace LogicMonitor.Api.Test.Safety;

public class ReadOnlyOptionTests
{
	private static LogicMonitorClient CreateClient(LogicMonitorClientOptions options)
		=> new(options);

	[Fact]
	public void IsReadOnly_DefaultsToFalse()
		=> new LogicMonitorClientOptions().IsReadOnly.Should().BeFalse();

	[Fact]
	public async Task PostAsync_WhenReadOnly_ThrowsBeforeSending()
	{
		using var client = CreateClient(new LogicMonitorClientOptions { Account = "readonly-test", AccessId = "id", AccessKey = "key", IsReadOnly = true });

		var act = () => client.PostAsync<EntityProperty, EntityProperty>(new EntityProperty { Name = "n", Value = "v" }, "setting/admins", CancellationToken.None);

		await act.Should().ThrowAsync<InvalidOperationException>();
		client.Statistics.ApiCallPostCount.Should().Be(0);
	}

	[Fact]
	public async Task DeleteAsync_WhenReadOnly_ThrowsBeforeSending()
	{
		using var client = CreateClient(new LogicMonitorClientOptions { Account = "readonly-test", AccessId = "id", AccessKey = "key", IsReadOnly = true });

		var act = () => client.DeleteAsync("device/devices/1", CancellationToken.None);

		await act.Should().ThrowAsync<InvalidOperationException>();
		client.Statistics.ApiCallDeleteCount.Should().Be(0);
	}

	[Fact]
	public async Task PostAsync_WhenIsReadOnlyChangedAfterConstruction_ThrowsBeforeSending()
	{
		var options = new LogicMonitorClientOptions { Account = "readonly-test", AccessId = "id", AccessKey = "key" };
		using var client = CreateClient(options);
		options.IsReadOnly = true;

		var act = () => client.PostAsync<EntityProperty, EntityProperty>(new EntityProperty { Name = "n", Value = "v" }, "setting/admins", CancellationToken.None);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}

	[Theory]
	[InlineData("POST")]
	[InlineData("PUT")]
	[InlineData("PATCH")]
	[InlineData("DELETE")]
	public void Guard_WhenReadOnly_RefusesWrites(string method)
	{
		var act = () => ReadOnlyGuard.ThrowIfWrite(new LogicMonitorClientOptions { IsReadOnly = true }, new HttpMethod(method), "setting/admins");

		act.Should().Throw<InvalidOperationException>();
	}

	[Theory]
	[InlineData("GET")]
	[InlineData("HEAD")]
	[InlineData("OPTIONS")]
	public void Guard_WhenReadOnly_AllowsReads(string method)
	{
		var act = () => ReadOnlyGuard.ThrowIfWrite(new LogicMonitorClientOptions { IsReadOnly = true }, new HttpMethod(method), "setting/admins");

		act.Should().NotThrow();
	}

	[Theory]
	[InlineData("functions")]
	[InlineData("device/instances/datafetch?start=1&end=2")]
	[InlineData("hierarchy/dataSourceInstances")]
	[InlineData("log/search")]
	[InlineData("log/search/abc123")]
	public void Guard_WhenReadOnly_AllowsNamedReadOnlyPosts(string subUrl)
	{
		var act = () => ReadOnlyGuard.ThrowIfWrite(new LogicMonitorClientOptions { IsReadOnly = true }, HttpMethod.Post, subUrl);

		act.Should().NotThrow();
	}

	[Theory]
	[InlineData("setting/functions")]
	[InlineData("functionsX")]
	[InlineData("log/ingest")]
	[InlineData("setting/admins")]
	public void Guard_WhenReadOnly_RefusesOtherPosts(string subUrl)
	{
		var act = () => ReadOnlyGuard.ThrowIfWrite(new LogicMonitorClientOptions { IsReadOnly = true }, HttpMethod.Post, subUrl);

		act.Should().Throw<InvalidOperationException>();
	}

	[Fact]
	public void Guard_WhenNotReadOnly_AllowsWrites()
	{
		var act = () => ReadOnlyGuard.ThrowIfWrite(new LogicMonitorClientOptions(), HttpMethod.Post, "setting/admins");

		act.Should().NotThrow();
	}

	[Fact]
	public async Task ExperimentalHandler_WhenReadOnly_ThrowsBeforeSending()
	{
		using var invoker = new HttpMessageInvoker(new LogicMonitor.Api.Experimental.SecureHttpClientHandler(new LogicMonitorClientOptions { IsReadOnly = true }));

		var act = () => invoker.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "https://readonly-test.invalid/santaba/rest/device/devices/1"), CancellationToken.None);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}
}
