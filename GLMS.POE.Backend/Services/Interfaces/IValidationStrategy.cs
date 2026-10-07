using GLMS.POE.Shared.Models;

namespace GLMS.POE.Backend.Services.Interfaces
{
    public interface IValidationStrategy
    {
        bool IsValid(Contract contract);
        string ErrorMessage { get; }
    }
}
