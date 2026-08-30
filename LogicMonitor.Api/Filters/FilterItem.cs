namespace LogicMonitor.Api.Filters;

/// <summary>
///     Extra filters
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public class FilterItem<T>
{
	/// <summary>
	///     The field
	/// </summary>
	[DataMember(Name = "name")]
	public string Property { get; set; } = string.Empty;

	/// <summary>
	///     The operation
	/// </summary>
	[DataMember(Name = "op")]
	public string Operation { get; set; } = string.Empty;

	/// <summary>
	///     The operation
	/// </summary>
	[IgnoreDataMember]
	public Comparator Comparator
	{
		get => Operation switch
		{
			":" => Comparator.Eq,
			":::null" => Comparator.IsNull,
			":::empty" => Comparator.IsNullOrEmpty,
			">:" => Comparator.Ge,
			">" => Comparator.Gt,
			"~" => Comparator.Includes,
			"<=" => Comparator.Le,
			"<" => Comparator.Lt,
			"!:" => Comparator.Ne,
			"!::null" => Comparator.IsNotNull,
			"!::empty" => Comparator.IsNotNullOrEmpty,
			"!~" => Comparator.NotIncludes,
			_ => throw new NotSupportedException($"Unexpected Operation: '{Operation}'"),
		};
		set => Operation = value switch
		{
			Comparator.Eq => ":",
			Comparator.IsNull => ":::null",
			Comparator.IsNullOrEmpty => ":::empty",
			Comparator.Ge => ">:",
			Comparator.Gt => ">",
			Comparator.Includes => "~",
			Comparator.Le => "<=",
			Comparator.Lt => "<",
			Comparator.Ne => "!:",
			Comparator.IsNotNull => "!::null",
			Comparator.IsNotNullOrEmpty => "!::empty",
			Comparator.NotIncludes => "!~",
			_ => throw new NotSupportedException($"Unexpected Comparator: '{value}'"),
		};
	}

	/// <summary>
	///     The value
	/// </summary>
	[DataMember(Name = "value")]
	public object? Value { get; set; }

	/// <inheritdoc />
	public override string ToString()
	{
		var field = LogicMonitorClient.GetSerializationName<T>(Property);

		ValidateValueAgainstOperation();

		return field + Operation + GetValueString();
	}

	/// <summary>
	/// The null/empty operations carry no value; every other operation requires one.
	/// </summary>
	private void ValidateValueAgainstOperation()
	{
		if (_valuelessOperations.Contains(Operation))
		{
			if (Value is not null)
			{
				throw new InvalidOperationException($"Value must be null for the '{Operation}' operation.");
			}

			return;
		}

		if (Value is null)
		{
			throw new InvalidOperationException($"Value must not be null for the '{Operation}' operation.");
		}
	}

	private static readonly HashSet<string> _valuelessOperations =
		new(StringComparer.Ordinal) { ":::empty", ":::null", "!::empty", "!::null" };

	/// <summary>
	/// Renders the value the way the portal's filter syntax expects it: booleans lowercase and
	/// bare, strings and enum members quoted, and lists quoted and pipe-separated.
	/// </summary>
	private string GetValueString() => Value switch
	{
		bool boolValue => boolValue.ToString().ToLowerInvariant(),
		string text => $"\"{text}\"",
		null => string.Empty,
		IEnumerable enumerable => string.Join("|", enumerable.Cast<object>().Select(item => $"\"{item}\"")),
		_ => Value.GetType().IsEnum
			? $"\"{LogicMonitorClient.GetSerializationNameFromEnumMember(Value)}\""
			: Value.ToString() ?? string.Empty,
	};

	/// <summary>
	///     Creates output as a json string
	/// </summary>
	public object ToJsonString() => JsonConvert.SerializeObject(this);
}
