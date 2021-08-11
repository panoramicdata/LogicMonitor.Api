using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace LogicMonitor.Api.Converters
{
	/// <summary>
	/// A JSON creation converter
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class JsonCreationConverter<T> : JsonConverter
	{
		/// <summary>
		/// Create an instance of objectType, based properties in the JSON object
		/// </summary>
		/// <param name="objectType">type of object expected</param>
		/// <param name="jObject">
		/// contents of JSON object that will be deserialized
		/// </param>
		protected abstract T Create(Type objectType, JObject jObject);

		/// <inheritdoc />
		public override bool CanConvert(Type objectType)
			=> typeof(T).IsAssignableFrom(objectType);

		/// <inheritdoc />
		public override bool CanWrite => false;

		/// <inheritdoc />
		public override object ReadJson(JsonReader reader,
			Type objectType,
			object existingValue,
			JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}

			// Load JObject from stream
			var jObject = JObject.Load(reader);

			// Create target object based on JObject
			var target = Create(objectType, jObject);

			// Populate the object properties
			serializer.Populate(jObject.CreateReader(), target);

			return target;
		}
	}
}