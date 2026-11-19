using System.Globalization;

namespace LogicMonitor.Api.Test.Settings;

public class NetscanTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	private const string ExpectedNetscanName = "LogicMonitor.Api UnitTest CreateNetscan";
	private const string ExpectedNetscanDescription = "Description 1";
	private const NetscanMethod ExpectedNetscanMethod = NetscanMethod.Icmp;
	private const NetscanScheduleType ExpectedScheduleType = NetscanScheduleType.Daily;
	private const string ExpectedScheduleCron = "01 00 * * *";
	private const bool ExpectedScheduleNotify = false;
	private const NetscanExcludeDuplicatesStrategy ExpectedDuplicatesStrategyType = NetscanExcludeDuplicatesStrategy.MatchingAnyMonitoredResources;

	[Fact]
	public async Task CanGetNetscanById()
	{
		var netscan = (await LogicMonitorClient.GetNetscanListAsync(CancellationToken)).Items?[0];

		if (netscan != null)
		{
			var refetchedNetscan = await LogicMonitorClient
				.GetNetscanByIdAsync(netscan.Id, CancellationToken);
			refetchedNetscan.Should().NotBeNull();
		}
	}

	[Fact]
	public async Task CreateNetscan()
	{
		var portalClient = LogicMonitorClient;

		var netscanGroups = await portalClient
			.GetAllAsync<NetscanGroup>(CancellationToken);
		var netscanGroup = netscanGroups.SingleOrDefault(npg => npg.Name == "LogicMonitor API Unit Tests");
		netscanGroup.Should().NotBeNull();
		netscanGroup ??= new();
		// We have the Unit test netscan  group

		var netscanCreationDto = BuildNetscanCreationDto(netscanGroup);

		// Remove any existing  by this name
		var existingNetscan = (await portalClient.GetAllAsync<Netscan>(CancellationToken))
			.SingleOrDefault(nsp => nsp.Name == ExpectedNetscanName);
		if (existingNetscan is not null)
		{
			await portalClient.DeleteAsync(existingNetscan, CancellationToken);
		}

		// Create one
		var createdNetscan = await portalClient.CreateAsync(netscanCreationDto, CancellationToken);
		createdNetscan.Should().NotBeNull();
		// Ensure that the  is returned as expected

		createdNetscan.Name.Should().Be(ExpectedNetscanName);
		createdNetscan.Description.Should().Be(ExpectedNetscanDescription);
		createdNetscan.CollectorId.Should().Be(CollectorId);
		createdNetscan.Method.Should().Be(ExpectedNetscanMethod);
		createdNetscan.Schedule.Should().NotBeNull();
		createdNetscan.Schedule.Type.Should().Be(ExpectedScheduleType);
		createdNetscan.Schedule.Cron.Should().Be(ExpectedScheduleCron);
		createdNetscan.Schedule.Notify.Should().Be(ExpectedScheduleNotify);
		createdNetscan.GroupName.Should().Be(netscanGroup.Name);
		createdNetscan.GroupId.Should().Be(netscanGroup.Id);
		createdNetscan.CreatorName.Should().NotBeNull();
		createdNetscan.DuplicatesStrategy.Should().NotBeNull();
		createdNetscan.DuplicatesStrategy.Type.Should().Be(ExpectedDuplicatesStrategyType);

		// Clean up
		await portalClient
			.DeleteAsync(createdNetscan, CancellationToken);
	}

	private NetscanCreationDto BuildNetscanCreationDto(NetscanGroup netscanGroup)
	{
		const int credentialsResourceGroupId = 0;
		var credentialsCustom = new List<object>();
		const string subnetScanRange = "10.9.9.0/24";
		const string excludedIpAddresses = "10.9.9.1, 10.9.9.2";

		return new NetscanCreationDto
		{
			Name = ExpectedNetscanName,
			Description = ExpectedNetscanDescription,
			CollectorId = CollectorId.ToString(CultureInfo.InvariantCulture),
			Credentials = new NetscanCredentials
			{
				ResourceGroupId = credentialsResourceGroupId,
				Custom = credentialsCustom
			},
			Method = ExpectedNetscanMethod,
			Schedule = BuildNetscanSchedule(),
			//GroupName = netscanGroupName,
			GroupId = netscanGroup.Id.ToString(CultureInfo.InvariantCulture),
			SubnetScanRange = subnetScanRange,
			ExcludedIpAddresses = excludedIpAddresses,
			DiscoveredResourceRule = BuildDiscoveredResourceRule(),
			DuplicatesStrategy = new NetscanDuplicatesStrategy
			{
				Type = ExpectedDuplicatesStrategyType,
			},
			Ports = new NetscanPorts
			{
				IsGlobalDefault = false,
				Value = "21,22,23,25,53,69,80,81,110,123,135,143,389,443,445,631,993,1433,1521,3306,3389,5432,5672,6081,7199,8000,8080,8081,9100,10000,11211,27017"
			}
		};
	}

	private static NetscanSchedule BuildNetscanSchedule()
	{
		var netscanScheduleWeekday = new List<string> { "1", "1" };
		const string netscanScheduleNthWeek = "5";
		var netscanScheduleScheduleRecipients = new List<object>();

		return new NetscanSchedule
		{
			Type = ExpectedScheduleType,
			Cron = ExpectedScheduleCron,
			WeekDays = netscanScheduleWeekday,
			NthWeek = netscanScheduleNthWeek,
			Notify = ExpectedScheduleNotify,
			Recipients = netscanScheduleScheduleRecipients
		};
	}

	private static DiscoveredResourceRule BuildDiscoveredResourceRule()
	{
		const string ddrChangeName = "##REVERSEDNS##";
		const int assignmentResourceGroupId = 1;
		const bool assignmentDisableAlerting = false;
		const NetscanInclusionType assignmentInclusionType = NetscanInclusionType.Include;

		return new DiscoveredResourceRule
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
		};
	}

	[Fact]
	public async Task ListAllNetscans()
	{
		var netscans = (await LogicMonitorClient.GetNetscanListAsync(CancellationToken)).Items;
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
			.GetAllAsync<Netscan>(CancellationToken);

		const int expectedCount = 5;
		var netscans = await LogicMonitorClient
			.GetPageAsync(new Filter<Netscan> { Skip = 0, Take = expectedCount }, CancellationToken);
		netscans.Should().NotBeNull();
		netscans.TotalCount.Should().Be(allNetscans.Count);
		netscans.Items.Should().NotBeNullOrEmpty();

		// Ids should all be distinct
		var ids = netscans.Items.Select(nsp => nsp.Id);
		netscans.Items.Should().HaveSameCount(ids);
		netscans.Items.Should().HaveCount(expectedCount);
	}
}
