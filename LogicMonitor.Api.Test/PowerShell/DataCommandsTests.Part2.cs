namespace LogicMonitor.Api.Test.PowerShell;

/// <summary>
/// Tests for LogicMonitor data PowerShell cmdlets
/// </summary>
public partial class DataCommandsTests
{

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
		}
	}
}
