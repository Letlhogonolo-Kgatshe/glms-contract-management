using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;
namespace GLMS.POE.Backend.Services.Interfaces
{
    public interface IContractService
    {
        Task<List<Contract>> GetAllContractsAsync();
        Task<List<Contract>> SearchContractsAsync(DateTime? startFrom, DateTime? startTo, ContractStatus? status);
        Task<Contract?> GetContractByIdAsync(int id);
        Task<Contract> CreateContractAsync(Contract contract);
        Task UpdateContractAsync(Contract contract);
        Task UpdateContractStatusAsync(int id, ContractStatus newStatus);
        Task DeleteContractAsync(int id);
        Task<List<Contract>> GetContractsByClientAsync(int clientId);
    }
}
