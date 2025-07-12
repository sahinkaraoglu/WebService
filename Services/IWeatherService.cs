using WeatherService.Models;

namespace WeatherService.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse?> GetWeatherByCityAsync(string city, string? countryCode = null, string? stateCode = null, string units = "metric", string language = "tr");
        Task<WeatherResponse?> GetWeatherByCityIdAsync(int cityId, string units = "metric", string language = "tr");
        Task<WeatherResponse?> GetWeatherByZipCodeAsync(string zipCode, string? countryCode = null, string units = "metric", string language = "tr");
        Task<WeatherResponse?> GetWeatherByCoordinatesAsync(double latitude, double longitude, string units = "metric", string language = "tr");
        Task<CitiesResponse> GetAllCitiesAsync(int page = 1, int pageSize = 50, string? countryCode = null, string? searchTerm = null);
    }
} 