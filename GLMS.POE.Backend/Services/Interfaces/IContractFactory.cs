using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;

namespace GLMS.POE.Backend.Services.Interfaces
{
    public interface IContractFactory
    {
        Contract CreateDraftContract(int clientId, DateTime startDate, DateTime endDate, string serviceLevel);
        Contract CreateActiveContract(int clientId, DateTime startDate, DateTime endDate, string serviceLevel);
    }
}
