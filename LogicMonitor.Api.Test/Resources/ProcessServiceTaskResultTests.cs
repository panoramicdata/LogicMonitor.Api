namespace LogicMonitor.Api.Test.Resources;

// Bodies captured from device/devices/{id}/fetchProcessServiceTask/{taskId} on 2026-11-04
public class ProcessServiceTaskResultTests
{
	[Fact]
	public void TaskRunning_ReturnsNull()
	{
		var response = JObject.Parse("""{ "errorMessage": "The task is running", "errorCode": 1202, "errorDetail": null }""");

		LogicMonitorClient.ReadProcessServiceTaskResult(response).Should().BeNull();
	}

	[Fact]
	public void TaskComplete_WithoutErrorFields_ReturnsThePage()
	{
		var response = JObject.Parse("""
			{
				"total": 1,
				"items": [
					{ "id": "wuauserv", "name": "wuauserv", "description": "Windows Update", "alert": false, "monitor": false, "comment": "", "flag": 0, "param": "" }
				],
				"searchId": null,
				"isMin": false
			}
			""");

		var page = LogicMonitorClient.ReadProcessServiceTaskResult(response);

		page.Should().NotBeNull();
		page!.TotalCount.Should().Be(1);
		page.Items.Should().ContainSingle().Which.Name.Should().Be("wuauserv");
	}

	[Fact]
	public void TaskFailed_Throws()
	{
		var response = JObject.Parse("""{ "errorMessage": "Collector unreachable", "errorCode": 1500, "errorDetail": null }""");

		var act = () => LogicMonitorClient.ReadProcessServiceTaskResult(response);

		act.Should().Throw<LogicMonitorApiException>().WithMessage("*1500*Collector unreachable*");
	}
}
