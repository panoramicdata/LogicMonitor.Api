using System.Globalization;

namespace LogicMonitor.Api.Test.Settings;

public class NetscanTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task CanGetNetscanById()
	{
		var netscan = (await LogicMonitorClient.GetNetscanListAsync(default).ConfigureAwait(true)).Items?[0];

		if (netscan != null)
		{
			var refetchedNetscan = await LogicMonitorClient
				.GetNetscanByIdAsync(netscan.Id, default)
				.ConfigureAwait(true);
			refetchedNetscan.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task CreateNetscan()
	{
		var portalClient = LogicMonitorClient;

		var netscanGroups = await portalClient
			.GetAllAsync<NetscanGroup>(default)
			.ConfigureAwait(true);
		var netscanGroup = netscanGroups.SingleOrDefault(npg => npg.Name == "LogicMonitor API Unit Tests");
		netscanGroup.Should().NotBeNull();
		netscanGroup ??= new();
		// We have the Unit test netscan  group

		const string name = "LogicMonitor.Api UnitTest CreateNetscan";
		const string description = "Description 1";
		const int credentialsResourceGroupId = 0;
		const string netscanScheduleCron = "01 00 * * *";
		const NetscanMethod netscanMethod = NetscanMethod.Icmp;
		const NetscanScheduleType netscanScheduleType = NetscanScheduleType.Daily;
		var netscanScheduleWeekday = new List<string> { "1", "1" };
		const string netscanScheduleNthWeek = "5";
		const bool netscanScheduleNotify = false;
		var credentialsCustom = new List<object>();
		var netscanScheduleScheduleRecipients = new List<object>();
		const string subnetScanRange = "10.9.9.0/24";
		const string excludedIpAddresses = "10.9.9.1, 10.9.9.2";
		const string ddrChangeName = "##REVERSEDNS##";
		const NetscanExcludeDuplicatesStrategy duplicatesStrategyType = NetscanExcludeDuplicatesStrategy.MatchingAnyMonitoredResources;
		const int assignmentResourceGroupId = 1;
		const bool assignmentDisableAlerting = false;
		const NetscanInclusionType assignmentInclusionType = NetscanInclusionType.Include;
		var netscanCreationDto = new NetscanCreationDto
		{
			Name = name,
			Description = description,
			CollectorId = CollectorId.ToString(CultureInfo.InvariantCulture),
			Credentials = new NetscanCredentials
			{
				ResourceGroupId = credentialsResourceGroupId,
				Custom = credentialsCustom
			},
			Method = netscanMethod,
			Schedule = new NetscanSchedule
			{
				Type = netscanScheduleType,
				Cron = netscanScheduleCron,
				WeekDays = netscanScheduleWeekday,
				NthWeek = netscanScheduleNthWeek,
				Notify = netscanScheduleNotify,
				Recipients = netscanScheduleScheduleRecipients
			},
			//GroupName = netscanGroupName,
			GroupId = netscanGroup.Id.ToString(CultureInfo.InvariantCulture),
			SubnetScanRange = subnetScanRange,
			ExcludedIpAddresses = excludedIpAddresses,
			DiscoveredDeviceRule = new DiscoveredDeviceRule
			{
				Assignment =
					[
						new NetscanAssignment
						{
							ResourceGroupId = assignmentResourceGroupId,
							DisableAlerting = assignmentDisableAlerting,
							InclusionType = assignmentInclusionType,
						}
					],
				ChangeName = ddrChangeName
			},
			DuplicatesStrategy = new NetscanDuplicatesStrategy
			{
				Type = duplicatesStrategyType,
			},
			Ports = new NetscanPorts
			{
				IsGlobalDefault = false,
				Value = "21,22,23,25,53,69,80,81,110,123,135,143,389,443,445,631,993,1433,1521,3306,3389,5432,5672,6081,7199,8000,8080,8081,9100,10000,11211,27017"
			}
		};

		// Remove any existing  by this name
		var existingNetscan = (await portalClient.GetAllAsync<Netscan>(default).ConfigureAwait(true)).SingleOrDefault(nsp => nsp.Name == name);
		if (existingNetscan is not null)
		{
			await portalClient
				.DeleteAsync(existingNetscan, cancellationToken: default)
				.ConfigureAwait(true);
		}

		// Create one
		var createdNetscan = await portalClient.CreateAsync(netscanCreationDto, default)
			.ConfigureAwait(true);
		createdNetscan.Should().NotBeNull();
		// Ensure that the  is returned as expected

		createdNetscan.Name.Should().Be(name);
		createdNetscan.Description.Should().Be(description);
		createdNetscan.CollectorId.Should().Be(CollectorId);
		createdNetscan.Method.Should().Be(netscanMethod);
		createdNetscan.Schedule.Should().NotBeNull();
		createdNetscan.Schedule.Type.Should().Be(netscanScheduleType);
		createdNetscan.Schedule.Cron.Should().Be(netscanScheduleCron);
		createdNetscan.Schedule.Notify.Should().Be(netscanScheduleNotify);
		createdNetscan.GroupName.Should().Be(netscanGroup.Name);
		createdNetscan.GroupId.Should().Be(netscanGroup.Id);
		createdNetscan.CreatorName.Should().NotBeNull();
		createdNetscan.DuplicatesStrategy.Should().NotBeNull();
		createdNetscan.DuplicatesStrategy.Type.Should().Be(duplicatesStrategyType);

		// Clean up
		await portalClient
			.DeleteAsync(createdNetscan, cancellationToken: default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task ListAllNetscans()
	{
		var netscans = (await LogicMonitorClient.GetNetscanListAsync(default).ConfigureAwait(true)).Items;
		netscans.Should().NotBeNull();
		netscans.Should().NotBeNullOrEmpty();

		// Ids should all be distinct
		var ids = netscans.Select(nsp => nsp.Id);
		ids.Should().HaveCount(netscans.Count);
	}

	[Fact]
	public async Task ListFirst5Netscans()
	{
		var allNetscans = await LogicMonitorClient
			.GetAllAsync<Netscan>(default)
			.ConfigureAwait(true);

		const int expectedCount = 5;
		var netscans = await LogicMonitorClient
			.GetPageAsync(new Filter<Netscan> { Skip = 0, Take = expectedCount }, default)
			.ConfigureAwait(true);
		netscans.Should().NotBeNull();
		netscans.TotalCount.Should().Be(allNetscans.Count);
		netscans.Items.Should().NotBeNullOrEmpty();

		// Ids should all be distinct
		var ids = netscans.Items.Select(nsp => nsp.Id);
		netscans.Items.Should().HaveSameCount(ids);
		netscans.Items.Should().HaveCount(expectedCount);
	}
}
