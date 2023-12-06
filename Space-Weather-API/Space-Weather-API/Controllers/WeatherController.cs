using Microsoft.AspNetCore.Mvc;
using Space_Weather_API.Models;
using System.Text.Json;

namespace Space_Weather_API.Controllers;

[ApiController]
[Route("api/v1/planets")]
public class WeatherController : ControllerBase
{
	private readonly string planetTemperatures = @"{
		  ""weatherData"": [
			{
			  ""id"": ""1"",
			  ""planet"": ""Mars"",
			  ""condition"": ""Sunny"",
			  ""temperature"": -23.5
			},
			{
			  ""id"": ""2"",
			  ""planet"": ""Europa"",
			  ""condition"": ""Icy"",
			  ""temperature"": -160.8
			},
			{
			  ""id"": ""3"",
			  ""planet"": ""Titan"",
			  ""condition"": ""Windy"",
			  ""temperature"": -178.2
			}
		  ]
		}";

	[HttpGet("{id}")]
	public IActionResult GetPlanetWeather(string id)
	{
		try
		{
			var planetTemperaturesDeserialized = JsonSerializer.Deserialize<WeatherDataContainer>(planetTemperatures);
			var weatherInfo = planetTemperaturesDeserialized?.WeatherData.FirstOrDefault(planet => planet.Id == id);

			if (weatherInfo != null)
			{
				Console.WriteLine($"Found planet. Id: {weatherInfo.Id} Planet: {weatherInfo.Planet} Condition: {weatherInfo.Condition} Temperature: {weatherInfo.Temperature}");
				return Ok(weatherInfo);
			}
			else
			{
				Console.WriteLine($"Planet can not found.");
				return NotFound();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return StatusCode(500, "Internal server error!");
		}
	}
}
