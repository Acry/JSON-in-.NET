using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonConvert;

/// <summary>
///     Working with JObjects, JProperty and JTokens:
/// </summary>
/// <remarks>
///     JObjects can be enumerated via JProperty objects by casting it to a JToken:
///     http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JObject.htm
///     http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JProperty.htm
///     http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JToken.htm
/// </remarks>
public class JsonObjects
{
	[Test]
	public void Case1()
	{
		var o = JObject.Parse(@"{'string1':'foo','integer2':99,'datetime3':'2000-05-23T00:00:00'}");
		TestContext.WriteLine(o.ToString());
	}

	[Test]
	public void Case2()
	{
		var o = JObject.Parse(@"{'string1':'foo','integer2':99,'datetime3':'2000-05-23T00:00:00'}");
		foreach (var x in o)
		{
			var name = x.Key;
			var value = x.Value;
			TestContext.WriteLine($"{name} : {value}");
		}
	}

	[Test]
	public void LinqCast()
	{
		var o = JObject.Parse(@"{'string1':'foo','integer2':99,'datetime3':'2000-05-23T00:00:00'}");
		var oLinq = o.Cast<KeyValuePair<string, JToken>>();

		var x = oLinq
			.Where(d => d.Key == "string1")
			.Select(v => v)
			.FirstOrDefault()
			.Value;

		var y = ((JValue)x).Value;

		TestContext.WriteLine($"{y}");

		oLinq.ToList().ForEach(x => { TestContext.WriteLine($"{x.Key} : {x.Value}"); });
	}

	[Test]
	public void Case3()
	{
		var o = JObject.Parse(@"{'string1':'foo','integer2':99,'datetime3':'2000-05-23T00:00:00'}");
		var foo = o.Properties().Select(p => p.Name + ": " + p.Value);
		TestContext.WriteLine(foo.ToArray()[0]);
	}

	[Test]
	public void SelectToken()
	{
		var o = JObject.Parse(@"{
			  'Stores': [
			    'Lambton Quay',
			    'Willis Street'
			  ],
			  'Manufacturers': [
			    {
			      'Name': 'Acme Co',
			      'Products': [
			        {
			          'Name': 'Anvil',
			          'Price': 50
			        }
			      ]
			    },
			    {
			      'Name': 'Contoso',
			      'Products': [
			        {
			          'Name': 'Elbow Grease',
			          'Price': 99.95
			        },
			        {
			          'Name': 'Headlight Fluid',
			          'Price': 4
			        }
			      ]
			    }
			  ]
			}");
		var name = (string)o.SelectToken("Manufacturers[0].Name");
		// Acme Co

		var productPrice = (decimal)o.SelectToken("Manufacturers[0].Products[0].Price");
		// 50

		var productName = (string)o.SelectToken("Manufacturers[1].Products[0].Name");
		// Elbow Grease
		TestContext.WriteLine($"{name}, {productPrice}, {productName}");
	}
	
		[Test]
	public void SelectTokenWithJsonPath()
	{
		var o = JObject.Parse(@"{
			  'Stores': [
			    'Lambton Quay',
			    'Willis Street'
			  ],
			  'Manufacturers': [
			    {
			      'Name': 'Acme Co',
			      'Products': [
			        {
			          'Name': 'Anvil',
			          'Price': 50
			        }
			      ]
			    },
			    {
			      'Name': 'Contoso',
			      'Products': [
			        {
			          'Name': 'Elbow Grease',
			          'Price': 99.95
			        },
			        {
			          'Name': 'Headlight Fluid',
			          'Price': 4
			        }
			      ]
			    }
			  ]
			}");
		
		var acme = o.SelectToken("$.Manufacturers[?(@.Name == 'Acme Co')]");
		TestContext.WriteLine($"{acme}");

		var pricyProducts = o.SelectTokens("$..Products[?(@.Price >= 50)].Name");
		
		foreach (var item in pricyProducts)
		{
			TestContext.Write(item);
		}
	}
}