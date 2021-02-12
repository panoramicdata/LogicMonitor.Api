using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Netscans;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class NetscanTests : TestWithOutput
	{
		public NetscanTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void CanGetNetscanById()
		{
			var netscan = (await LogicMonitorClient.GetAllAsync<Netscan>().ConfigureAwait(false))[0];

			var refetchedNetscan = await LogicMonitorClient.GetAsync<Netscan>(netscan.Id).ConfigureAwait(false);
			Assert.NotNull(refetchedNetscan);
		}

		[Fact]
		public async void CreateNetscan()
		{
			var portalClient = LogicMonitorClient;

			var netscanGroups = await portalClient.GetAllAsync<NetscanGroup>().ConfigureAwait(false);
			var netscanGroup = netscanGroups.SingleOrDefault(npg => npg.Name == "LogicMonitor API Unit Tests");
			Assert.NotNull(netscanGroup);
			// We have the Unit test netscan  group

			const string name = "LogicMonitor.Api UnitTest CreateNetscan";
			const string description = "Description 1";
			const int collectorId = 248;
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
				CollectorId = collectorId.ToString(),
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
				GroupId = netscanGroup.Id.ToString(),
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
			if (existingNetscan != null)
			{
				await portalClient.DeleteAsync(existingNetscan).ConfigureAwait(false);
			}

			// Create one
			var createdNetscan = await portalClient.CreateAsync(netscanCreationDto).ConfigureAwait(false);
			Assert.NotNull(createdNetscan);
			// Ensure that the  is returned as expected

			Assert.Equal(name, createdNetscan.Name);
			Assert.Equal(description, createdNetscan.Description);
			Assert.Equal(collectorId, createdNetscan.CollectorId);
			Assert.NotNull(createdNetscan.Credentials);
			Assert.Equal(credentialsDeviceGroupId, createdNetscan.Credentials.DeviceGroupId);
			//Assert.True(credentialsCustom.SequenceEqual(createdNetscan.Credentials.Custom));
			Assert.Equal(netscanMethod, createdNetscan.Method);
			Assert.NotNull(createdNetscan.Schedule);
			Assert.Equal(netscanScheduleType, createdNetscan.Schedule.Type);
			Assert.Equal(netscanScheduleCron, createdNetscan.Schedule.Cron);
			//Assert.True(netscanScheduleWeekday.SequenceEqual(createdNetscan.Schedule.Weekday));
			//Assert.Equal(netscanScheduleNthWeek, createdNetscan.Schedule.NthWeek);
			Assert.Equal(netscanScheduleNotify, createdNetscan.Schedule.Notify);
			//Assert.True(netscanScheduleScheduleRecipients.SequenceEqual(createdNetscan.Schedule.Recipients));
			Assert.Equal(netscanGroup.Name, createdNetscan.GroupName);
			Assert.Equal(netscanGroup.Id, createdNetscan.GroupId);
			Assert.Equal(subnetScanRange, createdNetscan.SubnetScanRange);
			Assert.Equal(excludedIpAddresses, createdNetscan.ExcludedIpAddresses);
			Assert.NotNull(createdNetscan.CreatorName);
			Assert.NotNull(createdNetscan.Ddr);
			Assert.NotNull(createdNetscan.Ddr.Assignment);
			Assert.Single(createdNetscan.Ddr.Assignment);
			Assert.Equal(assignmentDeviceGroupId, createdNetscan.Ddr.Assignment[0].DeviceGroupId);
			Assert.Equal(assignmentType, createdNetscan.Ddr.Assignment[0].Type);
			Assert.Equal(assignmentInclusionType, createdNetscan.Ddr.Assignment[0].InclusionType);
			Assert.Equal(assignmentQuery, createdNetscan.Ddr.Assignment[0].Query);
			Assert.Equal(assignmentDisableAlerting, createdNetscan.Ddr.Assignment[0].DisableAlerting);
			Assert.Equal(ddrChangeName, createdNetscan.Ddr.ChangeName);
			Assert.NotNull(createdNetscan.DuplicatesStrategy);
			Assert.Equal(duplicatesStrategyType, createdNetscan.DuplicatesStrategy.Type);
			//Assert.True(duplicatedStrategyGroups.Select(obj=>).SequenceEqual(createdNetscan.DuplicatesStrategy.Groups));
			//Assert.True(duplicatesStrategyCollectors.SequenceEqual(createdNetscan.DuplicatesStrategy.Collectors));

			// Clean up
			await portalClient.DeleteAsync(createdNetscan).ConfigureAwait(false);
		}

		[Fact]
		public async void ListAllNetscans()
		{
			var netscans = await LogicMonitorClient.GetAllAsync<Netscan>().ConfigureAwait(false);
			Assert.NotNull(netscans);
			Assert.NotEmpty(netscans);

			// Ids should all be distinct
			var ids = netscans.Select(nsp => nsp.Id);
			Assert.True(netscans.Count == ids.Count());
		}

		[Fact]
		public async void ListFirst5Netscans()
		{
			var allNetscans = await LogicMonitorClient.GetAllAsync<Netscan>().ConfigureAwait(false);

			const int expectedCount = 5;
			var netscans = await LogicMonitorClient.GetPageAsync(new Filter<Netscan> { Skip = 0, Take = expectedCount }).ConfigureAwait(false);
			Assert.NotNull(netscans);
			Assert.Equal(allNetscans.Count, netscans.TotalCount);
			Assert.NotEmpty(netscans.Items);

			// Ids should all be distinct
			var ids = netscans.Items.Select(nsp => nsp.Id);
			Assert.True(netscans.Items.Count == ids.Count());

			Assert.Equal(expectedCount, netscans.Items.Count);
		}
	}
}