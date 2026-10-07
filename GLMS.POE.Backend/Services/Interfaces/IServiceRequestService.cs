using GLMS.POE.Shared.Models;

namespace GLMS.POE.Backend.Services.Interfaces
{
    public interface IServiceRequestService
    {
        Task<List<ServiceRequest>> GetByContractAsync(int contractId);
        Task<List<ServiceRequest>> GetAllServiceRequestsAsync();
        Task<ServiceRequest> GetServiceRequestByIdAsync(int id);
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request);
        Task<ServiceRequest> UpdateServiceRequestAsync(ServiceRequest request);
        Task<ServiceRequest> DeleteServiceRequestAsync(int id);
        Task<List<ServiceRequest>> GetServiceRequestsByContractAsync(int contractId);
            Task<List<ServiceRequest>> GetAllAsync();
    }
}
