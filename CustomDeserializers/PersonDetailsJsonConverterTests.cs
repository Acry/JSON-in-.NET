using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonConvert.CustomDeserializers;

[JsonConverter(typeof(PersonDetailsJsonConverter))]
public class Person
{
	public Person(KeyValuePair<string, string> data, int age, string nickname)
	{
		FirstName = data.Key;
		FamilyName = data.Value;
		Age = age;
		Nickname = nickname;
	}

	public string Nickname { get; }
	public string FirstName { get; }
	public string FamilyName { get; }
	public int Age { get; }

	public override string ToString()
	{
		return $"Person: {FirstName} {FamilyName} aka {Nickname} is {Age}";
	}
}

[TestFixture]
public class PersonDetailsJsonConverterTests
{
	[Test]
	public void WithoutAttribute()
	{
		var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "CustomDeserializers",
			"PersonDetails.json");
		var json = File.ReadAllText(path);
		var p = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(json, new PersonDetailsJsonConverter());
		TestContext.WriteLine(p);
		p.FirstName.Should().Be("Vorname");
		p.FamilyName.Should().Be("Nachname");
		p.Age.Should().Be(26);
		p.Nickname.Should().Be("A fancy Nickname");
	}
	
	[Test]
	public void WithAttribute()
	{
		var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "CustomDeserializers",
			"PersonDetails.json");
		var json = File.ReadAllText(path);
		var p = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(json);
		TestContext.WriteLine(p);
		p.FirstName.Should().Be("Vorname");
		p.FamilyName.Should().Be("Nachname");
		p.Age.Should().Be(26);
		p.Nickname.Should().Be("A fancy Nickname");
	}
}