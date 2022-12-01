using Microsoft.Extensions.Options;
using TaxCalculator.Core.Configuration;
using TaxCalculator.Core.Configuration.Models;

namespace TaxCalculator.Api.Configuration
{
    public class TaxConfiguration : ITaxConfiguration
    {
        private readonly IOptionsSnapshot<TaxConfigurationOptions> _taxConfigurationOptions;

        public TaxConfiguration(IOptionsSnapshot<TaxConfigurationOptions> taxConfigurationOptions)
        {
            _taxConfigurationOptions = taxConfigurationOptions;
        }

        public Task<TaxConfigurationOptions> GetTaxConfiguration()
            => Task.FromResult(_taxConfigurationOptions.Value);
    }
}
