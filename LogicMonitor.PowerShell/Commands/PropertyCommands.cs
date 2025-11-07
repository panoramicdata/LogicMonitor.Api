using LogicMonitor.Api;
using System.Management.Automation;

namespace LogicMonitor.PowerShell.Commands
{
	/// <summary>
	/// Gets custom properties for a LogicMonitor resource
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "LMResourceProperty")]
	[OutputType(typeof(EntityProperty[]))]
	public class GetLMResourcePropertyCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int ResourceId { get; set; }

		/// <summary>
		/// Property name (optional - returns all properties if not specified)
		/// </summary>
		[Parameter(Position = 1)]
		public string? PropertyName { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Retrieving properties for Resource ID: {ResourceId}");

				if (!string.IsNullOrEmpty(PropertyName))
				{
					// Get specific property - need to get all properties and filter
					var allProperties = Client!.GetResourcePropertiesAsync(ResourceId, CancellationToken.None)
						.GetAwaiter().GetResult();
					var property = allProperties.FirstOrDefault(p => p.Name.Equals(PropertyName, StringComparison.OrdinalIgnoreCase));

					if (property != null)
					{
						WriteObject(property);
					}
					else
					{
						throw new InvalidOperationException($"Property '{PropertyName}' not found for resource {ResourceId}");
					}
				}
				else
				{
					// Get all properties
					var properties = Client!.GetResourcePropertiesAsync(ResourceId, CancellationToken.None)
						.GetAwaiter().GetResult();
					WriteVerboseMessage($"Retrieved {properties.Count} properties.");
					WriteObject(properties, true);
				}
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Sets a custom property for a LogicMonitor resource
	/// </summary>
	[Cmdlet(VerbsCommon.Set, "LMResourceProperty")]
	[OutputType(typeof(EntityProperty))]
	public class SetLMResourcePropertyCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int ResourceId { get; set; }

		/// <summary>
		/// Property name
		/// </summary>
		[Parameter(Mandatory = true, Position = 1)]
		[ValidateNotNullOrEmpty]
		public string PropertyName { get; set; } = string.Empty;

		/// <summary>
		/// Property value
		/// </summary>
		[Parameter(Mandatory = true, Position = 2)]
		public string PropertyValue { get; set; } = string.Empty;

		/// <summary>
		/// Create the property if it doesn't exist
		/// </summary>
		[Parameter()]
		public SwitchParameter CreateIfNotExists { get; set; }

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				WriteVerboseMessage($"Setting property '{PropertyName}' = '{PropertyValue}' for Resource ID: {ResourceId}");

				// Use the SetResourceCustomPropertyAsync method which handles create/update automatically
				var mode = CreateIfNotExists.IsPresent ? SetPropertyMode.Automatic : SetPropertyMode.Update;

				Client!.SetResourceCustomPropertyAsync(ResourceId, PropertyName, PropertyValue, mode, CancellationToken.None)
					.GetAwaiter().GetResult();

				// Get the updated property to return
				var properties = Client!.GetResourcePropertiesAsync(ResourceId, CancellationToken.None)
					.GetAwaiter().GetResult();
				var updatedProperty = properties.FirstOrDefault(p => p.Name.Equals(PropertyName, StringComparison.OrdinalIgnoreCase));

				if (updatedProperty != null)
				{
					WriteVerboseMessage($"Successfully set property: {PropertyName}");
					WriteObject(updatedProperty);
				}
				else
				{
					throw new InvalidOperationException($"Failed to set property '{PropertyName}'");
				}
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}

	/// <summary>
	/// Removes a custom property from a LogicMonitor resource
	/// </summary>
	[Cmdlet(VerbsCommon.Remove, "LMResourceProperty", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
	public class RemoveLMResourcePropertyCommand : LogicMonitorCmdletBase
	{
		/// <summary>
		/// Resource ID
		/// </summary>
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public int ResourceId { get; set; }

		/// <summary>
		/// Property name
		/// </summary>
		[Parameter(Mandatory = true, Position = 1)]
		[ValidateNotNullOrEmpty]
		public string PropertyName { get; set; } = string.Empty;

		protected override void ProcessRecord()
		{
			try
			{
				EnsureConnection();

				if (ShouldProcess($"Resource ID {ResourceId}", $"remove property '{PropertyName}'"))
				{
					WriteVerboseMessage($"Removing property '{PropertyName}' from Resource ID: {ResourceId}");

					// Use the SetResourceCustomPropertyAsync method with null value to delete
					Client!.SetResourceCustomPropertyAsync(ResourceId, PropertyName, null, SetPropertyMode.Automatic, CancellationToken.None)
						.GetAwaiter().GetResult();

					WriteVerboseMessage($"Successfully removed property: {PropertyName}");
				}
			}
			catch (Exception ex)
			{
				HandleApiException(ex);
			}
		}
	}
}