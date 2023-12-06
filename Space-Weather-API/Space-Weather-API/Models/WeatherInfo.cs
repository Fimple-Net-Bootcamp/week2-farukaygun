using System.Text.Json.Serialization;

namespace Space_Weather_API.Models
{
	public class WeatherInfo
	{
		[JsonPropertyName("id")]
		public required string Id { get; set; }
		[JsonPropertyName("planet")]
		public required string Planet { get; set; }
		[JsonPropertyName("condition")]
		public required string Condition { get; set; }
		[JsonPropertyName("temperature")]
		public double Temperature { get; set; } = 0.0;
	}
}
