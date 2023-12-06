using System.Text.Json.Serialization;

namespace Space_Weather_API.Models
{
	public class WeatherDataContainer
	{
		[JsonPropertyName("weatherData")]
		public required List<WeatherInfo> WeatherData { get; set; }
	}
}
