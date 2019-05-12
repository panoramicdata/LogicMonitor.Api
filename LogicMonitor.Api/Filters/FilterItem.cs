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
			set
			{
				switch (value)
				{
					case Comparator.Eq:
						Operation = ":";
						break;
					case Comparator.Ge:
						Operation = ">:";
						break;
					case Comparator.Gt:
						Operation = ">";
						break;
					case Comparator.Includes:
						Operation = "~";
						break;
					case Comparator.Le:
						Operation = "<=";
						break;
					case Comparator.Lt:
						Operation = "<";
						break;
					case Comparator.Ne:
						Operation = "!:";
						break;
					case Comparator.NotIncludes:
						Operation = "!~";
						break;
					default:
						throw new NotSupportedException($"Unexpected Comparator: '{value}'");
				}
			}
		}

		/// <summary>
		///     The value
		/// </summary>
		[DataMember(Name = "value")]
		public object Value { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			var field = PortalClient.GetSerializationName<T>(Property);

			string valueString;
			switch (Value)
			{
				case bool boolValue:
					valueString = $"\"{boolValue.ToString().ToLowerInvariant()}\"";
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
						valueString = PortalClient.GetSerializationNameFromEnumMember(Value);
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