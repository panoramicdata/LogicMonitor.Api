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

		// NB the timestamps must be removed by index, one at a time, as above. Removing them by
		// value is wrong: timestamps can be duplicated (crazy, right?), so a single removal by
		// value could drop e.g. 2 timestamps while only 1 is dropped from the line data, and the
		// data and timestamp counts MUST match.

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
