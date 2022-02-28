namespace LogicMonitor.Api.Test.Settings;

public class BackupTests : TestWithOutput
{
	public BackupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void Backup_ExceptLongRunningOnes()
	{
		var configurationBackup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(true)
		{
			DataSources = false,
			Logs = false
		}).ConfigureAwait(false);
		configurationBackup.Should().NotBeNull();
	}

	[Fact]
	public async void Backup_Users()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { Users = true }).ConfigureAwait(false);
		backup.Should().NotBeNull();
		backup.RoleGroups.Should().NotBeNull();
		backup.Roles.Should().NotBeNull();
		backup.UserGroups.Should().NotBeNull();
		backup.Users.Should().NotBeNull();

		var roleGroup = backup.RoleGroups[0];
		roleGroup.Should().NotBeNull();

		var role = backup.Roles[0];
		role.Should().NotBeNull();

		var userGroup = backup.UserGroups[0];
		userGroup.Should().NotBeNull();

		var user = backup.Users[0];
		user.Should().NotBeNull();

		// CreatedBy is populated
		user.CreatedBy.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async void Backup_Alerting()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { Alerting = true }).ConfigureAwait(false);

		backup.AlertRules.Should().NotBeNullOrEmpty();

		backup.EscalationChains.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void Backup_Integrations()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { Integrations = true }).ConfigureAwait(false);

		backup.Integrations.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void Backup_Dashboards()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { Dashboards = true }).ConfigureAwait(false);

		backup.Dashboards.Should().NotBeNullOrEmpty();
		backup.Widgets.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void Backup_AccountSettings()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { AccountSettings = true }).ConfigureAwait(false);

		backup.CompanyLogo.Should().NotBeNull();
	}

	[Fact]
	public async void Backup_AppliesToFunctions()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { AppliesToFunctions = true }).ConfigureAwait(false);

		backup.AppliesToFunctions.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async void Backup_ConfigSources()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { ConfigSources = true }).ConfigureAwait(false);

		backup.ConfigSources.Should().NotBeNullOrEmpty();
	}


	[Fact]
	public async void Backup_ScheduledDownTimes()
	{
		var fileInfo = new FileInfo(Path.GetTempFileName());
		try
		{
			// Backup this specific item
			var backup = await LogicMonitorClient
				.BackupAsync(new ConfigurationBackupSpecification(false)
				{
					ScheduledDownTimes = true,
					GzipFileInfo = fileInfo
				})
				.ConfigureAwait(false);
			fileInfo.Exists.Should().BeTrue();
			backup.ScheduledDownTimes.Should().NotBeNullOrEmpty();

			// Load back from disk
			var reloadedBackup = await LogicMonitorClient
				.LoadBackupAsync(fileInfo)
				.ConfigureAwait(false);

			reloadedBackup.Should().NotBeNull();
			reloadedBackup.Should().BeEquivalentTo(backup);
		}
		finally
		{
			fileInfo.Delete();
			fileInfo.Exists.Should().BeFalse();
		}
	}

	//[Fact(Skip = "Takes too long")]
	//public async void Backup_DataSources()
	//{
	//	var backup = await DefaultPortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { DataSources = true }).ConfigureAwait(false);

	//	Assert.NotNull(backup.DataSources);
	//	Assert.NotEmpty(backup.DataSources);
	//}

	[Fact]
	public async void Backup_EventSources()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { EventSources = true }).ConfigureAwait(false);

		Assert.NotNull(backup.EventSources);
		Assert.NotEmpty(backup.EventSources);
	}

	[Fact]
	public async void Backup_JobMonitors()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { JobMonitors = true }).ConfigureAwait(false);

		Assert.NotNull(backup.JobMonitors);
		Assert.NotEmpty(backup.JobMonitors);
	}

	[Fact]
	public async void Backup_PropertySources()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { PropertySources = true }).ConfigureAwait(false);

		Assert.NotNull(backup);
		Assert.NotNull(backup.PropertySources);
		Assert.NotEmpty(backup.PropertySources);
	}

	[Fact]
	public async void SdtBackup()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false)
		{
			ScheduledDownTimes = true
		}).ConfigureAwait(false);

		Assert.NotNull(backup);
		Assert.NotNull(backup.ScheduledDownTimes);
		Assert.NotEmpty(backup.ScheduledDownTimes);
	}

	[Fact]
	public async void Backup_SnmpSysOidMaps()
	{
		var backup = await LogicMonitorClient.BackupAsync(new ConfigurationBackupSpecification(false) { SnmpSysOidMaps = true }).ConfigureAwait(false);

		Assert.NotNull(backup.SnmpSysOidMaps);
		Assert.NotEmpty(backup.SnmpSysOidMaps);
	}
}
