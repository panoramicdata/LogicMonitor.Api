using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A big number item
	/// </summary>
	[DataContract]
	public class BigNumberItem
	{
		/// <summary>
		/// The position
		/// </summary>
		[DataMember(Name = "position")]
		public int Position { get; set; }

		/// <summary>
		/// The rightLabel
		/// </summary>
		[DataMember(Name = "rightLabel")]
		public string RightLabel { get; set; }

		/// <summary>
		/// The bottomLabel
		/// </summary>
		[DataMember(Name = "bottomLabel")]
		public string BottomLabel { get; set; }

		/// <summary>
		/// The dataPointName
		/// </summary>
		[DataMember(Name = "dataPointName")]
		public string DataPointName { get; set; }

		/// <summary>
		/// The rounding
		/// </summary>
		[DataMember(Name = "rounding")]
		public string Rounding { get; set; }

		/// <summary>
		/// Whether to use comma separators
		/// </summary>
		[DataMember(Name = "useCommaSeparators")]
		public bool UseCommaSeparators { get; set; }

		/// <summary>
		/// The color thresholds
		/// </summary>
		[DataMember(Name = "colorThresholds")]
		public List<ColorThreshold> ColorThresholds { get; set; }
	}
}