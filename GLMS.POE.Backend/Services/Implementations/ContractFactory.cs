using GLMS.POE.Backend.Services.Interfaces;
using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;

namespace GLMS.POE.Backend.Services.Implementations
{
    public class ContractFactory : IContractFactory
    {
        public Contract CreateDraftContract(int clientId, DateTime startDate, DateTime endDate, string serviceLevel)
        {
            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date.");

            return new Contract
            {
                ClientId = clientId,
                StartDate = startDate,
                EndDate = endDate,
                ServiceLevel = serviceLevel,
                Status = ContractStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };
        }

        public Contract CreateActiveContract(int clientId, DateTime startDate, DateTime endDate, string serviceLevel)
        {
            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date.");

            return new Contract
            {
                ClientId = clientId,
                StartDate = startDate,
                EndDate = endDate,
                ServiceLevel = serviceLevel,
                Status = ContractStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
