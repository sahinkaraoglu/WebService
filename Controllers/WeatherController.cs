using Microsoft.AspNetCore.Mvc;
using WeatherService.Models;
using WeatherService.Services;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        /// <summary>
        /// Şehir adına göre hava durumu bilgisi alır
        /// </summary>
        /// <param name="city">Şehir adı</param>
        /// <param name="countryCode">Ülke kodu (opsiyonel)</param>
        /// <param name="stateCode">Eyalet kodu (opsiyonel)</param>
        /// <param name="units">Birim sistemi (metric, imperial, standard)</param>
        /// <param name="language">Dil kodu (tr, en, fr, vb.)</param>
        /// <returns>Hava durumu bilgisi</returns>
        [HttpGet("city/{city}")]
        public async Task<ActionResult<WeatherResponse>> GetWeatherByCity(
            string city,
            [FromQuery] string? countryCode = null,
            [FromQuery] string? stateCode = null,
            [FromQuery] string units = "metric",
            [FromQuery] string language = "tr")
        {
            try
            {
                var weather = await _weatherService.GetWeatherByCityAsync(city, countryCode, stateCode, units, language);
                
                if (weather == null)
                {
                    return NotFound($"'{city}' şehri için hava durumu bilgisi bulunamadı.");
                }

                return Ok(weather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şehir ID'sine göre hava durumu bilgisi alır
        /// </summary>
        /// <param name="cityId">Şehir ID'si</param>
        /// <param name="units">Birim sistemi</param>
        /// <param name="language">Dil kodu</param>
        /// <returns>Hava durumu bilgisi</returns>
        [HttpGet("cityid/{cityId}")]
        public async Task<ActionResult<WeatherResponse>> GetWeatherByCityId(
            int cityId,
            [FromQuery] string units = "metric",
            [FromQuery] string language = "tr")
        {
            try
            {
                var weather = await _weatherService.GetWeatherByCityIdAsync(cityId, units, language);
                
                if (weather == null)
                {
                    return NotFound($"ID: {cityId} olan şehir için hava durumu bilgisi bulunamadı.");
                }

                return Ok(weather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Posta koduna göre hava durumu bilgisi alır
        /// </summary>
        /// <param name="zipCode">Posta kodu</param>
        /// <param name="countryCode">Ülke kodu (opsiyonel)</param>
        /// <param name="units">Birim sistemi</param>
        /// <param name="language">Dil kodu</param>
        /// <returns>Hava durumu bilgisi</returns>
        [HttpGet("zip/{zipCode}")]
        public async Task<ActionResult<WeatherResponse>> GetWeatherByZipCode(
            string zipCode,
            [FromQuery] string? countryCode = null,
            [FromQuery] string units = "metric",
            [FromQuery] string language = "tr")
        {
            try
            {
                var weather = await _weatherService.GetWeatherByZipCodeAsync(zipCode, countryCode, units, language);
                
                if (weather == null)
                {
                    return NotFound($"Posta kodu: {zipCode} için hava durumu bilgisi bulunamadı.");
                }

                return Ok(weather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Koordinatlara göre hava durumu bilgisi alır
        /// </summary>
        /// <param name="latitude">Enlem</param>
        /// <param name="longitude">Boylam</param>
        /// <param name="units">Birim sistemi</param>
        /// <param name="language">Dil kodu</param>
        /// <returns>Hava durumu bilgisi</returns>
        [HttpGet("coordinates")]
        public async Task<ActionResult<WeatherResponse>> GetWeatherByCoordinates(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] string units = "metric",
            [FromQuery] string language = "tr")
        {
            try
            {
                var weather = await _weatherService.GetWeatherByCoordinatesAsync(latitude, longitude, units, language);
                
                if (weather == null)
                {
                    return NotFound($"Koordinatlar ({latitude}, {longitude}) için hava durumu bilgisi bulunamadı.");
                }

                return Ok(weather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Tüm şehirlerin bilgilerini getirir
        /// </summary>
        /// <param name="page">Sayfa numarası (varsayılan: 1)</param>
        /// <param name="pageSize">Sayfa başına şehir sayısı (varsayılan: 50)</param>
        /// <param name="countryCode">Ülke kodu ile filtreleme (opsiyonel)</param>
        /// <param name="searchTerm">Arama terimi ile filtreleme (opsiyonel)</param>
        /// <returns>Şehir listesi</returns>
        [HttpGet("cities")]
        public async Task<ActionResult<CitiesResponse>> GetAllCities(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? countryCode = null,
            [FromQuery] string? searchTerm = null)
        {
            try
            {
                if (page < 1)
                {
                    return BadRequest("Sayfa numarası 1'den küçük olamaz.");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Sayfa boyutu 1 ile 100 arasında olmalıdır.");
                }

                var cities = await _weatherService.GetAllCitiesAsync(page, pageSize, countryCode, searchTerm);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Genel hava durumu sorgusu (POST metodu ile)
        /// </summary>
        /// <param name="request">Hava durumu sorgu parametreleri</param>
        /// <returns>Hava durumu bilgisi</returns>
        [HttpPost("query")]
        public async Task<ActionResult<WeatherResponse>> GetWeatherByQuery([FromBody] WeatherRequest request)
        {
            try
            {
                WeatherResponse? weather = null;

                if (!string.IsNullOrEmpty(request.City))
                {
                    weather = await _weatherService.GetWeatherByCityAsync(
                        request.City, 
                        request.CountryCode, 
                        request.StateCode, 
                        request.Units, 
                        request.Language);
                }
                else if (request.CityId.HasValue)
                {
                    weather = await _weatherService.GetWeatherByCityIdAsync(
                        request.CityId.Value, 
                        request.Units, 
                        request.Language);
                }
                else if (!string.IsNullOrEmpty(request.ZipCode))
                {
                    weather = await _weatherService.GetWeatherByZipCodeAsync(
                        request.ZipCode, 
                        request.CountryCode, 
                        request.Units, 
                        request.Language);
                }
                else if (request.Latitude.HasValue && request.Longitude.HasValue)
                {
                    weather = await _weatherService.GetWeatherByCoordinatesAsync(
                        request.Latitude.Value, 
                        request.Longitude.Value, 
                        request.Units, 
                        request.Language);
                }
                else
                {
                    return BadRequest("En az bir sorgu parametresi belirtilmelidir (city, cityId, zipCode veya coordinates).");
                }

                if (weather == null)
                {
                    return NotFound("Belirtilen kriterlere uygun hava durumu bilgisi bulunamadı.");
                }

                return Ok(weather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }
    }
} 