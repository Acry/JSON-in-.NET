using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonConvert.CustomDeserializers;

public class PersonDetailsJsonConverter : JsonConverter
{
	public override bool CanWrite => false;

	public override bool CanConvert(Type objectType)
	{
		return typeof (Person).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		var jObject = JObject.Load(reader);
		var firstName = jObject.Property("blubb").Value.Value<string>();
		var familyName = jObject.Property(nameof(Person.FamilyName)).Value.Value<string>();
		var age = jObject.Property(nameof(Person.Age)).Value.Value<int>();
		// var nickname = (string)jObject["nickname"];
		var nickname = jObject.Property(nameof(Person.Nickname).ToLower()).Value.Value<string>();

		var kvp = new KeyValuePair<string, string>(firstName, familyName);

		return new Person(kvp, age, nickname);
	}
}