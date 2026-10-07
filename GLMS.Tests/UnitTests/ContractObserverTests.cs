using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;
using GLMS.POE.Backend.Services.Implementations;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace GLMS.Tests.UnitTests
{
  
    public class ContractObserverTests
    {
        private readonly ContractObserver _sut;

        public ContractObserverTests()
        {
            _sut = new ContractObserver(NullLogger<ContractObserver>.Instance);
        }

        [Fact]
        public async Task OnStatusChangedAsync_RecordsEventInAuditLog()
        {
            var contract = new Contract
            {
                Id = 42,
                ClientId = 1,
                Client = new Client { Id = 1, Name = "Test Shipping Co", ContactDetails = "x", Region = "Africa" },
                ServiceLevel = "Premium",
                Status = ContractStatus.Active
            };

            int countBefore = ContractObserver.AuditLog.Count;

            await _sut.OnStatusChangedAsync(contract, ContractStatus.Draft, ContractStatus.Active);

            Assert.Equal(countBefore + 1, ContractObserver.AuditLog.Count);
        }

        [Fact]
        public async Task OnStatusChangedAsync_AuditEntryContainsContractId()
        {
            var contract = new Contract
            {
                Id = 99,
                ClientId = 1,
                Client = new Client { Id = 1, Name = "Observer Client", ContactDetails = "x", Region = "Europe" },
                ServiceLevel = "Standard",
                Status = ContractStatus.Active
            };

            await _sut.OnStatusChangedAsync(contract, ContractStatus.Draft, ContractStatus.Active);

            var lastEntry = ContractObserver.AuditLog.Last();
            Assert.Contains("99", lastEntry);
        }

        [Fact]
        public async Task OnStatusChangedAsync_AuditEntryContainsBothStatuses()
        {
            var contract = new Contract
            {
                Id = 55,
                ClientId = 1,
                Client = new Client { Id = 1, Name = "Status Client", ContactDetails = "x", Region = "Africa" },
                ServiceLevel = "Basic",
                Status = ContractStatus.Expired
            };

            await _sut.OnStatusChangedAsync(contract, ContractStatus.Active, ContractStatus.Expired);

            var lastEntry = ContractObserver.AuditLog.Last();
            Assert.Contains("Active", lastEntry);
            Assert.Contains("Expired", lastEntry);
        }

        [Fact]
        public async Task OnStatusChangedAsync_CompletesWithoutException()
        {
            var contract = new Contract { Id = 1, ClientId = 1, ServiceLevel = "Basic", Status = ContractStatus.Draft };
            // Should not throw
            var ex = await Record.ExceptionAsync(() =>
                _sut.OnStatusChangedAsync(contract, ContractStatus.Draft, ContractStatus.OnHold));

            Assert.Null(ex);
        }
    }
}
