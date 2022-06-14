using System.Reflection;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace JsonConvert;

[TestFixture]
public class PrivateResolverTests
{
    [Test]
    public void CustomContractResolver()
    {
        // works with private setter

        // For getter only see:
        // https://github.com/BrunoZell/PrivateSetterContractResolver/blob/master/PrivateSetterContractResolver/PrivateSetterContractResolver.cs
        // https://www.nuget.org/packages/PrivateSetterContractResolver/
        var expected = new ResolverTestClass
        {
            Name = "Meier"
        };

        // Setting Href via reflection, since no setter (field missing in constructor)
        // For test purpose, I would not suggest to use this in your runtime code like
        var field = typeof(ResolverTestClass).GetField("<Href>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);
        field!.SetValue(expected, "https://www.hevos.de");

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(expected, Formatting.Indented);
        var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<ResolverTestClass>(json, new JsonSerializerSettings
        {
            ContractResolver = new PrivateResolver()
        });
        actual!.Href.Should().Be("https://www.hevos.de");
    }
}

internal class ResolverTestClass
{
    public string Name;

    [JsonProperty] public string Href { get; private set; }
}

internal class PrivateResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);
        if (prop.Writable) return prop;
        var property = member as PropertyInfo;
        var hasPrivateSetter = property?.GetSetMethod(true) != null;
        prop.Writable = hasPrivateSetter;

        return prop;
    }
}