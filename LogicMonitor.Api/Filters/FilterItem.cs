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

		switch (Operation)
		{
			case ":::empty":
			case ":::null":
			case "!::empty":
			case "!::null":
				if (Value is not null)
				{
					throw new InvalidOperationException($"Value must be null for the '{Operation}' operation.");
				}

				break;
			default:
				if (Value is null)
				{
					throw new InvalidOperationException($"Value must not be null for the '{Operation}' operation.");
				}

				break;
		}

		string valueString;
		switch (Value)
		{
			case bool boolValue:
				valueString = boolValue.ToString().ToLowerInvariant();
				break;

			case string text:
				valueString = $"\"{text}\"";
				break;

			case null:
				valueString = string.Empty;
				break;

			default:
				if (Value is IEnumerable enumerable)
				{
					valueString = string.Join("|", enumerable.Cast<object>().Select(item => $"\"{item}\""));
					break;
				}
				else if (Value.GetType().IsEnum)
				{
					valueString = LogicMonitorClient.GetSerializationNameFromEnumMember(Value);
					break;
				}

				valueString = Value.ToString();
				break;
		}

		return field + Operation + valueString;
	}

	/// <summary>
	///     Creates output as a json string
	/// </summary>
	public object ToJsonString() => JsonConvert.SerializeObject(this);
}
