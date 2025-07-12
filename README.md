# Weather Service API

A RESTful weather service built with ASP.NET Core 9.0 that provides weather information using the OpenWeatherMap API.

## Features

- **Multiple Query Methods**: Get weather by city name, city ID, zip code, or coordinates
- **Multi-language Support**: Weather descriptions in multiple languages
- **Unit Systems**: Support for metric, imperial, and standard units
- **City Management**: List and search cities with pagination
- **Swagger Documentation**: Interactive API documentation

## API Endpoints

### Weather Information
- `GET /api/weather/city/{city}` - Get weather by city name
- `GET /api/weather/cityid/{cityId}` - Get weather by city ID
- `GET /api/weather/zip/{zipCode}` - Get weather by zip code
- `GET /api/weather/coordinates` - Get weather by coordinates
- `POST /api/weather/query` - General weather query

### City Management
- `GET /api/weather/cities` - List all cities with pagination and filtering

## Quick Start

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd WebService
   ```

2. **Configure API Key**
   Add your OpenWeatherMap API key to `appsettings.json`:
   ```json
   {
     "OpenWeatherMap": {
       "ApiKey": "your-api-key-here"
     }
   }
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access Swagger UI**
   Navigate to `https://localhost:7001/swagger` for interactive API documentation

## Example Usage

```bash
# Get weather for Istanbul
curl "https://localhost:7001/api/weather/city/Istanbul?units=metric&language=en"

# Get weather by coordinates
curl "https://localhost:7001/api/weather/coordinates?latitude=41.0082&longitude=28.9784&units=metric"

# List cities with pagination
curl "https://localhost:7001/api/weather/cities?page=1&pageSize=10&countryCode=TR"
```

## Technologies

- **.NET 9.0**
- **ASP.NET Core Web API**
- **OpenWeatherMap API**
- **Swagger/OpenAPI**
- **JSON Serialization**

## License

This project is licensed under the MIT License. 