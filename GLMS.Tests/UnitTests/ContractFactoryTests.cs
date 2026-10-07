using GLMS.POE.Shared.Models.Enums;
using GLMS.POE.Backend.Services.Implementations;
using Xunit;

namespace GLMS.Tests.UnitTests
{
    public class ContractFactoryTests
    {
        private readonly ContractFactory _sut = new();

        // ── CreateDraftContract ───────────────────────────────────────────────

        [Fact]
        public void CreateDraftContract_ValidInputs_ReturnsContractWithDraftStatus()
        {
            var contract = _sut.CreateDraftContract(
                clientId: 1,
                startDate: DateTime.Today,
                endDate: DateTime.Today.AddYears(1),
                serviceLevel: "Standard"
            );

            Assert.Equal(ContractStatus.Draft, contract.Status);
        }

        [Fact]
        public void CreateDraftContract_ValidInputs_SetsAllFields()
        {
            var start = DateTime.Today;
            var end   = DateTime.Today.AddMonths(6);

            var contract = _sut.CreateDraftContract(1, start, end, "Premium");

            Assert.Equal(1, contract.ClientId);
            Assert.Equal(start, contract.StartDate);
            Assert.Equal(end, contract.EndDate);
            Assert.Equal("Premium", contract.ServiceLevel);
        }

        [Fact]
        public void CreateDraftContract_EndBeforeStart_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _sut.CreateDraftContract(1, DateTime.Today, DateTime.Today.AddDays(-1), "Basic")
            );
        }

        [Fact]
        public void CreateDraftContract_EndEqualsStart_ThrowsArgumentException()
        {
            var date = DateTime.Today;
            Assert.Throws<ArgumentException>(() =>
                _sut.CreateDraftContract(1, date, date, "Basic")
            );
        }

        // ── CreateActiveContract ──────────────────────────────────────────────

        [Fact]
        public void CreateActiveContract_ValidInputs_ReturnsContractWithActiveStatus()
        {
            var contract = _sut.CreateActiveContract(
                clientId: 2,
                startDate: DateTime.Today,
                endDate: DateTime.Today.AddYears(2),
                serviceLevel: "Enterprise"
            );

            Assert.Equal(ContractStatus.Active, contract.Status);
        }

        [Fact]
        public void CreateActiveContract_EndBeforeStart_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _sut.CreateActiveContract(1, DateTime.Today, DateTime.Today.AddDays(-5), "Standard")
            );
        }

        [Fact]
        public void CreateDraftContract_CreatedAtIsSetToNow()
        {
            var before = DateTime.UtcNow.AddSeconds(-1);
            var contract = _sut.CreateDraftContract(1, DateTime.Today, DateTime.Today.AddYears(1), "Basic");
            var after = DateTime.UtcNow.AddSeconds(1);

            Assert.InRange(contract.CreatedAt, before, after);
        }
    }
}
