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

        public async Task<CitiesResponse> GetAllCitiesAsync(int page = 1, int pageSize = 50, string? countryCode = null, string? searchTerm = null)
        {
            try
            {
                // Örnek şehir verileri - gerçek uygulamada bu veriler bir veritabanından veya başka bir API'den gelir
                var cities = GetSampleCities();
                
                // Filtreleme
                if (!string.IsNullOrEmpty(countryCode))
                {
                    cities = cities.Where(c => c.Country.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    cities = cities.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                              c.Country.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                
                // Sayfalama
                var totalCount = cities.Count;
                var pagedCities = cities
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                
                return new CitiesResponse
                {
                    Cities = pagedCities,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while getting cities: {ex.Message}");
                return new CitiesResponse
                {
                    Cities = new List<CityInfo>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };
            }
        }

        private List<CityInfo> GetSampleCities()
        {
            return new List<CityInfo>
            {
                new CityInfo { Id = 323786, Name = "Ankara", Country = "TR", Coord = new Coordinates { Lat = 39.9199, Lon = 32.8543 }, Timezone = 10800 },
                new CityInfo { Id = 745044, Name = "İstanbul", Country = "TR", Coord = new Coordinates { Lat = 41.0082, Lon = 28.9784 }, Timezone = 10800 },
                new CityInfo { Id = 311044, Name = "İzmir", Country = "TR", Coord = new Coordinates { Lat = 38.4192, Lon = 27.1287 }, Timezone = 10800 },
                new CityInfo { Id = 310859, Name = "Bursa", Country = "TR", Coord = new Coordinates { Lat = 40.1885, Lon = 29.0610 }, Timezone = 10800 },
                new CityInfo { Id = 315468, Name = "Antalya", Country = "TR", Coord = new Coordinates { Lat = 36.8969, Lon = 30.7133 }, Timezone = 10800 },
                new CityInfo { Id = 298333, Name = "Adana", Country = "TR", Coord = new Coordinates { Lat = 37.0000, Lon = 35.3213 }, Timezone = 10800 },
                new CityInfo { Id = 306571, Name = "Konya", Country = "TR", Coord = new Coordinates { Lat = 37.8667, Lon = 32.4833 }, Timezone = 10800 },
                new CityInfo { Id = 314830, Name = "Gaziantep", Country = "TR", Coord = new Coordinates { Lat = 37.0662, Lon = 37.3833 }, Timezone = 10800 },
                new CityInfo { Id = 315202, Name = "Mersin", Country = "TR", Coord = new Coordinates { Lat = 36.8000, Lon = 34.6333 }, Timezone = 10800 },
                new CityInfo { Id = 315468, Name = "Diyarbakır", Country = "TR", Coord = new Coordinates { Lat = 37.9144, Lon = 40.2306 }, Timezone = 10800 },
                new CityInfo { Id = 2643743, Name = "London", Country = "GB", Coord = new Coordinates { Lat = 51.5074, Lon = -0.1278 }, Timezone = 0 },
                new CityInfo { Id = 5128581, Name = "New York", Country = "US", State = "NY", Coord = new Coordinates { Lat = 40.7128, Lon = -74.0060 }, Timezone = -18000 },
                new CityInfo { Id = 1850147, Name = "Tokyo", Country = "JP", Coord = new Coordinates { Lat = 35.6762, Lon = 139.6503 }, Timezone = 32400 },
                new CityInfo { Id = 2950159, Name = "Berlin", Country = "DE", Coord = new Coordinates { Lat = 52.5200, Lon = 13.4050 }, Timezone = 3600 },
                new CityInfo { Id = 2988507, Name = "Paris", Country = "FR", Coord = new Coordinates { Lat = 48.8566, Lon = 2.3522 }, Timezone = 3600 },
                new CityInfo { Id = 3435910, Name = "Buenos Aires", Country = "AR", Coord = new Coordinates { Lat = -34.6118, Lon = -58.3960 }, Timezone = -10800 },
                new CityInfo { Id = 1796236, Name = "Shanghai", Country = "CN", Coord = new Coordinates { Lat = 31.2304, Lon = 121.4737 }, Timezone = 28800 },
                new CityInfo { Id = 1275339, Name = "Mumbai", Country = "IN", Coord = new Coordinates { Lat = 19.0760, Lon = 72.8777 }, Timezone = 19800 },
                new CityInfo { Id = 524901, Name = "Moscow", Country = "RU", Coord = new Coordinates { Lat = 55.7558, Lon = 37.6176 }, Timezone = 10800 },
                new CityInfo { Id = 993800, Name = "Johannesburg", Country = "ZA", Coord = new Coordinates { Lat = -26.2041, Lon = 28.0473 }, Timezone = 7200 }
            };
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