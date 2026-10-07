using System.Text.Json;
using GLMS.POE.Backend.Services.Interfaces;

namespace GLMS.POE.Backend.Services.Implementations
{
    
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CurrencyService> _logger;
        private readonly IConfiguration _config;
        private decimal _cachedRate = 0;
        private DateTime _cacheExpiry = DateTime.MinValue;

        public CurrencyService(HttpClient httpClient, ILogger<CurrencyService> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            if (_cachedRate > 0 && DateTime.UtcNow < _cacheExpiry)
                return _cachedRate;

            try
            {
                var response = await _httpClient.GetAsync("https://open.er-api.com/v6/latest/USD");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var rate = doc.RootElement
                    .GetProperty("rates")
                    .GetProperty("ZAR")
                    .GetDecimal();

                _cachedRate = rate;
                _cacheExpiry = DateTime.UtcNow.AddMinutes(10);

                _logger.LogInformation("Fetched USD→ZAR rate: {Rate}", rate);
                return rate;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch exchange rate. Using fallback.");
                return 18.50m;
            }
        }

        public decimal ConvertUsdToZar(decimal usdAmount, decimal rate)
        {
            if (rate <= 0) throw new ArgumentException("Rate must be positive.", nameof(rate));
            return Math.Round(usdAmount * rate, 2);
        }
    }
}
