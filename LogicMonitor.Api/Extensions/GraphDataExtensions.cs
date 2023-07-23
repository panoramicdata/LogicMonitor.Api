namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Graph Data extensions
/// </summary>
public static class GraphDataExtensions
{
	private static readonly long timeThreshold = 2_000;

	/// <summary>
	/// Removes invalid LogicMonitor data
	/// </summary>
	/// <param name="graphData">The GraphData</param>
	/// <returns>A list of timestamp indexes</returns>
	private static List<(long Timestamp, int Index)> GetTimestampToRemove(this GraphData graphData)
	{
		var timestampToRemove = new List<(long, int)>();
		for (var index = 0; index < graphData.TimeStamps.Count; index++)
		{
			if (index == 0)
			{
				continue;
			}

			if (graphData.TimeStamps[index - 1] >= (graphData.TimeStamps[index] - timeThreshold))
			{
				timestampToRemove.Add((graphData.TimeStamps[index - 1], index - 1));
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
		var timestampsAndIndexes = graphData.GetTimestampToRemove();
		timestampsAndIndexes.Reverse();

		// Remove timestamps from the data
		graphData.TimeStamps.RemoveAll(timestamp => timestampsAndIndexes.Exists(ts => ts.Timestamp == timestamp));

		// Remove line data at those indexes
		foreach (var line in graphData.Lines)
		{
			var lineData = line.Data.ToList();
			foreach (var (_, Index) in timestampsAndIndexes)
			{
				lineData.RemoveAt(Index);
			}
			// Copy the data back
			line.Data = lineData.ToArray();
		}
	}
}
