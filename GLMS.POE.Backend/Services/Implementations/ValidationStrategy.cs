using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;
using GLMS.POE.Backend.Services.Interfaces;

namespace GLMS.POE.Backend.Services.Implementations
{
        public class ActiveContractValidationStrategy : IValidationStrategy
    {
        public string ErrorMessage => "Service requests can only be created against Active contracts. " +
                                     "This contract is either Expired, On Hold, or still in Draft.";

        public bool IsValid(Contract contract)
        {
            return contract.Status == ContractStatus.Active;
        }
    }

        public class ContractDateValidationStrategy : IValidationStrategy
    {
        public string ErrorMessage => "Service requests cannot be raised against a contract whose end date has passed.";

        public bool IsValid(Contract contract)
        {
            return contract.EndDate >= DateTime.UtcNow.Date;
        }
    }

       public class CompositeValidationStrategy : IValidationStrategy
    {
        private readonly IEnumerable<IValidationStrategy> _strategies;
        private string _errorMessage = string.Empty;

        public CompositeValidationStrategy(IEnumerable<IValidationStrategy> strategies)
        {
            _strategies = strategies;
        }

        public string ErrorMessage => _errorMessage;

        public bool IsValid(Contract contract)
        {
            foreach (var strategy in _strategies)
            {
                if (!strategy.IsValid(contract))
                {
                    _errorMessage = strategy.ErrorMessage;
                    return false;
                }
            }
            return true;
        }
    }
}
