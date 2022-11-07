using System.Globalization;

namespace LogicMonitor.Api.Test.Settings;

public class NetscanTests : TestWithOutput
{
	public NetscanTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task CanGetNetscanById()
	{
		var netscan = (await LogicMonitorClient.GetAllAsync<Netscan>().ConfigureAwait(false))[0];

		var refetchedNetscan = await LogicMonitorClient.GetAsync<Netscan>(netscan.Id).ConfigureAwait(false);
		refetchedNetscan.Should().NotBeNull();
	}

	[Fact]
	public async Task CreateNetscan()
	{
		var portalClient = LogicMonitorClient;

		var netscanGroups = await portalClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false);
		var netscanGroup = netscanGroups.SingleOrDefault(npg => npg.Name == "LogicMonitor API Unit Tests");
		netscanGroup.Should().NotBeNull();
		// We have the Unit test netscan  group

		const string name = "LogicMonitor.Api UnitTest CreateNetscan";
		const string description = "Description 1";
		const int credentialsDeviceGroupId = 0;
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
		const NetscanExcludeDuplicatesStrategy duplicatesStrategyType = NetscanExcludeDuplicatesStrategy.MatchingAnyMonitoredDevices;
		var duplicatedStrategyGroups = new List<object>();
		var duplicatesStrategyCollectors = new List<object>();
		const int assignmentDeviceGroupId = 1;
		const bool assignmentDisableAlerting = false;
		const NetscanInclusionType assignmentInclusionType = NetscanInclusionType.Include;
		const NetscanAssignmentType assignmentType = NetscanAssignmentType.All;
		var assignmentQuery = string.Empty;
		var netscanCreationDto = new NetscanCreationDto
		{
			Name = name,
			Description = description,
			CollectorId = CollectorId.ToString(CultureInfo.InvariantCulture),
			Credentials = new NetscanCredentials
			{
				DeviceGroupId = credentialsDeviceGroupId,
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
			Ddr = new NetscanDdr
			{
				Assignment = new List<NetscanAssignment>
					{
						new NetscanAssignment
						{
							DeviceGroupId = assignmentDeviceGroupId,
							DisableAlerting = assignmentDisableAlerting,
							InclusionType = assignmentInclusionType,
							Type = assignmentType,
							Query = assignmentQuery
						}
					},
				ChangeName = ddrChangeName
			},
			DuplicatesStrategy = new NetscanDuplicatesStrategy
			{
				Type = duplicatesStrategyType,
				Groups = duplicatedStrategyGroups,
				Collectors = duplicatesStrategyCollectors
			},
			Ports = new NetscanPorts
			{
				IsGlobalDefault = false,
				Value = "21,22,23,25,53,69,80,81,110,123,135,143,389,443,445,631,993,1433,1521,3306,3389,5432,5672,6081,7199,8000,8080,8081,9100,10000,11211,27017"
			}
		};

		// Remove any existing  by this name
		var existingNetscan = (await portalClient.GetAllAsync<Netscan>().ConfigureAwait(false)).SingleOrDefault(nsp => nsp.Name == name);
		if (existingNetscan is not null)
		{
			await portalClient.DeleteAsync(existingNetscan).ConfigureAwait(false);
		}

		// Create one
		var createdNetscan = await portalClient.CreateAsync(netscanCreationDto).ConfigureAwait(false);
		createdNetscan.Should().NotBeNull();
		// Ensure that the  is returned as expected

		createdNetscan.Name.Should().Be(name);
		createdNetscan.Description.Should().Be(description);
		createdNetscan.CollectorId.Should().Be(CollectorId);
		createdNetscan.Credentials.Should().NotBeNull();
		createdNetscan.Credentials.DeviceGroupId.Should().Be(credentialsDeviceGroupId);
		createdNetscan.Method.Should().Be(netscanMethod);
		createdNetscan.Schedule.Should().NotBeNull();
		createdNetscan.Schedule.Type.Should().Be(netscanScheduleType);
		createdNetscan.Schedule.Cron.Should().Be(netscanScheduleCron);
		createdNetscan.Schedule.Notify.Should().Be(netscanScheduleNotify);
		createdNetscan.GroupName.Should().Be(netscanGroup.Name);
		createdNetscan.GroupId.Should().Be(netscanGroup.Id);
		createdNetscan.SubnetScanRange.Should().Be(subnetScanRange);
		createdNetscan.ExcludedIpAddresses.Should().Be(excludedIpAddresses);
		createdNetscan.CreatorName.Should().NotBeNull();
		createdNetscan.Ddr.Should().NotBeNull();
		createdNetscan.Ddr.Assignment.Should().NotBeNull();
		createdNetscan.Ddr.Assignment.Should().HaveCount(1);
		createdNetscan.Ddr.Assignment[0].DeviceGroupId.Should().Be(assignmentDeviceGroupId);
		createdNetscan.Ddr.Assignment[0].Type.Should().Be(assignmentType);
		createdNetscan.Ddr.Assignment[0].InclusionType.Should().Be(assignmentInclusionType);
		createdNetscan.Ddr.Assignment[0].Query.Should().Be(assignmentQuery);
		createdNetscan.Ddr.Assignment[0].DisableAlerting.Should().Be(assignmentDisableAlerting);
		createdNetscan.Ddr.ChangeName.Should().Be(ddrChangeName);
		createdNetscan.DuplicatesStrategy.Should().NotBeNull();
		createdNetscan.DuplicatesStrategy.Type.Should().Be(duplicatesStrategyType);

		// Clean up
		await portalClient.DeleteAsync(createdNetscan).ConfigureAwait(false);
	}

	[Fact]
	public async Task ListAllNetscans()
	{
		var netscans = await LogicMonitorClient.GetAllAsync<Netscan>().ConfigureAwait(false);
		netscans.Should().NotBeNull();
		netscans.Should().NotBeNullOrEmpty();

		// Ids should all be distinct
		var ids = netscans.Select(nsp => nsp.Id);
		ids.Should().HaveCount(netscans.Count);
	}

	[Fact]
	public async Task ListFirst5Netscans()
	{
		var allNetscans = await LogicMonitorClient.GetAllAsync<Netscan>().ConfigureAwait(false);

		const int expectedCount = 5;
		var netscans = await LogicMonitorClient.GetPageAsync(new Filter<Netscan> { Skip = 0, Take = expectedCount }).ConfigureAwait(false);
		netscans.Should().NotBeNull();
		netscans.TotalCount.Should().Be(allNetscans.Count);
		netscans.Items.Should().NotBeNullOrEmpty();

		// Ids should all be distinct
		var ids = netscans.Items.Select(nsp => nsp.Id);
		netscans.Items.Should().HaveSameCount(ids);
		netscans.Items.Count.Should().Be(expectedCount);
	}
}
