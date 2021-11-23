namespace LogicMonitor.Api;

/// <summary>
/// A progress reporter
/// </summary>
public class ProgressReporter
{
	private readonly ILogger _logger;
	private readonly Stopwatch _totalStopwatch;
	private readonly Stopwatch _subTaskStopwatch;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="logger"></param>
	public ProgressReporter(ILogger logger)
	{
		_totalStopwatch = new Stopwatch();
		_subTaskStopwatch = new Stopwatch();
		_logger = logger;
	}

	/// <summary>
	/// Start
	/// </summary>
	public void Start()
	{
		if (_totalStopwatch.IsRunning)
		{
			throw new InvalidOperationException("Already started.");
		}

		_totalStopwatch.Start();
	}

	/// <summary>
	/// Start a subtask
	/// </summary>
	/// <param name="subTaskName"></param>
	public void StartSubTask(string subTaskName)
	{
		CheckStarted();
		if (_subTaskStopwatch.IsRunning)
		{
			throw new InvalidOperationException("Subtask is running.  Call StopSubTask() first.");
		}

		Notify($"{subTaskName}...");
		_subTaskStopwatch.Start();
	}

	/// <summary>
	/// Stop a subtask
	/// </summary>
	public void StopSubTask()
	{
		CheckStarted();
		if (!_subTaskStopwatch.IsRunning)
		{
			throw new InvalidOperationException("No subtask running, Call StartSubTask() first.");
		}

		_subTaskStopwatch.Stop();
		var elapsed = _subTaskStopwatch.Elapsed;
		Notify($"done in {(int)elapsed.TotalMinutes}:{elapsed.Seconds:00}.{elapsed.Milliseconds:000}");
	}

	/// <summary>
	/// Complete a sub task and start a new one
	/// </summary>
	/// <param name="text"></param>
	public void CompleteSubTaskAndStartNew(string text)
	{
		CheckStarted();
		if (!_subTaskStopwatch.IsRunning)
		{
			throw new InvalidOperationException("No subtask running, Call StartSubTask() first.");
		}

		var elapsed = _subTaskStopwatch.Elapsed;
		Notify($"done in {(int)elapsed.TotalMinutes}:{elapsed.Seconds:00}.{elapsed.Milliseconds:000}");
		Notify($"{text}...");
		_subTaskStopwatch.Restart();
	}

	/// <summary>
	/// Notify of progress
	/// </summary>
	/// <param name="text"></param>
	public void Notify(string text) => _logger?.LogInformation(text);

	/// <summary>
	/// Stop
	/// </summary>
	public void Stop()
	{
		CheckStarted();
		if (_subTaskStopwatch.IsRunning)
		{
			throw new InvalidOperationException("Subtask is running.  Call StopSubTask() first.");
		}

		var elapsed = _totalStopwatch.Elapsed;
		Notify($"Total: {(int)elapsed.TotalMinutes}:{elapsed.Seconds:00}.{elapsed.Milliseconds:000}");
		_totalStopwatch.Stop();
	}

	private void CheckStarted()
	{
		if (!_totalStopwatch.IsRunning)
		{
			throw new InvalidOperationException("Call Start() first.");
		}
	}

	/// <summary>
	/// Start a new progress reporter
	/// </summary>
	/// <param name="logger"></param>
	public static ProgressReporter StartNew(ILogger logger)
	{
		var progressReporter = new ProgressReporter(logger);
		progressReporter.Start();
		return progressReporter;
	}
}
