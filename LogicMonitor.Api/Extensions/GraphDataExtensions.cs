namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Graph Data extensions
/// </summary>
public static class GraphDataExtensions
{
	private static readonly long _timeThreshold = 2_000;

	/// <summary>
	/// Removes invalid LogicMonitor data
	/// </summary>
	/// <param name="graphData">The GraphData</param>
	/// <returns>A list of timestamp indexes</returns>
	private static List<int> GetTimestampToRemove(this GraphData graphData)
	{
		var timestampToRemove = new List<int>();
		for (var index = 0; index < graphData.TimeStamps.Count; index++)
		{
			if (index == 0)
			{
				continue;
			}

			if (graphData.TimeStamps[index - 1] >= (graphData.TimeStamps[index] - _timeThreshold))
			{
				timestampToRemove.Add(index - 1);
			}
		}

		return timestampToRemove;
	}

	/// <summary>
	/// Update the invalid graph data
	/// </summary>
	/// <param name="graphData"></param>
	public static void RemoveInvalidDataPoints(this GraphData graphData)
	{
		// Get invalid timestamps
		var timestampIndexes = graphData.GetTimestampToRemove();
		timestampIndexes.Reverse();

		// Remove timestamps from the data
		foreach (var index in timestampIndexes)
		{
			graphData.TimeStamps.RemoveAt(index);
		}

		//NO: what can happen is that some timestamps are duplicated (crazy, right?) and this could remove e.g. 2 but
		// then we only were removing 1 in the line data. That's not right as the data and timestamp counts MUST match
		//graphData.TimeStamps.RemoveAll(timestamp => timestampsAndIndexes.Exists(ts => ts.Timestamp == timestamp));

		// Remove line data at those indexes
		foreach (var line in graphData.Lines)
		{
			var lineData = line.Data.ToList();
			foreach (var index in timestampIndexes)
			{
				if (lineData.Count > index)
				{
					lineData.RemoveAt(index);
				}
			}
			// Copy the data back
			line.Data = lineData;
		}
	}
}
