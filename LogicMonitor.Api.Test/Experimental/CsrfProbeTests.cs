using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;

namespace LogicMonitor.Api.Test.Experimental;

/// <summary>
/// Probes /santaba/rest/functions/dummy - the endpoint that mints a JSESSIONID + X-CSRF-Token pair,
/// and checks what that anonymous pair can (and cannot) reach.
/// Diagnostic only; not part of the normal suite's assertions.
/// </summary>
public class CsrfProbeTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private TestPortalConfig Config => Fixture.GetService<IOptions<TestPortalConfig>>().Value;

	[Fact]
	public async Task Probe_FunctionsDummy()
	{
		var config = Config;
		var account = config.Account;

		// 1. Authenticated (LMv1) with X-CSRF-Token: Fetch
		await ProbeAsync(account, config, "authenticated + Fetch", authenticate: true, fetchHeader: true);

		// 2. Authenticated, no Fetch header
		await ProbeAsync(account, config, "authenticated, no Fetch header", authenticate: true, fetchHeader: false);

		// 3. UNAUTHENTICATED with Fetch header - does it hand a token to anyone?
		await ProbeAsync(account, config, "UNAUTHENTICATED + Fetch", authenticate: false, fetchHeader: true);
	}

	[Fact]
	public async Task Probe_AnonymousSessionAccessAndV4()
	{
		var config = Config;
		var account = config.Account;
		var cookies = new CookieContainer();
		using var handler = new HttpClientHandler { CookieContainer = cookies, UseCookies = true };
		using var client = new HttpClient(handler);

		// Bootstrap: anonymous dummy call to get JSESSIONID + CSRF token
		var dummyUri = new Uri($"https://{account}.logicmonitor.com/santaba/rest/functions/dummy");
		using var bootstrap = new HttpRequestMessage(HttpMethod.Get, dummyUri);
		bootstrap.Headers.Add("X-CSRF-Token", "Fetch");
		bootstrap.Headers.Add("X-version", "3");
		using var bootstrapResponse = await client.SendAsync(bootstrap, CancellationToken);
		var token = bootstrapResponse.Headers.TryGetValues("x-csrf-token", out var v) ? v.FirstOrDefault() : null;
		TestOutputHelper.WriteLine($"Bootstrap: {(int)bootstrapResponse.StatusCode}, token present: {token is not null}, cookies: {cookies.GetCookies(dummyUri).Count}");

		// (a) Try to read data with ONLY the anonymous session + token (no Authorization)
		foreach (var (label, subUrl, xVersion) in new[]
		{
			("anon session, v3 devices", "device/devices?size=1", "3"),
			("anon session, v4 devices", "device/devices?size=1", "4"),
			("anon session, setting/admins", "setting/admins?size=1", "3"),
		})
		{
			using var req = new HttpRequestMessage(HttpMethod.Get, $"https://{account}.logicmonitor.com/santaba/rest/{subUrl}");
			req.Headers.Add("X-version", xVersion);
			req.Headers.Add("X-Requested-With", "XMLHttpRequest");
			if (token is not null)
			{
				req.Headers.Add("X-CSRF-Token", token);
			}

			using var res = await client.SendAsync(req, CancellationToken);
			var body = await res.Content.ReadAsStringAsync(CancellationToken);
			TestOutputHelper.WriteLine($"[{label}] {(int)res.StatusCode} {res.StatusCode} :: {body[..Math.Min(200, body.Length)]}");
		}

		// (b) LMv1 API token against X-Version 4
		foreach (var (label, subUrl, xVersion) in new[]
		{
			("LMv1 v3 devices", "device/devices?size=1", "3"),
			("LMv1 v4 devices", "device/devices?size=1", "4"),
		})
		{
			using var handler2 = new HttpClientHandler();
			using var client2 = new HttpClient(handler2);
			var path = subUrl.Contains('?') ? subUrl[..subUrl.IndexOf('?')] : subUrl;
			var epoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
			using var req = new HttpRequestMessage(HttpMethod.Get, $"https://{account}.logicmonitor.com/santaba/rest/{subUrl}");
			req.Headers.Add("X-version", xVersion);
			req.Headers.Add("X-Requested-With", "XMLHttpRequest");
			req.Headers.Add("Authorization", config.AccessKey.StartsWith("lmb_", StringComparison.Ordinal)
				? $"Bearer {config.AccessKey}"
				: $"LMv1 {config.AccessId}:{Api.LogicMonitorClient.GetSignature("GET", epoch, string.Empty, $"/{path}", config.AccessKey)}:{epoch}");
			using var res = await client2.SendAsync(req, CancellationToken);
			var body = await res.Content.ReadAsStringAsync(CancellationToken);
			TestOutputHelper.WriteLine($"[{label}] {(int)res.StatusCode} {res.StatusCode} :: {body[..Math.Min(200, body.Length)]}");
		}
	}

	private async Task ProbeAsync(string account, TestPortalConfig config, string label, bool authenticate, bool fetchHeader)
	{
		var cookies = new CookieContainer();
		using var handler = new HttpClientHandler { CookieContainer = cookies, UseCookies = true };
		using var client = new HttpClient(handler);

		const string subUrl = "functions/dummy";
		var uri = new Uri($"https://{account}.logicmonitor.com/santaba/rest/{subUrl}");
		using var request = new HttpRequestMessage(HttpMethod.Get, uri);
		request.Headers.Add("X-Requested-With", "XMLHttpRequest");
		request.Headers.Add("X-version", "3");
		if (fetchHeader)
		{
			request.Headers.Add("X-CSRF-Token", "Fetch");
		}

		if (authenticate)
		{
			var epoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
			var authHeaderValue = config.AccessKey.StartsWith("lmb_", StringComparison.Ordinal)
				? $"Bearer {config.AccessKey}"
				: $"LMv1 {config.AccessId}:{Api.LogicMonitorClient.GetSignature("GET", epoch, string.Empty, $"/{subUrl}", config.AccessKey)}:{epoch}";
			request.Headers.Add("Authorization", authHeaderValue);
		}

		using var response = await client.SendAsync(request, CancellationToken);

		TestOutputHelper.WriteLine($"=== {label} ===");
		TestOutputHelper.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
		foreach (var header in response.Headers)
		{
			TestOutputHelper.WriteLine($"  H {header.Key}: {string.Join(", ", header.Value)}");
		}

		foreach (var header in response.Content.Headers)
		{
			TestOutputHelper.WriteLine($"  C {header.Key}: {string.Join(", ", header.Value)}");
		}

		foreach (Cookie cookie in cookies.GetCookies(uri))
		{
			TestOutputHelper.WriteLine($"  Cookie {cookie.Name} = {(cookie.Name.Contains("SESSION", StringComparison.OrdinalIgnoreCase) ? "<redacted len " + cookie.Value.Length + ">" : cookie.Value)} (HttpOnly={cookie.HttpOnly}, Secure={cookie.Secure})");
		}

		var body = await response.Content.ReadAsStringAsync(CancellationToken);
		TestOutputHelper.WriteLine($"  Body ({body.Length} chars): {body[..Math.Min(500, body.Length)]}");
		TestOutputHelper.WriteLine("");
	}
}
