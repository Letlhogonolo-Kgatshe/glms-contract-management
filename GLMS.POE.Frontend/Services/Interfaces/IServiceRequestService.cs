using GLMS.POE.Shared.Models;

namespace GLMS.POE.Frontend.Services.Interfaces;

public interface IServiceRequestService
{
    Task<List<ServiceRequest>> GetAllAsync();
    Task<List<ServiceRequest>> GetAllServiceRequestsAsync();
    Task<ServiceRequest?> GetByIdAsync(int id);
    Task<(bool success, string error)> CreateAsync(ServiceRequest request);
}