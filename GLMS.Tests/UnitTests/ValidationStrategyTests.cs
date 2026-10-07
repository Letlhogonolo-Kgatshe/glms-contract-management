using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;
using GLMS.POE.Backend.Services.Implementations;
using GLMS.POE.Backend.Services.Interfaces;
using Xunit;

namespace GLMS.Tests.UnitTests
{
     public class ValidationStrategyTests
    {
        // ── ActiveContractValidationStrategy ──────────────────────────────────

        [Fact]
        public void ActiveStrategy_ActiveContract_ReturnsTrue()
        {
            var strategy = new ActiveContractValidationStrategy();
            var contract = new Contract { Status = ContractStatus.Active };
            Assert.True(strategy.IsValid(contract));
        }

        [Theory]
        [InlineData(ContractStatus.Draft)]
        [InlineData(ContractStatus.Expired)]
        [InlineData(ContractStatus.OnHold)]
        public void ActiveStrategy_NonActiveStatus_ReturnsFalse(ContractStatus status)
        {
            var strategy = new ActiveContractValidationStrategy();
            var contract = new Contract { Status = status };
            Assert.False(strategy.IsValid(contract));
        }

        [Fact]
        public void ActiveStrategy_ErrorMessage_IsNotEmpty()
        {
            var strategy = new ActiveContractValidationStrategy();
            Assert.False(string.IsNullOrWhiteSpace(strategy.ErrorMessage));
        }

        // ── ContractDateValidationStrategy ────────────────────────────────────

        [Fact]
        public void DateStrategy_FutureEndDate_ReturnsTrue()
        {
            var strategy = new ContractDateValidationStrategy();
            var contract = new Contract
            {
                Status = ContractStatus.Active,
                EndDate = DateTime.Today.AddYears(1)
            };
            Assert.True(strategy.IsValid(contract));
        }

        [Fact]
        public void DateStrategy_PastEndDate_ReturnsFalse()
        {
            var strategy = new ContractDateValidationStrategy();
            var contract = new Contract
            {
                Status = ContractStatus.Active,
                EndDate = DateTime.Today.AddDays(-1)
            };
            Assert.False(strategy.IsValid(contract));
        }

        [Fact]
        public void DateStrategy_TodayEndDate_ReturnsTrue()
        {
            var strategy = new ContractDateValidationStrategy();
            var contract = new Contract
            {
                Status = ContractStatus.Active,
                EndDate = DateTime.Today
            };
            Assert.True(strategy.IsValid(contract));
        }

        // ── CompositeValidationStrategy ───────────────────────────────────────

        [Fact]
        public void CompositeStrategy_AllStrategiesPass_ReturnsTrue()
        {
            var composite = new CompositeValidationStrategy(new[]
            {
                (IValidationStrategy)new ActiveContractValidationStrategy(),
                new ContractDateValidationStrategy()
            });

            var contract = new Contract
            {
                Status = ContractStatus.Active,
                EndDate = DateTime.Today.AddYears(1)
            };

            Assert.True(composite.IsValid(contract));
        }

        [Fact]
        public void CompositeStrategy_FirstStrategyFails_ReturnsFalseWithCorrectMessage()
        {
            var composite = new CompositeValidationStrategy(new[]
            {
                (IValidationStrategy)new ActiveContractValidationStrategy(),
                new ContractDateValidationStrategy()
            });

            var contract = new Contract
            {
                Status = ContractStatus.Expired,
                EndDate = DateTime.Today.AddYears(1)
            };

            Assert.False(composite.IsValid(contract));
            Assert.False(string.IsNullOrWhiteSpace(composite.ErrorMessage));
        }

        [Fact]
        public void CompositeStrategy_SecondStrategyFails_ReturnsFalse()
        {
            var composite = new CompositeValidationStrategy(new[]
            {
                (IValidationStrategy)new ActiveContractValidationStrategy(),
                new ContractDateValidationStrategy()
            });

            var contract = new Contract
            {
                Status = ContractStatus.Active,
                EndDate = DateTime.Today.AddDays(-30) // date has passed
            };

            Assert.False(composite.IsValid(contract));
        }
    }
}
