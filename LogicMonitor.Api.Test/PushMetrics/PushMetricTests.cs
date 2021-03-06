﻿using FluentAssertions;
using LogicMonitor.Api.PushMetrics;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.PushMetrics
{
	public class PushMetricTests : TestWithOutput
	{
		public PushMetricTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void PushMetric_Succeeds()
		{
			var response = await LogicMonitorClient
				.PushMetricAsync(new PushMetric
				{
					ResourceIds = new()
					{
						["system.deviceId"] = WindowsDeviceId.ToString()
					},
					DataSourceName = "UnitTest_PushMetric_Succeeds",
					DataSourceDisplayName = "PushMetric_Succeeds",
					DataSourceGroup = "Unit Tests",
					Instances = new()
					{
						new()
						{
							Name = "Slot1",
							DisplayName = "Slot 1",
							Properties = new()
							{
								["unit_test.slot_id"] = "1"
							},
							DataPoints = new()
							{
								new()
								{
									Name = "DataPoint1",
									Description = "DataPoint 1",
									//AggregationType = PushMetricAggregationType.Mean,
									DataType = PushMetricDataPointDataType.Counter,
									Values = new Dictionary<DateTimeOffset, int>
									{
										[DateTimeOffset.UtcNow] = 10
									}.ToLogicMonitorDictionary()
								}
							}
						}
					}
				})
				.ConfigureAwait(false);
			response.Should().NotBeNull();
		}
	}
}