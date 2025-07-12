namespace WeatherService.Models
{
    public class WeatherRequest
    {
        public string? City { get; set; }
        public string? CountryCode { get; set; }
        public string? StateCode { get; set; }
        public int? CityId { get; set; }
        public string? ZipCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Units { get; set; } = "metric"; // metric, imperial, standard
        public string Language { get; set; } = "tr"; // tr, en, fr, etc.
        public string Format { get; set; } = "json"; // json, xml, html
    }
} 