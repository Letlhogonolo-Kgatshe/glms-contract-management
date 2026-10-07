using GLMS.POE.Shared.Models;

namespace GLMS.POE.Backend.Services.Interfaces
{
        public interface IContractObserver
    {
        Task NotifyAsync(Contract contract, string message);

    }

}
