using System;
using FluentAssertions;
using NUnit.Framework;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace JsonConvert;

public class MicrosoftBasicExamples
{

    private const string G = "2905cfaf-d13d-4df1-af83-e4dcde20d44f";

    [Test]
    public void Test()
    {
        var expected = new TestRecord1(new Guid(G));
        var json = JsonSerializer.Serialize(expected);
        var actual = Newtonsoft.Json.JsonConvert.DeserializeObject<TestRecord1>(json);
        actual.Should().BeEquivalentTo(expected);
    }
}
