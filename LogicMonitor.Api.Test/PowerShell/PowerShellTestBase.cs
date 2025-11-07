using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace LogicMonitor.Api.Test.PowerShell;

/// <summary>
/// Base class for PowerShell cmdlet tests that provides shared infrastructure
/// </summary>
public abstract class PowerShellTestBase : TestWithOutput, IDisposable
{
	protected System.Management.Automation.PowerShell? PowerShellInstance { get; private set; }

	protected Runspace? TestRunspace { get; private set; }

	// Shared configuration from the main test fixture
	protected string Account => GetConfiguration()["LogicMonitor:Account"] ?? throw new InvalidOperationException("LogicMonitor:Account not configured");
	protected string AccessId => GetConfiguration()["LogicMonitor:AccessId"] ?? throw new InvalidOperationException("LogicMonitor:AccessId not configured");
	protected string AccessKey => GetConfiguration()["LogicMonitor:AccessKey"] ?? throw new InvalidOperationException("LogicMonitor:AccessKey not configured");

	protected PowerShellTestBase(ITestOutputHelper iTestOutputHelper, Fixture fixture)
		: base(iTestOutputHelper, fixture)
	{
		SetupPowerShell();
	}

	private IConfiguration GetConfiguration() => Fixture.GetService<IConfiguration>();

	private void SetupPowerShell()
	{
		// Create a PowerShell runspace
		var initialSessionState = InitialSessionState.CreateDefault();

		// Add the LogicMonitor module path
		var modulePath = GetModulePath();
		if (!string.IsNullOrEmpty(modulePath))
		{
			initialSessionState.ImportPSModule([modulePath]);
		}

		TestRunspace = RunspaceFactory.CreateRunspace(initialSessionState);
		TestRunspace.Open();

		PowerShellInstance = System.Management.Automation.PowerShell.Create();
		PowerShellInstance.Runspace = TestRunspace;
	}

	private static string GetModulePath()
	{
		// Get the path to the built LogicMonitor.PowerShell module
		var currentDir = Directory.GetCurrentDirectory();
		var projectDir = Path.GetDirectoryName(currentDir);
		while (projectDir != null && !Directory.Exists(Path.Combine(projectDir, "LogicMonitor.PowerShell")))
		{
			projectDir = Path.GetDirectoryName(projectDir);
		}

		if (projectDir != null)
		{
			var modulePath = Path.Combine(projectDir, "LogicMonitor.PowerShell", "bin", "Debug", "net9.0", "LogicMonitor.psd1");
			if (File.Exists(modulePath))
			{
				return modulePath;
			}
		}

		return string.Empty;
	}

	/// <summary>
	/// Establishes a connection to LogicMonitor for testing
	/// </summary>
	protected void ConnectToLogicMonitor()
	{
		if (PowerShellInstance is null)
		{
			throw new InvalidOperationException("PowerShell instance is not initialized.");
		}

		PowerShellInstance.Commands.Clear();
		PowerShellInstance.AddCommand("Connect-LogicMonitor")
			.AddParameter("Account", Account)
			.AddParameter("AccessId", AccessId)
			.AddParameter("AccessKey", AccessKey);

		var results = PowerShellInstance.Invoke();

		if (PowerShellInstance.HadErrors)
		{
			var errors = PowerShellInstance.Streams.Error.ReadAll();
			var errorMessage = string.Join(Environment.NewLine, errors.Select(e => e.ToString()));
			throw new InvalidOperationException($"Failed to connect to LogicMonitor: {errorMessage}");
		}

		Logger.LogInformation("Successfully connected to LogicMonitor account: {Account}", Account);
	}

	/// <summary>
	/// Disconnects from LogicMonitor
	/// </summary>
	protected void DisconnectFromLogicMonitor()
	{
		try
		{
			if (PowerShellInstance is null)
			{
				throw new InvalidOperationException("PowerShell instance is not initialized.");
			}

			PowerShellInstance.Commands.Clear();
			PowerShellInstance.AddCommand("Disconnect-LogicMonitor");
			PowerShellInstance.Invoke();
		}
		catch (Exception ex)
		{
			Logger.LogWarning(ex, "Error during disconnect");
		}
	}

	/// <summary>
	/// Executes a PowerShell command and returns the results
	/// </summary>
	protected Collection<PSObject> InvokePowerShell(string command)
	{
		if (PowerShellInstance is null)
		{
			throw new InvalidOperationException("PowerShell instance is not initialized.");
		}

		PowerShellInstance.Commands.Clear();
		PowerShellInstance.AddScript(command);
		var results = PowerShellInstance.Invoke();

		if (PowerShellInstance.HadErrors)
		{
			var errors = PowerShellInstance.Streams.Error.ReadAll();
			var errorMessage = string.Join(Environment.NewLine, errors.Select(e => e.ToString()));
			throw new InvalidOperationException($"PowerShell command failed: {errorMessage}");
		}

		return results;
	}

	/// <summary>
	/// Executes a PowerShell command with parameters and returns the results
	/// </summary>
	protected Collection<PSObject> InvokePowerShell(string commandName, Dictionary<string, object> parameters)
	{
		if (PowerShellInstance is null)
		{
			throw new InvalidOperationException("PowerShell instance is not initialized.");
		}

		PowerShellInstance.Commands.Clear();
		var command = PowerShellInstance.AddCommand(commandName);

		foreach (var param in parameters)
		{
			command.AddParameter(param.Key, param.Value);
		}

		var results = PowerShellInstance.Invoke();

		if (PowerShellInstance.HadErrors)
		{
			var errors = PowerShellInstance.Streams.Error.ReadAll();
			var errorMessage = string.Join(Environment.NewLine, errors.Select(e => e.ToString()));
			throw new InvalidOperationException($"PowerShell command '{commandName}' failed: {errorMessage}");
		}

		return results;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			DisconnectFromLogicMonitor();
			PowerShellInstance?.Dispose();
			TestRunspace?.Dispose();
		}
	}
}