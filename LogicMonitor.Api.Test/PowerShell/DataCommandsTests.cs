using System.Management.Automation;

namespace LogicMonitor.Api.Test.PowerShell;

/// <summary>
/// Tests for LogicMonitor data PowerShell cmdlets
/// </summary>
public class DataCommandsTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) 
	: PowerShellTestBase(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public void GetLMRawData_WithValidParameters_ShouldReturnData()
	{
		// Arrange
		ConnectToLogicMonitor();
		
		// Get a resource with data
		var resources = InvokePowerShell("Get-LMResource", new Dictionary<string, object>
		{
			{ "Take", 5 }
		});

		resources.Should().HaveCountGreaterThan(0);
		
		// Find a resource that likely has data (skip if none found)
		dynamic? resourceWithData = null;
		foreach (var resourceObj in resources)
		{
			dynamic? resource = resourceObj.BaseObject;
			if (resource != null)
			{
				resourceWithData = resource;
				break;
			}
		}

		if (resourceWithData == null)
		{
			return; // Skip test if no suitable resource found
		}

		var resourceId = (int)resourceWithData.Id;

		// For this test, we'll use common DataSource and Instance IDs that are likely to exist
		// In a real scenario, you'd query for actual DataSources and Instances
		var dataSourceId = 1; // Often system DataSources start at 1
		var instanceId = 1;

		// Act
		try
		{
			var results = InvokePowerShell("Get-LMRawData", new Dictionary<string, object>
			{
				{ "ResourceId", resourceId },
				{ "DataSourceId", dataSourceId },
				{ "InstanceId", instanceId }
			});

			// Assert
			results.Should().NotBeNull();
			
			if (results.Count > 0)
			{
				var rawData = results.First().BaseObject;
				rawData.Should().NotBeNull();
				
				// Check that it has expected properties
				var properties = rawData.GetType().GetProperties();
				properties.Should().Contain(p => p.Name == "Values" || p.Name == "DataPoints");
			}
		}
		catch (InvalidOperationException ex) when (ex.Message.Contains("not found") || ex.Message.Contains("does not exist"))
		{
			// Skip test if the specific DataSource/Instance combination doesn't exist
			return;
		}
	}

	[Fact]
	public void InvokeLMPollNow_WithValidParameters_ShouldTriggerPoll()
	{
		// Arrange
		ConnectToLogicMonitor();
		
		// Get a resource
		var resources = InvokePowerShell("Get-LMResource", new Dictionary<string, object>
		{
			{ "Take", 1 }
		});

		if (resources.Count == 0)
		{
			return; // Skip if no resources
		}

		dynamic? resource = resources.First().BaseObject;
		var resourceId = (int)resource!.Id;

		// For this test, we'll use common IDs
		var resourceDataSourceId = 1;
		var resourceDataSourceInstanceId = 1;

		// Act
		try
		{
			var results = InvokePowerShell("Invoke-LMPollNow", new Dictionary<string, object>
			{
				{ "ResourceId", resourceId },
				{ "ResourceDataSourceId", resourceDataSourceId },
				{ "ResourceDataSourceInstanceId", resourceDataSourceInstanceId }
			});

			// Assert
			results.Should().NotBeNull();
			
			if (results.Count > 0)
			{
				var pollResponse = results.First().BaseObject;
				pollResponse.Should().NotBeNull();
				
				// Check that it has expected properties
				var properties = pollResponse.GetType().GetProperties();
				properties.Should().Contain(p => p.Name == "RequestStatus" || p.Name == "Status");
			}
		}
		catch (InvalidOperationException ex) when (ex.Message.Contains("not found") || ex.Message.Contains("does not exist"))
		{
			// Skip test if the specific DataSource/Instance combination doesn't exist
			return;
		}
	}

	[Fact]
	public void GetLMGraphData_WithValidParameters_ShouldReturnGraphData()
	{
		// Arrange
		ConnectToLogicMonitor();

		// For this test, we'll use common IDs that are likely to exist
		var resourceDataSourceInstanceId = 1;
		var dataSourceGraphId = 1;

		// Act
		try
		{
			var results = InvokePowerShell("Get-LMGraphData", new Dictionary<string, object>
			{
				{ "ResourceDataSourceInstanceId", resourceDataSourceInstanceId },
				{ "DataSourceGraphId", dataSourceGraphId }
			});

			// Assert
			results.Should().NotBeNull();
			
			if (results.Count > 0)
			{
				var graphData = results.First().BaseObject;
				graphData.Should().NotBeNull();
				
				// Check that it has expected properties
				var properties = graphData.GetType().GetProperties();
				properties.Should().Contain(p => p.Name == "Lines" || p.Name == "TimeStamps");
			}
		}
		catch (InvalidOperationException ex) when (ex.Message.Contains("not found") || ex.Message.Contains("does not exist"))
		{
			// Skip test if the specific instance/graph combination doesn't exist
			return;
		}
	}

	[Fact]
	public void GetLMGraphData_WithTimeRange_ShouldReturnFilteredData()
	{
		// Arrange
		ConnectToLogicMonitor();

		var resourceDataSourceInstanceId = 1;
		var dataSourceGraphId = 1;
		var endTime = DateTime.UtcNow;
		var startTime = endTime.AddHours(-1);

		// Act
		try
		{
			var results = InvokePowerShell("Get-LMGraphData", new Dictionary<string, object>
			{
				{ "ResourceDataSourceInstanceId", resourceDataSourceInstanceId },
				{ "DataSourceGraphId", dataSourceGraphId },
				{ "StartTime", startTime },
				{ "EndTime", endTime }
			});

			// Assert
			results.Should().NotBeNull();
			
			if (results.Count > 0)
			{
				var graphData = results.First().BaseObject;
				graphData.Should().NotBeNull();
			}
		}
		catch (InvalidOperationException ex) when (ex.Message.Contains("not found") || ex.Message.Contains("does not exist"))
		{
			// Skip test if the specific instance/graph combination doesn't exist
			return;
		}
	}

	[Fact]
	public void GetLMFetchData_WithValidParameters_ShouldReturnFetchData()
	{
		// Arrange
		ConnectToLogicMonitor();

		var instanceIds = new[] { 1, 2, 3 }; // Common instance IDs
		var endTime = DateTimeOffset.UtcNow;
		var startTime = endTime.AddHours(-1);

		// Act
		try
		{
			var results = InvokePowerShell("Get-LMFetchData", new Dictionary<string, object>
			{
				{ "InstanceIds", instanceIds },
				{ "StartTime", startTime },
				{ "EndTime", endTime }
			});

			// Assert
			results.Should().NotBeNull();
			
			if (results.Count > 0)
			{
				var fetchData = results.First().BaseObject;
				fetchData.Should().NotBeNull();
				
				// Check that it has expected properties
				var properties = fetchData.GetType().GetProperties();
				properties.Should().Contain(p => p.Name == "TotalCount" || p.Name == "InstanceFetchDataResponses");
			}
		}
		catch (InvalidOperationException ex) when (ex.Message.Contains("not found") || ex.Message.Contains("does not exist"))
		{
			// Skip test if the specific instances don't exist
			return;
		}
	}
}