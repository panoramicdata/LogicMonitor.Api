using System;
using System.Diagnostics;

namespace LogicMonitor.Api
{
	/// <summary>
	/// A progress reporter
	/// </summary>
	public class ProgressReporter
	{
		private readonly Action<string> _action;
		private readonly Stopwatch _totalStopwatch;
		private readonly Stopwatch _subTaskStopwatch;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="action"></param>
		public ProgressReporter(Action<string> action)
		{
			_totalStopwatch = new Stopwatch();
			_subTaskStopwatch = new Stopwatch();
			_action = action;
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

			_action?.Invoke($"{subTaskName}...");
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
			_action?.Invoke($"done in {(int)elapsed.TotalMinutes}:{elapsed.Seconds:00}.{elapsed.Milliseconds:000}\r\n");
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
			_action?.Invoke($"done in {(int)elapsed.TotalMinutes}:{elapsed.Seconds:00}.{elapsed.Milliseconds:000}\r\n");
			_action?.Invoke($"{text}...");
			_subTaskStopwatch.Restart();
		}

		/// <summary>
		/// Notify of progress
		/// </summary>
		/// <param name="text"></param>
		public void Notify(string text) => _action?.Invoke(text);

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
			_action?.Invoke($"Total: {(int)elapsed.TotalMinutes}:{elapsed.Seconds:00}.{elapsed.Milliseconds:000}\r\n");
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
		/// <param name="progressFunc"></param>
		/// <returns></returns>
		public static ProgressReporter StartNew(Action<string> progressFunc)
		{
			var progressReporter = new ProgressReporter(progressFunc);
			progressReporter.Start();
			return progressReporter;
		}
	}
}