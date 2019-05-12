using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// An AppliesTo Function creation DTO
	/// </summary>
	[DataContract]
	public class AppliesToFunctionCreationDto : CreationDto<AppliesToFunction>
	{
		/// <summary>
		/// The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// The description
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		/// <summary>
		/// The code
		/// </summary>
		[DataMember(Name = "code")]
		public string Code { get; set; }
	}
}