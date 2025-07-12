# Weather Service - Hava Durumu Web Servisi

Bu proje, OpenWeatherMap API'sini kullanarak hava durumu bilgilerini sağlayan bir .NET Core Web API uygulamasıdır.

## Özellikler

- Şehir adına göre hava durumu sorgulama
- Şehir ID'sine göre hava durumu sorgulama
- Posta koduna göre hava durumu sorgulama
- Koordinatlara göre hava durumu sorgulama
- Çoklu dil desteği (Türkçe, İngilizce, vb.)
- Farklı birim sistemleri (Celsius, Fahrenheit, Kelvin)

## API Endpoint'leri

### 1. Şehir Adına Göre Hava Durumu
```
GET /api/weather/city/{city}?countryCode={countryCode}&stateCode={stateCode}&units={units}&language={language}
```

**Örnek:**
```
GET /api/weather/city/Istanbul?countryCode=tr&units=metric&language=tr
```

### 2. Şehir ID'sine Göre Hava Durumu
```
GET /api/weather/cityid/{cityId}?units={units}&language={language}
```

**Örnek:**
```
GET /api/weather/cityid/745044?units=metric&language=tr
```

### 3. Posta Koduna Göre Hava Durumu
```
GET /api/weather/zip/{zipCode}?countryCode={countryCode}&units={units}&language={language}
```

**Örnek:**
```
GET /api/weather/zip/34000?countryCode=tr&units=metric&language=tr
```

### 4. Koordinatlara Göre Hava Durumu
```
GET /api/weather/coordinates?latitude={lat}&longitude={lon}&units={units}&language={language}
```

**Örnek:**
```
GET /api/weather/coordinates?latitude=41.0082&longitude=28.9784&units=metric&language=tr
```

### 5. Genel Sorgu (POST)
```
POST /api/weather/query
```

**Request Body:**
```json
{
  "city": "Istanbul",
  "countryCode": "tr",
  "units": "metric",
  "language": "tr"
}
```

## Parametreler

### units (Birim Sistemi)
- `metric`: Celsius, metre/saniye, hPa
- `imperial`: Fahrenheit, mil/saat, hPa
- `standard`: Kelvin, metre/saniye, hPa

### language (Dil)
- `tr`: Türkçe
- `en`: İngilizce
- `fr`: Fransızca
- `de`: Almanca
- `es`: İspanyolca
- Ve daha fazlası...

## Kurulum

1. Projeyi klonlayın:
```bash
git clone <repository-url>
cd WeatherService
```

2. Bağımlılıkları yükleyin:
```bash
dotnet restore
```

3. Uygulamayı çalıştırın:
```bash
dotnet run
```

4. Tarayıcınızda `https://localhost:7001` adresine gidin.

## API Anahtarı

Proje, OpenWeatherMap API anahtarını `appsettings.json` dosyasında saklar. Kendi API anahtarınızı almak için [OpenWeatherMap](https://openweathermap.org/api) sitesine kayıt olun.

## Örnek Kullanım

### cURL ile Test

```bash
# İstanbul hava durumu
curl "https://localhost:7001/api/weather/city/Istanbul?countryCode=tr&units=metric&language=tr"

# Koordinatlara göre
curl "https://localhost:7001/api/weather/coordinates?latitude=41.0082&longitude=28.9784&units=metric&language=tr"
```

### JavaScript ile Test

```javascript
// Şehir adına göre
fetch('/api/weather/city/Istanbul?countryCode=tr&units=metric&language=tr')
  .then(response => response.json())
  .then(data => console.log(data));

// POST ile genel sorgu
fetch('/api/weather/query', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    city: 'Istanbul',
    countryCode: 'tr',
    units: 'metric',
    language: 'tr'
  })
})
.then(response => response.json())
.then(data => console.log(data));
```

## Yanıt Formatı

API, OpenWeatherMap'ten gelen JSON formatında yanıt verir:

```json
{
  "coord": {
    "lon": 28.9784,
    "lat": 41.0082
  },
  "weather": [
    {
      "id": 800,
      "main": "Clear",
      "description": "açık",
      "icon": "01d"
    }
  ],
  "base": "stations",
  "main": {
    "temp": 25.6,
    "feels_like": 25.8,
    "temp_min": 23.2,
    "temp_max": 28.1,
    "pressure": 1013,
    "humidity": 65
  },
  "visibility": 10000,
  "wind": {
    "speed": 3.6,
    "deg": 280
  },
  "clouds": {
    "all": 0
  },
  "dt": 1640995200,
  "sys": {
    "type": 2,
    "id": 2000692,
    "country": "TR",
    "sunrise": 1640966400,
    "sunset": 1640995200
  },
  "timezone": 10800,
  "id": 745044,
  "name": "Istanbul",
  "cod": 200
}
```

## Hata Kodları

- `200`: Başarılı
- `400`: Geçersiz istek parametreleri
- `404`: Hava durumu bilgisi bulunamadı
- `500`: Sunucu hatası

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. 