using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Space_Weather_API.Models;
using System.Text.Json;

namespace Space_Weather_API.Controllers;

[ApiController]
[Route("api/v1/weathers/planets")]
public class WeatherController : ControllerBase
{
	private string PlanetTemperatures { get; set; } = @"{
		  ""weatherData"": [
			{
			  ""id"": 1,
			  ""planet"": ""Mars"",
			  ""condition"": ""Sunny"",
			  ""temperature"": -23.5
			},
			{
			  ""id"": 2,
			  ""planet"": ""Europa"",
			  ""condition"": ""Icy"",
			  ""temperature"": -160.8
			},
			{
			  ""id"": 3,
			  ""planet"": ""Titan"",
			  ""condition"": ""Windy"",
			  ""temperature"": -178.2
			}
		  ]
		}";


	[HttpGet("")]
	public IActionResult GetAllPlanetsWeather()
	{
		try
		{
			var planetTemperaturesDeserialized = JsonSerializer.Deserialize<WeatherDataContainer>(PlanetTemperatures);

			if (planetTemperaturesDeserialized != null && planetTemperaturesDeserialized.WeatherData.Count > 0)
			{
				return Ok(planetTemperaturesDeserialized);
			}
			else 
			{
				return NotFound();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return StatusCode(500, "Internal server error!");
		}
	}

	[HttpGet("{id}")]
	public IActionResult GetPlanetWeatherById(int id)
	{
		try
		{
			var planetTemperaturesDeserialized = JsonSerializer.Deserialize<WeatherDataContainer>(PlanetTemperatures);
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

	[HttpPost("")]
	public IActionResult AddNewPlanetWeather([FromBody] WeatherInfo newWeatherInfo)
	{
		try
		{
			var planetTemperaturesDeserialized = JsonSerializer.Deserialize<WeatherDataContainer>(PlanetTemperatures);
			var orderedList = planetTemperaturesDeserialized?.WeatherData.OrderBy(weatherInfo => weatherInfo.Id).ToList();
			var id = orderedList?.LastOrDefault()?.Id + 1 ?? 1;

			var newPlanetWeather = new WeatherInfo
			{
				Id = id,
				Planet = newWeatherInfo.Planet,
				Condition = newWeatherInfo.Condition,
				Temperature = newWeatherInfo.Temperature
			};

			planetTemperaturesDeserialized?.WeatherData.Add(newPlanetWeather);

			var newPlanetTemperatures = JsonSerializer.Serialize(planetTemperaturesDeserialized, new JsonSerializerOptions { WriteIndented = true });
			PlanetTemperatures = newPlanetTemperatures;
			Console.WriteLine(PlanetTemperatures);

			return CreatedAtAction(nameof(GetPlanetWeatherById), new { id = newPlanetWeather.Id }, newPlanetWeather);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return StatusCode(500, "Internal server error!");
		}
	}

	[HttpPut("{id}")]
	public IActionResult UpdatePlanetWeather(int id, [FromBody] WeatherInfo newWeatherInfo)
	{
		try
		{
			var planetTemperaturesDeserialized = JsonSerializer.Deserialize<WeatherDataContainer>(PlanetTemperatures);
			var weatherInfo = planetTemperaturesDeserialized?.WeatherData.FirstOrDefault(planet => planet.Id == id);

			if (weatherInfo != null)
			{
				weatherInfo.Planet = newWeatherInfo.Planet;
				weatherInfo.Condition = newWeatherInfo.Condition;
				weatherInfo.Temperature = newWeatherInfo.Temperature;

				var newPlanetTemperatures = JsonSerializer.Serialize(planetTemperaturesDeserialized, new JsonSerializerOptions { WriteIndented = true });
				Console.WriteLine(newPlanetTemperatures);

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

	[HttpPatch("{id}")]
	public IActionResult UpdatePlanetWeatherByPatch(int id, [FromBody] JsonElement newWeatherPatchInfo)
	{
		try
		{
			var planetTemperaturesDeserialized = JsonSerializer.Deserialize<WeatherDataContainer>(PlanetTemperatures);
			var weatherInfo = planetTemperaturesDeserialized?.WeatherData.FirstOrDefault(planet => planet.Id == id);

			if (weatherInfo != null)
			{
				var patchInfoProperties = newWeatherPatchInfo.EnumerateObject();
				foreach (var property in patchInfoProperties)
				{
					if (property.Name == "planet")
					{
						if (string.IsNullOrEmpty(property.Value.GetString()))
							return BadRequest("Invalid format: Planet cannot be empty.");
						
						weatherInfo.Planet = property.Value.GetString()!;
					}
					else if (property.Name == "condition")
					{
						if (string.IsNullOrEmpty(property.Value.GetString()))
							return BadRequest("Invalid format: Condition cannot be empty.");

						weatherInfo.Condition = property.Value.GetString()!;
					}
					else if (property.Name == "temperature")
					{
						if (property.Value.ValueKind != JsonValueKind.Number)
							return BadRequest("Invalid format: Temperature cannot be empty.");

						weatherInfo.Temperature = property.Value.GetDouble();
					}
				}

				var newPlanetTemperatures = JsonSerializer.Serialize(planetTemperaturesDeserialized, new JsonSerializerOptions { WriteIndented = true });
				Console.WriteLine(newPlanetTemperatures);

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
