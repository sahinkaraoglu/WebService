using System.Text.Json;
using WeatherService.Models;

namespace WeatherService.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["OpenWeatherMap:ApiKey"] ?? "34302733b252924bf7cdf7ddc7f67ac0";
        }

        public async Task<WeatherResponse?> GetWeatherByCityAsync(string city, string? countryCode = null, string? stateCode = null, string units = "metric", string language = "tr")
        {
            var query = $"q={Uri.EscapeDataString(city)}";
            
            if (!string.IsNullOrEmpty(stateCode) && !string.IsNullOrEmpty(countryCode))
            {
                query += $",{stateCode},{countryCode}";
            }
            else if (!string.IsNullOrEmpty(countryCode))
            {
                query += $",{countryCode}";
            }

            return await MakeApiCallAsync(query, units, language);
        }

        public async Task<WeatherResponse?> GetWeatherByCityIdAsync(int cityId, string units = "metric", string language = "tr")
        {
            var query = $"id={cityId}";
            return await MakeApiCallAsync(query, units, language);
        }

        public async Task<WeatherResponse?> GetWeatherByZipCodeAsync(string zipCode, string? countryCode = null, string units = "metric", string language = "tr")
        {
            var query = $"zip={zipCode}";
            if (!string.IsNullOrEmpty(countryCode))
            {
                query += $",{countryCode}";
            }

            return await MakeApiCallAsync(query, units, language);
        }

        public async Task<WeatherResponse?> GetWeatherByCoordinatesAsync(double latitude, double longitude, string units = "metric", string language = "tr")
        {
            var query = $"lat={latitude}&lon={longitude}";
            return await MakeApiCallAsync(query, units, language);
        }

        private async Task<WeatherResponse?> MakeApiCallAsync(string query, string units, string language)
        {
            try
            {
                var url = $"{_baseUrl}?{query}&appid={_apiKey}&units={units}&lang={language}";
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    return weatherResponse;
                }
                else
                {
                    // Log error or handle specific error codes
                    Console.WriteLine($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return null;
            }
        }
    }
} 