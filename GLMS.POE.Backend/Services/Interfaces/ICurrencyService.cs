namespace GLMS.POE.Backend.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<decimal> GetUsdToZarRateAsync();

        decimal ConvertUsdToZar(decimal usdAmount, decimal rate);
    }
}
