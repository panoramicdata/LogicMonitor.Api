using System.Management.Automation;
using LogicMonitor.Api.Dashboards;
using LogicMonitor.Api.Collectors;
using LogicMonitor.Api.LogicModules;
using LogicMonitor.Api.Websites;
using LogicMonitor.Api.Filters;

namespace LogicMonitor.PowerShell.Commands
{
    /// <summary>
    /// Gets LogicMonitor dashboards
  /// </summary>
    [Cmdlet(VerbsCommon.Get, "LMDashboard")]
    [OutputType(typeof(Dashboard[]))]
    public class GetLMDashboardCommand : LogicMonitorCmdletBase
    {
 /// <summary>
   /// Dashboard ID to retrieve
        /// </summary>
 [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public int Id { get; set; }

     /// <summary>
        /// Dashboard name pattern
   /// </summary>
        [Parameter()]
        public string? Name { get; set; }

        /// <summary>
 /// Dashboard group ID
   /// </summary>
        [Parameter()]
        public int GroupId { get; set; }

   /// <summary>
     /// Maximum number of results to return
     /// </summary>
     [Parameter()]
        public int Take { get; set; } = 300;

        protected override void ProcessRecord()
        {
     try
     {
 EnsureConnection();

WriteVerboseMessage("Retrieving LogicMonitor dashboards...");

      if (Id > 0)
   {
   var dashboard = Client!.GetAsync<Dashboard>(Id, CancellationToken.None)
    .GetAwaiter().GetResult();
       WriteObject(dashboard);
      return;
     }

      var filter = new Filter<Dashboard> { Take = Take };
      var filterItems = new List<FilterItem<Dashboard>>();

      if (!string.IsNullOrEmpty(Name))
    {
         filterItems.Add(new Eq<Dashboard>(nameof(Dashboard.Name), Name));
        }

  if (GroupId > 0)
    {
      filterItems.Add(new Eq<Dashboard>(nameof(Dashboard.DashboardGroupId), GroupId.ToString()));
 }

       if (filterItems.Count != 0)
 {
      filter.FilterItems = filterItems;
    }

     var dashboards = Client!.GetAllAsync(filter, CancellationToken.None)
           .GetAwaiter().GetResult();

WriteVerboseMessage($"Retrieved {dashboards.Count} dashboards.");
      WriteObject(dashboards, true);
      }
 catch (Exception ex)
 {
      HandleApiException(ex);
 }
    }
    }

    /// <summary>
  /// Gets LogicMonitor dashboard groups
    /// </summary>
 [Cmdlet(VerbsCommon.Get, "LMDashboardGroup")]
    [OutputType(typeof(DashboardGroup[]))]
 public class GetLMDashboardGroupCommand : LogicMonitorCmdletBase
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public int Id { get; set; }

        [Parameter()]
    public string? Name { get; set; }

        [Parameter()]
        public int Take { get; set; } = 300;

        protected override void ProcessRecord()
     {
 try
{
         EnsureConnection();

          if (Id > 0)
       {
   var group = Client!.GetAsync<DashboardGroup>(Id, CancellationToken.None)
      .GetAwaiter().GetResult();
 WriteObject(group);
   return;
          }

          var filter = new Filter<DashboardGroup> { Take = Take };
          if (!string.IsNullOrEmpty(Name))
  {
     filter.FilterItems =
 [
		 new Eq<DashboardGroup>(nameof(DashboardGroup.Name), Name)
    ];
            }

        var groups = Client!.GetAllAsync(filter, CancellationToken.None)
      .GetAwaiter().GetResult();

        WriteObject(groups, true);
      }
   catch (Exception ex)
  {
     HandleApiException(ex);
  }
        }
    }

    /// <summary>
    /// Gets LogicMonitor collectors
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "LMCollector")]
    [OutputType(typeof(Collector[]))]
    public class GetLMCollectorCommand : LogicMonitorCmdletBase
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public int Id { get; set; }

        [Parameter()]
     public string? Description { get; set; }

       [Parameter()]
        public int Take { get; set; } = 300;

   protected override void ProcessRecord()
        {
        try
   {
    EnsureConnection();

         if (Id > 0)
     {
        var collector = Client!.GetAsync<Collector>(Id, CancellationToken.None)
 .GetAwaiter().GetResult();
   WriteObject(collector);
 return;
        }

   var filter = new Filter<Collector> { Take = Take };
       if (!string.IsNullOrEmpty(Description))
  {
         filter.FilterItems =
 [
		 new Eq<Collector>(nameof(Collector.Description), Description)
    ];
  }

    var collectors = Client!.GetAllAsync(filter, CancellationToken.None)
.GetAwaiter().GetResult();

     WriteObject(collectors, true);
     }
   catch (Exception ex)
       {
     HandleApiException(ex);
 }
       }
    }

    /// <summary>
/// Gets LogicMonitor collector groups
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "LMCollectorGroup")]
   [OutputType(typeof(CollectorGroup[]))]
    public class GetLMCollectorGroupCommand : LogicMonitorCmdletBase
    {
    [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
public int Id { get; set; }

    [Parameter()]
    public string? Name { get; set; }

        [Parameter()]
     public int Take { get; set; } = 300;

        protected override void ProcessRecord()
    {
   try
        {
     EnsureConnection();

     if (Id > 0)
    {
 var group = Client!.GetAsync<CollectorGroup>(Id, CancellationToken.None)
       .GetAwaiter().GetResult();
  WriteObject(group);
       return;
    }

        var filter = new Filter<CollectorGroup> { Take = Take };
    if (!string.IsNullOrEmpty(Name))
 {
       filter.FilterItems =
  [
new Eq<CollectorGroup>(nameof(CollectorGroup.Name), Name)
      ];
 }

   var groups = Client!.GetAllAsync(filter, CancellationToken.None)
        .GetAwaiter().GetResult();

   WriteObject(groups, true);
        }
catch (Exception ex)
 {
HandleApiException(ex);
        }
        }
    }

    /// <summary>
 /// Gets LogicMonitor data sources
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "LMDataSource")]
    [OutputType(typeof(DataSource[]))]
    public class GetLMDataSourceCommand : LogicMonitorCmdletBase
  {
    [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public int Id { get; set; }

   [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
 public int Take { get; set; } = 300;

        protected override void ProcessRecord()
   {
   try
         {
      EnsureConnection();

       if (Id > 0)
    {
  var dataSource = Client!.GetAsync<DataSource>(Id, CancellationToken.None)
      .GetAwaiter().GetResult();
    WriteObject(dataSource);
        return;
        }

       var filter = new Filter<DataSource> { Take = Take };
   if (!string.IsNullOrEmpty(Name))
      {
      filter.FilterItems =
		[
	  new Eq<DataSource>(nameof(DataSource.Name), Name)
    ];
       }

   var dataSources = Client!.GetAllAsync(filter, CancellationToken.None)
       .GetAwaiter().GetResult();

            WriteObject(dataSources, true);
            }
            catch (Exception ex)
            {
     HandleApiException(ex);
     }
        }
    }

  /// <summary>
    /// Gets LogicMonitor websites
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "LMWebsite")]
   [OutputType(typeof(Website[]))]
 public class GetLMWebsiteCommand : LogicMonitorCmdletBase
    {
     [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public int Id { get; set; }

   [Parameter()]
        public string? Name { get; set; }

    [Parameter()]
        public int Take { get; set; } = 300;

        protected override void ProcessRecord()
        {
        try
     {
   EnsureConnection();

     if (Id > 0)
        {
     var website = Client!.GetAsync<Website>(Id, CancellationToken.None)
 .GetAwaiter().GetResult();
       WriteObject(website);
   return;
    }

    var filter = new Filter<Website> { Take = Take };
     if (!string.IsNullOrEmpty(Name))
       {
   filter.FilterItems =
	 [
		new Eq<Website>(nameof(Website.Name), Name)
  ];
  }

    var websites = Client!.GetAllAsync(filter, CancellationToken.None)
 .GetAwaiter().GetResult();

    WriteObject(websites, true);
          }
  catch (Exception ex)
      {
    HandleApiException(ex);
 }
        }
 }

 /// <summary>
    /// Gets LogicMonitor website groups
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "LMWebsiteGroup")]
    [OutputType(typeof(WebsiteGroup[]))]
 public class GetLMWebsiteGroupCommand : LogicMonitorCmdletBase
    {
    [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public int Id { get; set; }

[Parameter()]
 public string? Name { get; set; }

 [Parameter()]
        public int Take { get; set; } = 300;

    protected override void ProcessRecord()
        {
        try
     {
        EnsureConnection();

  if (Id > 0)
            {
     var group = Client!.GetAsync<WebsiteGroup>(Id, CancellationToken.None)
  .GetAwaiter().GetResult();
WriteObject(group);
   return;
      }

  var filter = new Filter<WebsiteGroup> { Take = Take };
       if (!string.IsNullOrEmpty(Name))
      {
    filter.FilterItems =
	 [
		new Eq<WebsiteGroup>(nameof(WebsiteGroup.Name), Name)
    ];
  }

   var groups = Client!.GetAllAsync(filter, CancellationToken.None)
       .GetAwaiter().GetResult();

        WriteObject(groups, true);
      }
            catch (Exception ex)
     {
   HandleApiException(ex);
            }
        }
    }
}