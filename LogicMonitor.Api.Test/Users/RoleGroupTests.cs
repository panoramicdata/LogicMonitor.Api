﻿using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Reports;
using LogicMonitor.Api.Users;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Users;

public class RoleGroupTests : TestWithOutput
{
	private const string TestName = "Test Name";

	public RoleGroupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void GetAll()
	{
		var items = await LogicMonitorClient
			.GetAllAsync<RoleGroup>()
			.ConfigureAwait(false);
		Assert.NotNull(items);
		Assert.NotEmpty(items);
	}

	[Fact]
	public async void Crud()
	{
		// Delete it if it already exists
		var existingItems = await LogicMonitorClient.GetAllAsync(new Filter<RoleGroup>
		{
			FilterItems = new List<FilterItem<RoleGroup>>
				{
					new Eq<RoleGroup>(nameof(ReportGroup.Name), TestName)
				}
		}).ConfigureAwait(false);
		foreach (var existingItem in existingItems)
		{
			await LogicMonitorClient
				.DeleteAsync(existingItem)
				.ConfigureAwait(false);
		}

		// Create it
		var newItem = await LogicMonitorClient
			.CreateAsync(
				new ReportGroupCreationDto
				{
					Name = TestName,
					Description = "Test Description"
				}
			)
			.ConfigureAwait(false);

		// Delete it again
		await LogicMonitorClient
			.DeleteAsync(newItem)
			.ConfigureAwait(false);
	}
}
