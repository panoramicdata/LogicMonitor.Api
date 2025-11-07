using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Users;
using System.Management.Automation;

namespace LogicMonitor.PowerShell.Commands
{
	/// <summary>
	/// Gets LogicMonitor users
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "LMUser")]
	[OutputType(typeof(User[]))]
	public class GetLMUserCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// User ID to retrieve
		/// </summary>
		[Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int Id { get; set; }

		/// <summary>
		/// Username pattern
		/// </summary>
		[Parameter()]
		public string? Username { get; set; }

		/// <summary>
		/// Email pattern
		/// </summary>
		[Parameter()]
		public string? Email { get; set; }

		/// <summary>
		/// Maximum number of results to return
		/// </summary>
		[Parameter()]
		public int Take { get; set; } = 300;

		/// <summary>
		/// Number of results to skip
		/// </summary>
		[Parameter()]
		public int Skip { get; set; } = 0;

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage("Retrieving LogicMonitor users...");

				if (Id > 0)
				{
					// Get specific user by ID
					var user = Client!.GetAsync<User>(Id, CancellationToken.None)
							.GetAwaiter().GetResult();
					WriteObject(user);
					return;
				}

				// Build filter
				var filter = new Filter<User>
				{
					Take = Take,
					Skip = Skip
				};

				// Add filter conditions
				var filterItems = new List<FilterItem<User>>();

				if (!string.IsNullOrEmpty(Username))
				{
					filterItems.Add(new Eq<User>(nameof(User.UserName), Username));
				}

				if (!string.IsNullOrEmpty(Email))
				{
					filterItems.Add(new Eq<User>(nameof(User.Email), Email));
				}

				if (filterItems.Count != 0)
				{
					filter.FilterItems = filterItems;
				}

				// Get users
				var users = Client!.GetAllAsync(filter, CancellationToken.None)
				   .GetAwaiter().GetResult();

				WriteVerboseMessage($"Retrieved {users.Count} users.");
				WriteObject(users, true);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Creates a new LogicMonitor user
	/// </summary>
	[Cmdlet(VerbsCommon.New, "LMUser")]
	[OutputType(typeof(User))]
	public class NewLMUserCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Username
		/// </summary>
		[Parameter(Mandatory = true, Position = 0)]
		[ValidateNotNullOrEmpty]
		public string Username { get; set; } = string.Empty;

		/// <summary>
		/// First name
		/// </summary>
		[Parameter(Mandatory = true, Position = 1)]
		[ValidateNotNullOrEmpty]
		public string FirstName { get; set; } = string.Empty;

		/// <summary>
		/// Last name
		/// </summary>
		[Parameter(Mandatory = true, Position = 2)]
		[ValidateNotNullOrEmpty]
		public string LastName { get; set; } = string.Empty;

		/// <summary>
		/// Email address
		/// </summary>
		[Parameter(Mandatory = true, Position = 3)]
		[ValidateNotNullOrEmpty]
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// Password
		/// </summary>
		[Parameter(Mandatory = true, Position = 4)]
		[ValidateNotNullOrEmpty]
		public string Password { get; set; } = string.Empty;

		/// <summary>
		/// Role IDs to assign to the user
		/// </summary>
		[Parameter()]
		public int[]? RoleIds { get; set; }

		/// <summary>
		/// Force password change on first login
		/// </summary>
		[Parameter()]
		public SwitchParameter ForcePasswordChange { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Creating user: {Username}");

				var creationDto = new UserCreationDto
				{
					Username = Username,
					FirstName = FirstName,
					LastName = LastName,
					Email = Email,
					Password = Password,
					Password1 = Password,
					ForcePasswordChange = ForcePasswordChange.IsPresent,
					Status = "active"
				};

				// Add roles if specified
				if (RoleIds != null && RoleIds.Length > 0)
				{
					creationDto.Roles = [.. RoleIds.Select(id => new Role { Id = id })];
				}

				var user = Client!.CreateAsync(creationDto, CancellationToken.None)
				  .GetAwaiter().GetResult();

				WriteVerboseMessage($"Successfully created user: {user.UserName} (ID: {user.Id})");
				WriteObject(user);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Updates a LogicMonitor user
	/// </summary>
	[Cmdlet(VerbsCommon.Set, "LMUser")]
	[OutputType(typeof(User))]
	public class SetLMUserCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// User ID to update
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int Id { get; set; }

		/// <summary>
		/// New first name
		/// </summary>
		[Parameter()]
		public string? FirstName { get; set; }

		/// <summary>
		/// New last name
		/// </summary>
		[Parameter()]
		public string? LastName { get; set; }

		/// <summary>
		/// New email address
		/// </summary>
		[Parameter()]
		public string? Email { get; set; }

		/// <summary>
		/// Enable or disable the user
		/// </summary>
		[Parameter()]
		public bool? Enabled { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Updating user ID: {Id}");

				// Get the existing user
				var user = Client!.GetAsync<User>(Id, CancellationToken.None)
				 .GetAwaiter().GetResult();

				// Update properties
				if (!string.IsNullOrEmpty(FirstName))
				{
					user.FirstName = FirstName;
				}

				if (!string.IsNullOrEmpty(LastName))
				{
					user.LastName = LastName;
				}

				if (!string.IsNullOrEmpty(Email))
				{
					user.Email = Email;
				}

				if (Enabled.HasValue)
				{
					user.Status = Enabled.Value ? UserStatus.Active : UserStatus.Suspended;
				}

				// Update the user
				Client!.PutAsync(user, CancellationToken.None)
				  .GetAwaiter().GetResult();

				WriteVerboseMessage($"Successfully updated user: {user.UserName}");
				WriteObject(user);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Removes a LogicMonitor user
	/// </summary>
	[Cmdlet(VerbsCommon.Remove, "LMUser", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
	public class RemoveLMUserCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// User ID to remove
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int Id { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				// Get user info for confirmation
				var user = Client!.GetAsync<User>(Id, CancellationToken.None)
		  .GetAwaiter().GetResult();

				if (ShouldProcess($"User '{user.UserName}' (ID: {Id})", "delete user"))
				{
					WriteVerboseMessage($"Removing user: {user.UserName} (ID: {Id})");

					Client!.DeleteAsync(user, CancellationToken.None)
					.GetAwaiter().GetResult();

					WriteVerboseMessage($"Successfully removed user: {user.UserName}");
				}
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}
}