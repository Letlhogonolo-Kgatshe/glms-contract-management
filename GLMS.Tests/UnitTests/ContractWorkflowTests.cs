using GLMS.POE.Backend.Data;
using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;
using GLMS.POE.Backend.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace GLMS.Tests.UnitTests
{
     public class ContractWorkflowTests : IDisposable
    {
        private readonly ApplicationDbContext _db;
        private readonly ServiceRequestService _sut;

        public ContractWorkflowTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // fresh DB per test
                .Options;

            _db  = new ApplicationDbContext(options);
            _sut = new ServiceRequestService(_db);
        }

        public void Dispose() => _db.Dispose();

        // ── Happy Path ────────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ActiveContract_SucceedsAndPersists()
        {
            // Arrange
            var client = new Client { Name = "Test Co", ContactDetails = "test@test.com", Region = "Africa" };
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            var contract = new Contract
            {
                ClientId = client.Id,
                StartDate = DateTime.Today.AddDays(-10),
                EndDate = DateTime.Today.AddYears(1),
                Status = ContractStatus.Active,
                ServiceLevel = "Premium"
            };
            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();

            var request = new ServiceRequest
            {
                ContractId = contract.Id,
                Description = "Freight delivery — Cape Town to Durban",
                CostUsd = 500m,
                CostZar = 9250m,
                ExchangeRateUsed = 18.50m,
                Status = ServiceRequestStatus.Pending
            };

            // Act
            var result = await _sut.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(1, await _db.ServiceRequests.CountAsync());
        }

        // ── Blocked Statuses ──────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ExpiredContract_ThrowsInvalidOperationException()
        {
            var client = new Client { Name = "Blocked Co", ContactDetails = "x@x.com", Region = "Europe" };
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            var contract = new Contract
            {
                ClientId = client.Id,
                StartDate = DateTime.Today.AddYears(-2),
                EndDate = DateTime.Today.AddYears(-1),
                Status = ContractStatus.Expired,
                ServiceLevel = "Standard"
            };
            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();

            var request = new ServiceRequest { ContractId = contract.Id, Description = "Should fail", CostUsd = 100m };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(request));
        }

        [Fact]
        public async Task CreateAsync_DraftContract_ThrowsInvalidOperationException()
        {
            var client = new Client { Name = "Draft Co", ContactDetails = "d@d.com", Region = "Asia-Pacific" };
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            var contract = new Contract
            {
                ClientId = client.Id,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                Status = ContractStatus.Draft,
                ServiceLevel = "Basic"
            };
            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();

            var request = new ServiceRequest { ContractId = contract.Id, Description = "Should fail", CostUsd = 100m };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(request));
        }

        [Fact]
        public async Task CreateAsync_OnHoldContract_ThrowsInvalidOperationException()
        {
            var client = new Client { Name = "Hold Co", ContactDetails = "h@h.com", Region = "Europe" };
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            var contract = new Contract
            {
                ClientId = client.Id,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                Status = ContractStatus.OnHold,
                ServiceLevel = "Enterprise"
            };
            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();

            var request = new ServiceRequest { ContractId = contract.Id, Description = "Should fail", CostUsd = 100m };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(request));
        }

        [Fact]
        public async Task CreateAsync_NonExistentContract_ThrowsKeyNotFoundException()
        {
            var request = new ServiceRequest { ContractId = 99999, Description = "Ghost contract", CostUsd = 100m };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CreateAsync(request));
        }

        [Fact]
        public async Task CreateAsync_MultipleRequestsOnSameActiveContract_AllSucceed()
        {
            var client = new Client { Name = "Multi Co", ContactDetails = "m@m.com", Region = "Africa" };
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            var contract = new Contract
            {
                ClientId = client.Id,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(2),
                Status = ContractStatus.Active,
                ServiceLevel = "Enterprise"
            };
            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();

            for (int i = 1; i <= 3; i++)
            {
                await _sut.CreateAsync(new ServiceRequest
                {
                    ContractId = contract.Id,
                    Description = $"Request {i}",
                    CostUsd = i * 100m
                });
            }

            Assert.Equal(3, await _db.ServiceRequests.CountAsync());
        }
    }
}
