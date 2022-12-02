using Microsoft.Extensions.Options;
using TaxCalculator.Core.Configuration;
using TaxCalculator.Core.Configuration.Models;

namespace TaxCalculator.Api.Configuration
{
    public class TaxConfiguration : ITaxConfiguration
    {
        private readonly IOptionsSnapshot<TaxConfig> _taxConfigurationOptions;

        public TaxConfiguration(IOptionsSnapshot<TaxConfig> taxConfigurationOptions)
        {
            _taxConfigurationOptions = taxConfigurationOptions;
        }

        public Task<TaxConfig> GetTaxConfiguration()
            => Task.FromResult(_taxConfigurationOptions.Value);
    }
}
