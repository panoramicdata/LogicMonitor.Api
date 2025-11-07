using LogicMonitor.Api;
using LogicMonitor.Api.Filters;
using LogicMonitor.Api.Resources;
using System.Collections;
using System.Management.Automation;

namespace LogicMonitor.PowerShell.Commands
{
	/// <summary>
	/// Gets LogicMonitor resource groups
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "LMResourceGroup")]
	[OutputType(typeof(ResourceGroup[]))]
	public class GetLMResourceGroupCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource group ID to retrieve
		/// </summary>
		[Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int Id { get; set; }

		/// <summary>
		/// Resource group name pattern
		/// </summary>
		[Parameter()]
		public string? Name { get; set; }

		/// <summary>
		/// Parent group ID
		/// </summary>
		[Parameter()]
		public int ParentId { get; set; }

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

				WriteVerboseMessage("Retrieving LogicMonitor resource groups...");

				if (Id > 0)
				{
					// Get specific resource group by ID
					var resourceGroup = Client!.GetAsync<ResourceGroup>(Id, CancellationToken.None)
							.GetAwaiter().GetResult();
					WriteObject(resourceGroup);
					return;
				}

				// Build filter
				var filter = new Filter<ResourceGroup>
				{
					Take = Take,
					Skip = Skip
				};

				// Add filter conditions
				var filterItems = new List<FilterItem<ResourceGroup>>();

				if (!string.IsNullOrEmpty(Name))
				{
					filterItems.Add(new Eq<ResourceGroup>(nameof(ResourceGroup.Name), Name));
				}

				if (ParentId > 0)
				{
					filterItems.Add(new Eq<ResourceGroup>(nameof(ResourceGroup.ParentId), ParentId.ToString()));
				}

				if (filterItems.Count != 0)
				{
					filter.FilterItems = filterItems;
				}

				// Get resource groups
				var resourceGroups = Client!.GetAllAsync(filter, CancellationToken.None)
				   .GetAwaiter().GetResult();

				WriteVerboseMessage($"Retrieved {resourceGroups.Count} resource groups.");
				WriteObject(resourceGroups, true);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Creates a new LogicMonitor resource group
	/// </summary>
	[Cmdlet(VerbsCommon.New, "LMResourceGroup")]
	[OutputType(typeof(ResourceGroup))]
	public class NewLMResourceGroupCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource group name
		/// </summary>
		[Parameter(Mandatory = true, Position = 0)]
		[ValidateNotNullOrEmpty]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Parent group ID (default: root group)
		/// </summary>
		[Parameter(Position = 1)]
		public int ParentId { get; set; } = 1;

		/// <summary>
		/// Resource group description
		/// </summary>
		[Parameter()]
		public string? Description { get; set; }

		/// <summary>
		/// Custom properties hash table
		/// </summary>
		[Parameter()]
		public Hashtable? Properties { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Creating resource group: {Name}");

				var creationDto = new ResourceGroupCreationDto
				{
					Name = Name,
					ParentId = ParentId.ToString(),
					Description = Description ?? ""
				};

				// Add custom properties
				if (Properties != null)
				{
					creationDto.CustomProperties = [];
					foreach (var key in Properties.Keys)
					{
						creationDto.CustomProperties.Add(new EntityProperty
						{
							Name = key.ToString() ?? "",
							Value = Properties[key]?.ToString() ?? ""
						});
					}
				}

				var resourceGroup = Client!.CreateAsync(creationDto, CancellationToken.None)
				  .GetAwaiter().GetResult();

				WriteVerboseMessage($"Successfully created resource group: {resourceGroup.Name} (ID: {resourceGroup.Id})");
				WriteObject(resourceGroup);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Updates a LogicMonitor resource group
	/// </summary>
	[Cmdlet(VerbsCommon.Set, "LMResourceGroup")]
	[OutputType(typeof(ResourceGroup))]
	public class SetLMResourceGroupCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource group ID to update
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int Id { get; set; }

		/// <summary>
		/// New resource group name
		/// </summary>
		[Parameter()]
		public string? Name { get; set; }

		/// <summary>
		/// New description
		/// </summary>
		[Parameter()]
		public string? Description { get; set; }

		/// <summary>
		/// New parent group ID
		/// </summary>
		[Parameter()]
		public int ParentId { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Updating resource group ID: {Id}");

				// Get the existing resource group
				var resourceGroup = Client!.GetAsync<ResourceGroup>(Id, CancellationToken.None)
				 .GetAwaiter().GetResult();

				// Update properties
				if (!string.IsNullOrEmpty(Name))
				{
					resourceGroup.Name = Name;
				}

				if (!string.IsNullOrEmpty(Description))
				{
					resourceGroup.Description = Description;
				}

				if (ParentId > 0)
				{
					resourceGroup.ParentId = ParentId;
				}

				// Update the resource group
				Client!.PutAsync(resourceGroup, CancellationToken.None)
				  .GetAwaiter().GetResult();

				WriteVerboseMessage($"Successfully updated resource group: {resourceGroup.Name}");
				WriteObject(resourceGroup);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Removes a LogicMonitor resource group
	/// </summary>
	[Cmdlet(VerbsCommon.Remove, "LMResourceGroup", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
	public class RemoveLMResourceGroupCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource group ID to remove
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int Id { get; set; }

		/// <summary>
		/// Perform hard delete (permanent removal)
		/// </summary>
		[Parameter()]
		public SwitchParameter HardDelete { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				// Get resource group info for confirmation
				var resourceGroup = Client!.GetAsync<ResourceGroup>(Id, CancellationToken.None)
		  .GetAwaiter().GetResult();

				var deleteType = HardDelete ? "permanently delete" : "delete";
				if (ShouldProcess($"Resource Group '{resourceGroup.Name}' (ID: {Id})", $"{deleteType} resource group"))
				{
					WriteVerboseMessage($"Removing resource group: {resourceGroup.Name} (ID: {Id})");

					Client!.DeleteAsync<ResourceGroup>(Id, HardDelete, CancellationToken.None)
					.GetAwaiter().GetResult();

					WriteVerboseMessage($"Successfully removed resource group: {resourceGroup.Name}");
				}
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}
}