using LogicMonitor.Api.Netscans;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class NetscanGroupTests : TestWithOutput
	{
		public NetscanGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void CanCreateAndDeleteNetscanGroups()
		{
			var allNetscanGroups = await PortalClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false);
			const string name = "API Unit Test CanCreateAndDeleteNetscanGroups";
			const string description = "API Unit Test CanCreateAndDeleteNetscanGroups Description";

			var existingTestNetscanGroup = allNetscanGroups.SingleOrDefault(group => group.Name == name);
			if (existingTestNetscanGroup != null)
			{
				await PortalClient.DeleteAsync<NetscanGroup>(existingTestNetscanGroup.Id).ConfigureAwait(false);
			}
			Assert.DoesNotContain(await PortalClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false), group => group.Name == name);
			// Definitely not there now

			// Create one
			var netscanGroupCreationDto = new NetscanGroupCreationDto
			{
				Name = name,
				Description = description
			};
			var newNetscanGroup = await PortalClient.CreateAsync(netscanGroupCreationDto).ConfigureAwait(false);
			Assert.Contains(await PortalClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false), group => group.Name == name);

			await PortalClient.DeleteAsync(newNetscanGroup).ConfigureAwait(false);
			Assert.DoesNotContain(await PortalClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false), group => group.Name == name);
		}

		[Fact]
		public async void CanGetNetscanGroups()
		{
			var allNetscanGroups = await PortalClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false);
			Assert.NotNull(allNetscanGroups);
			Assert.NotEmpty(allNetscanGroups);
			var ids = allNetscanGroups.Select(nspg => nspg.Id);
			Assert.True(allNetscanGroups.Count == ids.Count());
			Assert.NotEmpty(allNetscanGroups);
		}
	}
}