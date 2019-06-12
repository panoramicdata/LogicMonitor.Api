using LogicMonitor.Api.Extensions;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.ScheduledDownTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.ScheduledDownTimes
{
	public class ScheduledDownTimeTests : TestWithOutput
	{
		public ScheduledDownTimeTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetDeviceScheduledDownTimes()
		{
			var sdts = await PortalClient.GetAllAsync(new Filter<ScheduledDownTime>
			{
				FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceSDT"),
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.IsEffective), false),
				}
			}).ConfigureAwait(false);
			Assert.All(sdts, sdt => Assert.False(sdt.IsEffective));
		}

		[Fact]
		public async void AddAndDeleteADeviceSdt()
		{
			var portalClient = PortalClient;
			// var device = await portalClient.GetDeviceByDisplayNameAsync(portalConfig.WindowsDeviceDisplayName);
			const string initialComment = "Woo";
			var deviceId = WindowsDeviceId;
			var sdtCreationDto = new DeviceScheduledDownTimeCreationDto(deviceId)
			{
				Comment = initialComment,
				StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
				EndDateTimeEpochMs = DateTime.UtcNow.AddDays(7).MillisecondsSinceTheEpoch(),
				RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
			};

			// Check the created SDT looks right
			var createdSdt = await portalClient.CreateAsync(sdtCreationDto).ConfigureAwait(false);
			Assert.Equal(initialComment, createdSdt.Comment);
			Assert.Equal(deviceId, createdSdt.DeviceId);

			// Check the re-fetched SDT looks right
			var refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
			Assert.Equal(initialComment, refetchSdt.Comment);
			Assert.Equal(deviceId, refetchSdt.DeviceId);

			// Update
			const string newComment = "Yay";
			createdSdt.Comment = newComment;
			await portalClient.PutStringIdentifiedItemAsync(createdSdt).ConfigureAwait(false);

			// Check the re-fetched SDT looks right
			refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
			Assert.Equal(newComment, refetchSdt.Comment);
			Assert.Equal(deviceId, refetchSdt.DeviceId);

			// Get all scheduled downtimes (we have created one, so at least that one should be there)
			var scheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
			{
				FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
			}).ConfigureAwait(false);
			Assert.NotNull(scheduledDownTimes);
			Assert.NotEmpty(scheduledDownTimes);

			// Get them all individually
			foreach (var sdt in scheduledDownTimes)
			{
				var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
				Assert.Equal(sdt.Id, refetchedSdt.Id);
				Assert.Equal(sdt.DeviceId, refetchedSdt.DeviceId);
				Assert.Equal(sdt.Comment, refetchedSdt.Comment);
			}

			// Delete
			await portalClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		}

		[Fact]
		public async void AddAndDeleteADeviceGroupSdt()
		{
			var portalClient = PortalClient;
			const string initialComment = "Woo";
			var deviceGroupId = 1; // The root
			var sdtCreationDto = new DeviceGroupScheduledDownTimeCreationDto(deviceGroupId)
			{
				Comment = initialComment,
				StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
				EndDateTimeEpochMs = DateTime.UtcNow.AddDays(7).MillisecondsSinceTheEpoch(),
				RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
			};

			// Check the created SDT looks right
			var createdSdt = await portalClient.CreateAsync(sdtCreationDto).ConfigureAwait(false);
			Assert.Equal(initialComment, createdSdt.Comment);
			Assert.Equal(deviceGroupId, createdSdt.DeviceGroupId);

			// Check the re-fetched SDT looks right
			var refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
			Assert.Equal(initialComment, refetchSdt.Comment);
			Assert.Equal(deviceGroupId, refetchSdt.DeviceGroupId);

			// Update
			const string newComment = "Yay";
			createdSdt.Comment = newComment;
			await portalClient.PutStringIdentifiedItemAsync(createdSdt).ConfigureAwait(false);

			// Check the re-fetched SDT looks right
			refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
			Assert.Equal(newComment, refetchSdt.Comment);
			Assert.Equal(deviceGroupId, refetchSdt.DeviceGroupId);

			// Get all scheduled downtimes (we have created one, so at least that one should be there)
			var scheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
			{
				FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "DeviceGroupSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
			}).ConfigureAwait(false);
			Assert.NotNull(scheduledDownTimes);
			Assert.NotEmpty(scheduledDownTimes);

			// Get them all individually
			foreach (var sdt in scheduledDownTimes)
			{
				var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
				Assert.Equal(sdt.Id, refetchedSdt.Id);
				Assert.Equal(sdt.DeviceGroupId, refetchedSdt.DeviceGroupId);
				Assert.Equal(sdt.Comment, refetchedSdt.Comment);
			}

			// Delete
			await portalClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		}

		[Fact]
		public async void AddAndDeleteACollectorSdt()
		{
			var portalClient = PortalClient;
			var collector = (await portalClient
				.GetAllAsync(new Filter<Collectors.Collector> { Take = 1 })
				.ConfigureAwait(false))
				.SingleOrDefault();
			Assert.NotNull(collector);
			const string initialComment = "Woo";
			var collectorId = collector.Id;
			var sdtCreationDto = new CollectorScheduledDownTimeCreationDto(collectorId)
			{
				Comment = initialComment,
				StartDateTimeEpochMs = DateTime.UtcNow.MillisecondsSinceTheEpoch(),
				EndDateTimeEpochMs = DateTime.UtcNow.AddDays(7).MillisecondsSinceTheEpoch(),
				RecurrenceType = ScheduledDownTimeRecurrenceType.OneTime
			};

			// Check the created SDT looks right
			var createdSdt = await portalClient.CreateAsync(sdtCreationDto).ConfigureAwait(false);
			Assert.Equal(initialComment, createdSdt.Comment);
			Assert.Equal(collectorId, createdSdt.CollectorId);

			// Check the re-fetched SDT looks right
			var refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
			Assert.Equal(initialComment, refetchSdt.Comment);
			Assert.Equal(collectorId, refetchSdt.CollectorId);

			// Update
			const string newComment = "Yay";
			createdSdt.Comment = newComment;
			await portalClient.PutStringIdentifiedItemAsync(createdSdt).ConfigureAwait(false);

			// Check the re-fetched SDT looks right
			refetchSdt = await portalClient.GetAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
			Assert.Equal(newComment, refetchSdt.Comment);
			Assert.Equal(collectorId, refetchSdt.CollectorId);

			// Get all scheduled downtimes (we have created one, so at least that one should be there)
			var scheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
			{
				FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.Type), "CollectorSDT"),
					new Gt<ScheduledDownTime>(nameof(ScheduledDownTime.StartDateTimeMs), DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch())
				}
			}).ConfigureAwait(false);
			Assert.NotNull(scheduledDownTimes);
			Assert.NotEmpty(scheduledDownTimes);

			// Get them all individually
			foreach (var sdt in scheduledDownTimes)
			{
				var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
				Assert.Equal(sdt.Id, refetchedSdt.Id);
				Assert.Equal(sdt.DeviceId, refetchedSdt.DeviceId);
				Assert.Equal(sdt.Comment, refetchedSdt.Comment);
			}

			// Delete
			await portalClient.DeleteAsync<ScheduledDownTime>(createdSdt.Id).ConfigureAwait(false);
		}

		[Fact]
		public async void GetScheduledDownTimesFilteredByDevice()
		{
			var portalClient = PortalClient;

			var allScheduledDownTimes = await portalClient.GetAllAsync<ScheduledDownTime>().ConfigureAwait(false);

			var deviceId = allScheduledDownTimes.Find(sdt => sdt.Type == ScheduledDownTimeType.Device)?.DeviceId;
			Assert.NotNull(deviceId);
			var filteredScheduledDownTimes = await portalClient.GetAllAsync(new Filter<ScheduledDownTime>
			{
				FilterItems = new List<FilterItem<ScheduledDownTime>>
				{
					new Eq<ScheduledDownTime>(nameof(Type), "DeviceSDT"),
					new Eq<ScheduledDownTime>(nameof(ScheduledDownTime.DeviceId), deviceId)
				}
			}
			).ConfigureAwait(false);
			Assert.NotNull(filteredScheduledDownTimes);

			// Get them all individually
			foreach (var sdt in filteredScheduledDownTimes)
			{
				var refetchedSdt = await portalClient.GetAsync<ScheduledDownTime>(sdt.Id).ConfigureAwait(false);
				Assert.Equal(sdt.Id, refetchedSdt.Id);
				Assert.Equal(deviceId, refetchedSdt.DeviceId);
				Assert.Equal(sdt.Comment, refetchedSdt.Comment);
			}
		}
	}
}