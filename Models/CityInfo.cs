using System.Text.Json.Serialization;

namespace WeatherService.Models
{
    public class CityInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("coord")]
        public Coordinates Coord { get; set; }

        [JsonPropertyName("population")]
        public int? Population { get; set; }

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }
    }

    public class CitiesResponse
    {
        [JsonPropertyName("cities")]
        public List<CityInfo> Cities { get; set; } = new List<CityInfo>();

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
    }
} 