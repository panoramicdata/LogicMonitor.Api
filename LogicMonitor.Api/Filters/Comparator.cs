namespace LogicMonitor.Api.Filters;

/// <summary>
/// Comparator
/// </summary>
public enum Comparator
{
	/// <summary>
	/// Equals
	/// </summary>
	Eq,

	/// <summary>
	/// Greater than or equal to
	/// </summary>
	Ge,

	/// <summary>
	/// Greater than
	/// </summary>
	Gt,

	/// <summary>
	/// Includes
	/// </summary>
	Includes,

	/// <summary>
	/// Less than or equal to
	/// </summary>
	Le,

	/// <summary>
	/// Less than
	/// </summary>
	Lt,

	/// <summary>
	/// Not equal
	/// </summary>
	Ne,

	/// <summary>
	/// Does not include
	/// </summary>
	NotIncludes,

	/// <summary>
	/// Is null or empty
	/// </summary>
	IsNullOrEmpty,

	/// <summary>
	/// Is not null or empty
	/// </summary>
	IsNotNullOrEmpty,

	/// <summary>
	/// Is not null
	/// </summary>
	IsNotNull,

	/// <summary>
	/// Is null
	/// </summary>
	IsNull
}
