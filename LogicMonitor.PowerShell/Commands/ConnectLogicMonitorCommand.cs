using System.Management.Automation;
using LogicMonitor.Api;
using Microsoft.Extensions.Logging;

namespace LogicMonitor.PowerShell.Commands
{
	/// <summary>
	/// Establishes a connection to LogicMonitor
	/// </summary>
	[Cmdlet(VerbsCommunications.Connect, "LogicMonitor")]
	[OutputType(typeof(LogicMonitorConnectionInfo))]
	public class ConnectLogicMonitorCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// The LogicMonitor account name (subdomain)
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "LogicMonitor account name (subdomain)")]
		[ValidateNotNullOrEmpty]
		public string Account { get; set; } = string.Empty;

		/// <summary>
		/// The API Access ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 1, HelpMessage = "LogicMonitor API Access ID")]
		[ValidateNotNullOrEmpty]
		public string AccessId { get; set; } = string.Empty;

		/// <summary>
		/// The API Access Key
		/// </summary>
		[Parameter(Mandatory = true, Position = 2, HelpMessage = "LogicMonitor API Access Key")]
		[ValidateNotNullOrEmpty]
		public string AccessKey { get; set; } = string.Empty;

		/// <summary>
		/// HTTP timeout in seconds (default: 60)
		/// </summary>
		[Parameter(HelpMessage = "HTTP timeout in seconds")]
		public int TimeoutSeconds { get; set; } = 60;

		/// <summary>
		/// Maximum backoff time in seconds for rate limiting (default: 300)
		/// </summary>
		[Parameter(HelpMessage = "Maximum backoff time in seconds for rate limiting")]
		public int MaxBackoffSeconds { get; set; } = 300;

		/// <summary>
		/// Enable verbose logging
		/// </summary>
		[Parameter(HelpMessage = "Enable verbose API logging")]
		public SwitchParameter EnableLogging { get; set; }

		/// <summary>
		/// Force connection even if already connected
		/// </summary>
		[Parameter(HelpMessage = "Force new connection even if already connected")]
		public SwitchParameter Force { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				// Check if already connected
				if (Client != null && !Force)
				{
					WriteWarning("Already connected to LogicMonitor. Use -Force to create a new connection.");
					WriteObject(ConnectionInfo);
					return;
				}

				WriteVerboseMessage($"Connecting to LogicMonitor account: {Account}");

				// Create logger if logging is enabled
				ILogger? logger = null;
				if (EnableLogging)
				{
					var loggerFactory = LoggerFactory.Create(builder =>
						builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
					logger = loggerFactory.CreateLogger<LogicMonitorClient>();
				}

				// Create client options
				var options = new LogicMonitorClientOptions
				{
					Account = Account,
					AccessId = AccessId,
					AccessKey = AccessKey,
					HttpClientTimeoutSeconds = TimeoutSeconds,
					MaximumBackOffSeconds = MaxBackoffSeconds,
					Logger = logger
				};

				// Create the client
				Client = new LogicMonitorClient(options);

				// Test the connection by getting account information
				WriteVerboseMessage("Testing connection...");
				var _ = Client.GetAsync<Api.Settings.AccountSettings>(CancellationToken.None).GetAwaiter().GetResult();

				// Create connection info
				ConnectionInfo = new LogicMonitorConnectionInfo
				{
					Account = Account,
					AccessId = AccessId,
					ConnectedAt = DateTime.Now,
					IsConnected = true
				};

				WriteVerboseMessage($"Successfully connected to LogicMonitor account: {Account}");
				WriteObject(ConnectionInfo);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}
}