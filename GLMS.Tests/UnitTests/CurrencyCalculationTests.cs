using GLMS.POE.Backend.Services.Implementations;
using Xunit;

namespace GLMS.Tests.UnitTests
{
      public class CurrencyCalculationTests
    {
        
        private readonly CurrencyService _sut;

        public CurrencyCalculationTests()
        {
            _sut = new CurrencyService(
                httpClient: null!,
                logger: Microsoft.Extensions.Logging.Abstractions.NullLogger<CurrencyService>.Instance,
                config: null!
            );
        }

        // ── Happy path ────────────────────────────────────────────────────────

        [Fact]
        public void ConvertUsdToZar_GivenKnownRate_ReturnsCorrectAmount()
        {
            // Arrange
            decimal usd = 100m;
            decimal rate = 18.50m;

            // Act
            decimal result = _sut.ConvertUsdToZar(usd, rate);

            // Assert
            Assert.Equal(1850.00m, result);
        }

        [Fact]
        public void ConvertUsdToZar_SmallAmount_RoundsToTwoDecimals()
        {
            decimal usd = 1m;
            decimal rate = 18.1234m;

            decimal result = _sut.ConvertUsdToZar(usd, rate);

            // Should be rounded to 2 d.p.
            Assert.Equal(18.12m, result);
        }

        [Fact]
        public void ConvertUsdToZar_LargeAmount_CalculatesCorrectly()
        {
            decimal usd = 50_000m;
            decimal rate = 19.00m;

            decimal result = _sut.ConvertUsdToZar(usd, rate);

            Assert.Equal(950_000.00m, result);
        }

        [Fact]
        public void ConvertUsdToZar_ZeroUsd_ReturnsZero()
        {
            decimal result = _sut.ConvertUsdToZar(0m, 18.50m);
            Assert.Equal(0m, result);
        }

        [Fact]
        public void ConvertUsdToZar_FractionalUsd_CalculatesCorrectly()
        {
            // $0.50 at rate 18.50 = 9.25
            decimal result = _sut.ConvertUsdToZar(0.50m, 18.50m);
            Assert.Equal(9.25m, result);
        }

        // ── Edge / Failure cases ──────────────────────────────────────────────

        [Fact]
        public void ConvertUsdToZar_ZeroRate_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.ConvertUsdToZar(100m, 0m));
        }

        [Fact]
        public void ConvertUsdToZar_NegativeRate_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.ConvertUsdToZar(100m, -1m));
        }

        [Theory]
        [InlineData(10,   18.50,  185.00)]
        [InlineData(250,  18.00, 4500.00)]
        [InlineData(1000, 17.75, 17750.00)]
        public void ConvertUsdToZar_MultipleKnownValues_AreCorrect(decimal usd, decimal rate, decimal expected)
        {
            decimal result = _sut.ConvertUsdToZar(usd, rate);
            Assert.Equal(expected, result);
        }
    }
}
