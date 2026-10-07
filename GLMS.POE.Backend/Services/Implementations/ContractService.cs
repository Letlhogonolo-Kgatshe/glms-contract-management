using Microsoft.EntityFrameworkCore;
using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;
using GLMS.POE.Backend.Services.Interfaces;
using GLMS.POE.Backend.Data;

namespace GLMS.POE.Backend.Services.Implementations
{
    public class ContractService : IContractService
    {
        private readonly ApplicationDbContext _db;
        private readonly IContractObserver _observer;

        public ContractService(ApplicationDbContext db, IContractObserver observer)
        {
            _db = db;
            _observer = observer;
        }

        public async Task<List<Contract>> GetAllContractsAsync()
        {
            return await _db.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Contract>> SearchContractsAsync(DateTime? startFrom, DateTime? startTo, ContractStatus? status)
        {
            var query = _db.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .AsQueryable();

            if (startFrom.HasValue)
                query = query.Where(c => c.StartDate >= startFrom.Value);

            if (startTo.HasValue)
                query = query.Where(c => c.StartDate <= startTo.Value);

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
        }

        public async Task<Contract?> GetContractByIdAsync(int id)
        {
            return await _db.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Contract> CreateContractAsync(Contract contract)
        {
            ValidateContract(contract);

            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();

            await _observer.NotifyAsync(contract, $"Contract {contract.Id} created.");

            return contract;
        }

        public async Task UpdateContractAsync(Contract contract)
        {
            ValidateContract(contract);

            _db.Contracts.Update(contract);
            await _db.SaveChangesAsync();

            await _observer.NotifyAsync(contract, $"Contract {contract.Id} updated.");
        }

        public async Task UpdateContractStatusAsync(int id, ContractStatus newStatus)
        {
            var contract = await _db.Contracts.FindAsync(id);

            if (contract == null)
                throw new KeyNotFoundException("Contract not found.");

            contract.Status = newStatus;

            await _db.SaveChangesAsync();

            await _observer.NotifyAsync(contract, $"Contract {contract.Id} status changed to {newStatus}.");
        }

        public async Task DeleteContractAsync(int id)
        {
            var contract = await _db.Contracts.FindAsync(id);

            if (contract == null)
                throw new KeyNotFoundException("Contract not found.");

            _db.Contracts.Remove(contract);
            await _db.SaveChangesAsync();

            await _observer.NotifyAsync(contract, $"Contract {contract.Id} deleted.");
        }

        public async Task<List<Contract>> GetContractsByClientAsync(int clientId)
        {
            return await _db.Contracts
                .Where(c => c.ClientId == clientId)
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .ToListAsync();
        }

        private static void ValidateContract(Contract contract)
        {
            if (contract.EndDate <= contract.StartDate)
            {
                throw new ArgumentException("End date must be after start date.");
            }

            if (string.IsNullOrWhiteSpace(contract.ServiceLevel))
            {
                throw new ArgumentException("Service level is required.");
            }
        }
    }
}
