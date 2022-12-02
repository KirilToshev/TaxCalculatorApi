using TaxCalculator.Core.Configuration.Models;

namespace TaxCalculator.Core.Configuration
{
    public interface ITaxConfiguration
    {
        Task<TaxConfig> GetTaxConfiguration();
    }
}
