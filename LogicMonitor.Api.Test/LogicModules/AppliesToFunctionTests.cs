using LogicMonitor.Api.LogicModules;
using System;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.LogicModules
{
	public class AppliesToFunctionTests : TestWithOutput
	{
		public AppliesToFunctionTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void CreateUpdateAndDelete()
		{
			const string testAppliesToFunctionName = "XxxUnitTest";
			var testAppliesToFunctionDescription = $"{testAppliesToFunctionName} - Description";
			var testAppliesToFunctionCode = $"displayname == \"{testAppliesToFunctionName}\"";

			// Delete if already present
			var existingAppliesToFunction = await PortalClient.GetByNameAsync<AppliesToFunction>(testAppliesToFunctionName).ConfigureAwait(false);
			if (existingAppliesToFunction != null)
			{
				await PortalClient.DeleteAsync(existingAppliesToFunction).ConfigureAwait(false);
			}

			// Create
			var createdAppliesToFunction = await PortalClient.CreateAsync(new AppliesToFunctionCreationDto
			{
				Name = testAppliesToFunctionName,
				Description = testAppliesToFunctionDescription,
				Code = testAppliesToFunctionCode
			}).ConfigureAwait(false);

			// Refetch and check
			existingAppliesToFunction = await PortalClient.GetByNameAsync<AppliesToFunction>(testAppliesToFunctionName).ConfigureAwait(false);
			Assert.NotNull(existingAppliesToFunction);
			Assert.Equal(createdAppliesToFunction.Id, existingAppliesToFunction.Id);

			// Update
			var newDescription = testAppliesToFunctionDescription + "2";
			existingAppliesToFunction.Description = newDescription;
			await PortalClient.PutAsync(existingAppliesToFunction).ConfigureAwait(false);

			// Refetch and check
			existingAppliesToFunction = await PortalClient.GetByNameAsync<AppliesToFunction>(testAppliesToFunctionName).ConfigureAwait(false);
			Assert.Equal(newDescription, existingAppliesToFunction.Description);

			// Delete
			await PortalClient.DeleteAsync(existingAppliesToFunction).ConfigureAwait(false);

			// Refetch and check
			existingAppliesToFunction = await PortalClient.GetByNameAsync<AppliesToFunction>(testAppliesToFunctionName).ConfigureAwait(false);
			Assert.Null(existingAppliesToFunction);
		}

		[Theory]
		[InlineData("1.2.3/24", "join(system.ips, \",\") =~ \"(^|,)1\\\\.2\\\\.3\\\\.\\\\d+(,|$)\"")]
		public void AppliesToFromCidrTests_Succeeds(string input, string expected)
		{
			var appliesToFunction = new AppliesToFunction();
			appliesToFunction.SetCodeFromCidr(input);
			Assert.Equal(expected, appliesToFunction.Code);
		}

		[Theory]
		[InlineData("1.2.3")]
		[InlineData("1.2.3.4")]
		[InlineData("1.2.3.4/33")]
		[InlineData("1.2.3.4/x")]
		[InlineData("x/x")]
		[InlineData("1.2.333.0/24")]
		public void AppliesToFromCidrTests_ThrowsFormatException(string input)
		{
			var appliesToFunction = new AppliesToFunction();
			Assert.Throws<FormatException>(() => appliesToFunction.SetCodeFromCidr(input));
		}

		[Theory]
		[InlineData("0.0.0.0/0")]
		[InlineData("0.0.0.0/1")]
		[InlineData("0.0.0.0/2")]
		[InlineData("0.0.0.0/3")]
		[InlineData("0.0.0.0/4")]
		[InlineData("0.0.0.0/5")]
		[InlineData("0.0.0.0/6")]
		[InlineData("0.0.0.0/7")]
		[InlineData("0.0.0.0/8")]
		[InlineData("0.0.0.0/9")]
		[InlineData("0.0.0.0/10")]
		[InlineData("0.0.0.0/11")]
		[InlineData("0.0.0.0/12")]
		[InlineData("0.0.0.0/13")]
		[InlineData("0.0.0.0/14")]
		[InlineData("0.0.0.0/15")]
		[InlineData("0.0.0.0/16")]
		[InlineData("0.0.0.0/17")]
		[InlineData("0.0.0.0/18")]
		[InlineData("0.0.0.0/19")]
		[InlineData("0.0.0.0/20")]
		[InlineData("0.0.0.0/21")]
		[InlineData("0.0.0.0/22")]
		public void AppliesToFromCidrTests_ThrowsNotSupportedException(string input)
		{
			var appliesToFunction = new AppliesToFunction();
			Assert.Throws<NotSupportedException>(() => appliesToFunction.SetCodeFromCidr(input));
		}

		[Fact]
		public async void CustomerCodeWorks()
		{
			var matches = await PortalClient
				.GetAppliesToAsync("customer.code == \"PDL\"")
				.ConfigureAwait(false);
			Assert.NotNull(matches);
			Assert.NotEmpty(matches);
		}
	}
}