using LogicMonitor.Api.Backup;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Settings
{
	public class BackupTests : TestWithOutput
	{
		public BackupTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void Backup_ExceptLongRunningOnes()
		{
			var configurationBackup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(true)
			{
				DataSources = false,
				Logs = false
			}).ConfigureAwait(false);
			Assert.NotNull(configurationBackup);
		}

		[Fact]
		public async void Backup_Users()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { Users = true }).ConfigureAwait(false);
			Assert.NotNull(backup);
			Assert.NotNull(backup.RoleGroups);
			Assert.NotNull(backup.Roles);
			Assert.NotNull(backup.UserGroups);
			Assert.NotNull(backup.Users);

			var roleGroup = backup.RoleGroups[0];
			Assert.NotNull(roleGroup);

			var role = backup.Roles[0];
			Assert.NotNull(role);

			var userGroup = backup.UserGroups[0];
			Assert.NotNull(userGroup);

			var user = backup.Users[0];
			Assert.NotNull(user);

			// CreatedBy is populated
			Assert.False(string.IsNullOrWhiteSpace(user.CreatedBy));
		}

		[Fact]
		public async void Backup_Alerting()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { Alerting = true }).ConfigureAwait(false);

			Assert.NotNull(backup.AlertRules);
			Assert.NotEmpty(backup.AlertRules);

			Assert.NotNull(backup.EscalationChains);
			Assert.NotEmpty(backup.EscalationChains);

			var acmeEscalationChain = backup.EscalationChains.SingleOrDefault(ec => ec.Name == "ReportMagic Operations");
			Assert.NotNull(acmeEscalationChain);
		}

		[Fact]
		public async void Backup_Integrations()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { Integrations = true }).ConfigureAwait(false);

			Assert.NotNull(backup.Integrations);
			Assert.NotEmpty(backup.Integrations);
		}

		[Fact]
		public async void Backup_Dashboards()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { Dashboards = true }).ConfigureAwait(false);

			Assert.NotNull(backup.Dashboards);
			Assert.NotEmpty(backup.Dashboards);
			Assert.NotNull(backup.Widgets);
			Assert.NotEmpty(backup.Widgets);
		}

		[Fact]
		public async void Backup_AccountSettings()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { AccountSettings = true }).ConfigureAwait(false);

			Assert.NotNull(backup.CompanyLogo);
		}

		//[Fact(Skip = "Takes too long - testing individual items instead")]
		//public async void Backup_LogicModules()
		//{
		//	var backup = await DefaultPortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { LogicModules = true }).ConfigureAwait(false);

		//	Assert.NotNull(backup.AppliesToFunctions);
		//	Assert.NotEmpty(backup.AppliesToFunctions);

		//	Assert.NotNull(backup.ConfigSources);
		//	Assert.NotEmpty(backup.ConfigSources);

		//	Assert.NotNull(backup.DataSources);
		//	Assert.NotEmpty(backup.DataSources);

		//	Assert.NotNull(backup.EventSources);
		//	Assert.NotEmpty(backup.EventSources);

		//	Assert.NotNull(backup.JobMonitors);
		//	Assert.NotEmpty(backup.JobMonitors);

		//	Assert.NotNull(backup.PropertySources);
		//	Assert.NotEmpty(backup.PropertySources);

		//	Assert.NotNull(backup.SnmpSysOidMaps);
		//	Assert.NotEmpty(backup.SnmpSysOidMaps);
		//}

		[Fact]
		public async void Backup_AppliesToFunctions()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { AppliesToFunctions = true }).ConfigureAwait(false);

			Assert.NotNull(backup.AppliesToFunctions);
			Assert.NotEmpty(backup.AppliesToFunctions);
		}

		[Fact]
		public async void Backup_ConfigSources()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { ConfigSources = true }).ConfigureAwait(false);

			Assert.NotNull(backup.ConfigSources);
			Assert.NotEmpty(backup.ConfigSources);
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
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { EventSources = true }).ConfigureAwait(false);

			Assert.NotNull(backup.EventSources);
			Assert.NotEmpty(backup.EventSources);
		}

		[Fact]
		public async void Backup_JobMonitors()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { JobMonitors = true }).ConfigureAwait(false);

			Assert.NotNull(backup.JobMonitors);
			Assert.NotEmpty(backup.JobMonitors);
		}

		[Fact]
		public async void Backup_PropertySources()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { PropertySources = true }).ConfigureAwait(false);

			Assert.NotNull(backup);
			Assert.NotNull(backup.PropertySources);
			Assert.NotEmpty(backup.PropertySources);
		}

		[Fact]
		public async void SdtBackup()
		{
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false)
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
			var backup = await PortalClient.BackupAsync(new ConfigurationBackupSpecification(false) { SnmpSysOidMaps = true }).ConfigureAwait(false);

			Assert.NotNull(backup.SnmpSysOidMaps);
			Assert.NotEmpty(backup.SnmpSysOidMaps);
		}
	}
}