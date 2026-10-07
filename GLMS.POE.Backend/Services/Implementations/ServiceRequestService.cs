using Microsoft.EntityFrameworkCore;
using GLMS.POE.Shared.Models;
using GLMS.POE.Backend.Services.Interfaces;
using GLMS.POE.Backend.Services;
using GLMS.POE.Backend.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GLMS.POE.Backend.Services.Implementations
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly ApplicationDbContext _db;
        private readonly IValidationStrategy _validator;

        public ServiceRequestService(ApplicationDbContext db)
        {
            _db = db;
            _validator = new CompositeValidationStrategy(new IValidationStrategy[]
            {
                new ActiveContractValidationStrategy(),
                new ContractDateValidationStrategy()
            });
        }

        public async Task<List<ServiceRequest>> GetAllAsync()
        {
            return await _db.ServiceRequests
                .Include(sr => sr.Contract)
                    .ThenInclude(c => c!.Client)
                .OrderByDescending(sr => sr.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ServiceRequest>> GetByContractAsync(int contractId)
        {
            return await _db.ServiceRequests
                .Where(sr => sr.ContractId == contractId)
                .OrderByDescending(sr => sr.CreatedAt)
                .ToListAsync();
        }

        public async Task<ServiceRequest> CreateAsync(ServiceRequest request)
        {
            var contract = await _db.Contracts.FindAsync(request.ContractId)
                ?? throw new KeyNotFoundException($"Contract {request.ContractId} not found.");

            if (!_validator.IsValid(contract))
                throw new InvalidOperationException(_validator.ErrorMessage);

            _db.ServiceRequests.Add(request);
            await _db.SaveChangesAsync();
            return request;
        }

        // Interface compatibility wrappers / implementations

        public async Task<List<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<ServiceRequest> GetServiceRequestByIdAsync(int id)
        {
            var sr = await _db.ServiceRequests
                .Include(s => s.Contract)
                    .ThenInclude(c => c!.Client)
                .FirstOrDefaultAsync(s => s.Id == id)
                ?? throw new KeyNotFoundException($"ServiceRequest {id} not found.");

            return sr;
        }

        public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request)
        {
            return await CreateAsync(request);
        }

        public async Task<ServiceRequest> UpdateServiceRequestAsync(ServiceRequest request)
        {
            var existing = await _db.ServiceRequests.FindAsync(request.Id)
                ?? throw new KeyNotFoundException($"ServiceRequest {request.Id} not found.");

            var contract = await _db.Contracts.FindAsync(request.ContractId)
                ?? throw new KeyNotFoundException($"Contract {request.ContractId} not found.");

            if (!_validator.IsValid(contract))
                throw new InvalidOperationException(_validator.ErrorMessage);

            _db.Entry(existing).CurrentValues.SetValues(request);
            await _db.SaveChangesAsync();
            return request;
        }

        public async Task<ServiceRequest> DeleteServiceRequestAsync(int id)
        {
            var existing = await _db.ServiceRequests.FindAsync(id)
                ?? throw new KeyNotFoundException($"ServiceRequest {id} not found.");

            _db.ServiceRequests.Remove(existing);
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<List<ServiceRequest>> GetServiceRequestsByContractAsync(int contractId)
        {
            return await GetByContractAsync(contractId);
        }
    }
}
