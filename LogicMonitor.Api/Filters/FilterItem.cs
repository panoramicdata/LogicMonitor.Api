using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Filters
{
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
		public string Property { get; set; }

		/// <summary>
		///     The operation
		/// </summary>
		[DataMember(Name = "op")]
		public string Operation { get; set; }

		/// <summary>
		///     The operation
		/// </summary>
		[IgnoreDataMember]
		public Comparator Comparator
		{
			set => Operation = value switch
			{
				Comparator.Eq => ":",
				Comparator.Ge => ">:",
				Comparator.Gt => ">",
				Comparator.Includes => "~",
				Comparator.Le => "<=",
				Comparator.Lt => "<",
				Comparator.Ne => "!:",
				Comparator.NotIncludes => "!~",
				_ => throw new NotSupportedException($"Unexpected Comparator: '{value}'"),
			};
		}

		/// <summary>
		///     The value
		/// </summary>
		[DataMember(Name = "value")]
		public object Value { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			var field = LogicMonitorClient.GetSerializationName<T>(Property);

			string valueString;
			switch (Value)
			{
				case bool boolValue:
					valueString = boolValue.ToString().ToLowerInvariant();
					break;

				case string text:
					valueString = $"\"{text}\"";
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
					}
					valueString = Value.ToString();
					break;
			}
			return field + Operation + valueString;
		}

		/// <summary>
		///     Creates output as a json string
		/// </summary>
		/// <returns></returns>
		public object ToJsonString() => JsonConvert.SerializeObject(this);
	}
}