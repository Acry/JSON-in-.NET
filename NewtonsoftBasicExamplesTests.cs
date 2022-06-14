using System;
using System.Reflection;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonConvert;

internal record TestRecord1(Guid Id)
{
}

internal record TestRecord2
{
    public Guid Id;
    public string Name;
}

internal class TestClass
{
    public string Name;
    public string Href { get; }
}

internal class TestClassA
{
    public string Name;

    [JsonProperty] public string Href { get; private set; }
}

internal struct TestStruct
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}

public class NewtonsoftBasicExamplesTests
{
    private const string G = "2905cfaf-d13d-4df1-af83-e4dcde20d44f";

    [Test]
    public void Test0A()
    {
        // If you already have a string representation of the Guid, you can do this:
        var expected = new TestStruct {Id = new Guid(G), Name = "Müller"};

        // If you want a brand new Guid then just do
        // var g = Guid.NewGuid(); // convenient static method that you can call to get a new Guid
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expected, Formatting.Indented);
        var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<TestStruct>(json);
        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Test1A()
    {
        // If you already have a string representation of the Guid, you can do this:
        var expected = new TestRecord1(new Guid(G));

        // If you want a brand new Guid then just do
        // var g = Guid.NewGuid(); // convenient static method that you can call to get a new Guid
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expected, Formatting.Indented);
        var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<TestRecord1>(json);
        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Test1B()
    {
        var expected = new TestRecord2 {Id = new Guid(G), Name = "Müller"};
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expected, Formatting.Indented);
        var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<TestRecord2>(json);
        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ProblemMissingSetter()
    {
        var expected = new TestClass
        {
            Name = "Müller"
        };

        // Setting Href via reflection, since no setter (field missing in constructor)
        // For test purpose, I would not suggest to use this in your runtime code like
        var field = typeof(TestClass).GetField("<Href>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        field!.SetValue(expected, "https://www.hevos.de");

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expected, Formatting.Indented);
        var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<TestClass>(json);
        actual!.Href.Should().BeNull();
    }

    [Test]
    public void ProblemMissingSetterSolutionA()
    {
        // Works with private setter
        var expected = new TestClassA
        {
            Name = "Müller"
        };

        // Setting Href via reflection, since no setter (field missing in constructor)
        // For test purpose, I would not suggest to use this in your runtime code like
        var field = typeof(TestClassA).GetField("<Href>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);
        field!.SetValue(expected, "https://www.hevos.de");

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expected, Formatting.Indented);
        var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<TestClassA>(json);
        actual!.Href.Should().Be("https://www.hevos.de");
    }

    [Test]
    public void Test1C()
    {
        var expected = new TestRecord2();
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expected, Formatting.Indented);
        var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<TestRecord2>(json);
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void Test3()
    {
        var guid = Guid.Parse(G);
        var expected = new TestRecord1(guid);
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expected, Formatting.Indented);
        var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<TestRecord1>(json);
        actual.Should().BeEquivalentTo(expected);
    }
}
