using GLMS.POE.Backend.Services.Interfaces;
using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;

namespace GLMS.POE.Backend.Services.Implementations
{
      public class ContractObserver : IContractObserver
    {
        private readonly ILogger<ContractObserver> _logger;

        private static readonly List<string> _auditLog = new();
        public static IReadOnlyList<string> AuditLog => _auditLog.AsReadOnly();

        public ContractObserver(ILogger<ContractObserver> logger)
        {
            _logger = logger;
        }

        public Task OnStatusChangedAsync(Contract contract, ContractStatus oldStatus, ContractStatus newStatus)
        {
            var message = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Contract #{contract.Id} " +
                          $"(Client: {contract.Client?.Name ?? contract.ClientId.ToString()}) " +
                          $"status changed: {oldStatus} → {newStatus}";

            _logger.LogInformation(message);
            _auditLog.Add(message);

            if (newStatus == ContractStatus.Expired)
            {
                _logger.LogWarning("Contract #{Id} has EXPIRED — service requests will be blocked.", contract.Id);
            }

            return Task.CompletedTask;
        }

        public Task NotifyAsync(Contract contract, string message)
        {
            throw new NotImplementedException();
        }
    }
}
