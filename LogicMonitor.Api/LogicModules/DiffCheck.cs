using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A diff check
	/// </summary>
	[DataContract]
	public class DiffCheck
	{
		/// <summary>
		/// Whether to ignore blank lines
		/// </summary>
		[DataMember(Name = "ignore_blank_lines")]
		public bool AreBlankLinesIgnored { get; set; }

		/// <summary>
		/// Whether to ignore white space
		/// </summary>
		[DataMember(Name = "ignore_space")]
		public bool IsWhiteSpaceIgnored { get; set; }

		/// <summary>
		/// Ignore lines containing this string
		/// </summary>
		[DataMember(Name = "ignore_line_contain")]
		public List<string> IgnoreLinesContainingList { get; set; }

		/// <summary>
		/// Ignore lines matching this regular expression
		/// </summary>
		[DataMember(Name = "ignore_line_with_regex")]
		public List<string> IgnoreLinesMatchingRegexList { get; set; }

		/// <summary>
		/// Ignored lines
		/// </summary>
		[DataMember(Name = "ignore_line_start_with")]
		public List<string> IgnoreLinesStartingWithList { get; set; }
	}
}