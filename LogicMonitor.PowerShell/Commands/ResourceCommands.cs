using System.Management.Automation;
using System.Collections;
using LogicMonitor.Api;
using LogicMonitor.Api.Resources;
using LogicMonitor.Api.Filters;

namespace LogicMonitor.PowerShell.Commands
{
    /// <summary>
    /// Gets LogicMonitor resources
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "LMResource")]
    [OutputType(typeof(Resource[]))]
    public class GetLMResourceCommand : LogicMonitorCmdletBase
    {
        /// <summary>
        /// Resource ID to retrieve
        /// </summary>
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public int Id { get; set; }

        /// <summary>
        /// Resource name pattern
     /// </summary>
    [Parameter()]
        public string? Name { get; set; }

   /// <summary>
        /// Resource display name pattern
     /// </summary>
        [Parameter()]
        public string? DisplayName { get; set; }

        /// <summary>
        /// Resource group ID
        /// </summary>
        [Parameter()]
        public int GroupId { get; set; }

   /// <summary>
      /// Filter hashtable for complex filtering
/// </summary>
        [Parameter()]
public Hashtable? Filter { get; set; }

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

        WriteVerboseMessage("Retrieving LogicMonitor resources...");

      if (Id > 0)
        {
            // Get specific resource by ID
            var resource = Client!.GetAsync<Resource>(Id, CancellationToken.None)
    .GetAwaiter().GetResult();
            WriteObject(resource);
               return;
}

                // Build filter
       var filter = new Filter<Resource>
         {
Take = Take,
        Skip = Skip
            };

        // Add filter conditions
            var filterItems = new List<FilterItem<Resource>>();

 if (!string.IsNullOrEmpty(Name))
{
       filterItems.Add(new Eq<Resource>(nameof(Resource.Name), Name));
       }

            if (!string.IsNullOrEmpty(DisplayName))
     {
           filterItems.Add(new Eq<Resource>(nameof(Resource.DisplayName), DisplayName));
    }

   if (GroupId > 0)
    {
filterItems.Add(new Eq<Resource>("resourceGroupIds", GroupId.ToString()));
       }

      if (filterItems.Count != 0)
            {
 filter.FilterItems = filterItems;
     }

      // Get resources
    var resources = Client!.GetAllAsync(filter, CancellationToken.None)
.GetAwaiter().GetResult();

        WriteVerboseMessage($"Retrieved {resources.Count} resources.");
         WriteObject(resources, true);
     }
       catch (Exception ex)
     {
            HandleApiException(ex);
     }
    }
    }

    /// <summary>
    /// Creates a new LogicMonitor resource
    /// </summary>
    [Cmdlet(VerbsCommon.New, "LMResource")]
    [OutputType(typeof(Resource))]
  public class NewLMResourceCommand : LogicMonitorCmdletBase
    {
  /// <summary>
        /// Resource name
        /// </summary>
 [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Resource display name
        /// </summary>
 [Parameter(Position = 1)]
        public string? DisplayName { get; set; }

        /// <summary>
    /// Resource group ID
        /// </summary>
        [Parameter(Mandatory = true, Position = 2)]
    public int ResourceGroupId { get; set; }

     /// <summary>
        /// Resource description
   /// </summary>
    [Parameter()]
public string? Description { get; set; }

        /// <summary>
/// Custom properties hashtable
        /// </summary>
        [Parameter()]
        public Hashtable? Properties { get; set; }

        protected override void ProcessRecord()
        {
  try
   {
   EnsureConnection();

      WriteVerboseMessage($"Creating resource: {Name}");

      var creationDto = new ResourceCreationDto
         {
         Name = Name,
            DisplayName = DisplayName ?? Name,
        ResourceGroupIds = ResourceGroupId.ToString(),
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

    var resource = Client!.CreateAsync(creationDto, CancellationToken.None)
   .GetAwaiter().GetResult();

        WriteVerboseMessage($"Successfully created resource: {resource.Name} (ID: {resource.Id})");
   WriteObject(resource);
            }
  catch (Exception ex)
      {
      HandleApiException(ex);
            }
     }
    }

    /// <summary>
	/// Updates a LogicMonitor resource
	/// </summary>
	[Cmdlet(VerbsCommon.Set, "LMResource")]
	[OutputType(typeof(Resource))]
	public class SetLMResourceCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource ID to update
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int Id { get; set; }

		/// <summary>
		/// New resource name
		/// </summary>
		[Parameter()]
		public string? Name { get; set; }

		/// <summary>
		/// New display name
		/// </summary>
		[Parameter()]
		public string? DisplayName { get; set; }

		/// <summary>
		/// New description
		/// </summary>
		[Parameter()]
		public string? Description { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Updating resource ID: {Id}");

				// Get the existing resource
				var resource = Client!.GetAsync<Resource>(Id, CancellationToken.None)
				 .GetAwaiter().GetResult();

				// Update properties
				if (!string.IsNullOrEmpty(Name))
				{
					resource.Name = Name;
				}

				if (!string.IsNullOrEmpty(DisplayName))
				{
					resource.DisplayName = DisplayName;
				}

				if (!string.IsNullOrEmpty(Description))
				{
					resource.Description = Description;
				}

				// Update the resource
				Client!.PutAsync(resource, CancellationToken.None)
					 .GetAwaiter().GetResult();

				WriteVerboseMessage($"Successfully updated resource: {resource.Name}");
				WriteObject(resource);
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Removes a LogicMonitor resource
	/// </summary>
	[Cmdlet(VerbsCommon.Remove, "LMResource", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
	public class RemoveLMResourceCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource ID to remove
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

				// Get resource info for confirmation
				var resource = Client!.GetAsync<Resource>(Id, CancellationToken.None)
			 .GetAwaiter().GetResult();

				var deleteType = HardDelete ? "permanently delete" : "delete";
				if (ShouldProcess($"Resource '{resource.Name}' (ID: {Id})", $"{deleteType} resource"))
				{
					WriteVerboseMessage($"Removing resource: {resource.Name} (ID: {Id})");

					Client!.DeleteAsync<Resource>(Id, HardDelete, CancellationToken.None)
								 .GetAwaiter().GetResult();

					WriteVerboseMessage($"Successfully removed resource: {resource.Name}");
				}
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}
}