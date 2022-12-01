using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TaxCalculator.Core.Configuration.Models;
using TaxCalculator.Core.Interfaces;
using TaxCalculator.Core.Models;

namespace TaxCalculator.Data
{
    public class TaxPayerRepository : ITaxPayerRepository
    {
        private readonly TaxConfigurationOptions _taxConfiguration;
        private readonly IMemoryCache _memoryCache;

        public TaxPayerRepository(
            IMemoryCache memoryCache,
            IOptionsSnapshot<TaxConfigurationOptions> taxConfigurationOptions)
        {
            _taxConfiguration = taxConfigurationOptions.Value;
            _memoryCache = memoryCache;
        }

        public Task<TaxCalculationResult?> Get(string ssn)
        {
            return Task.FromResult(_memoryCache.Get<TaxCalculationResult>(ssn));
        }

        public Task<bool> Save(string ssn, TaxCalculationResult taxCalculationResult)
        {
            if (!_memoryCache.TryGetValue(ssn, out TaxCalculationResult? value))
            {
                try
                {
                    _memoryCache.Set(ssn, taxCalculationResult, TimeSpan.FromHours(_taxConfiguration.CacheExpiration));
                }
                catch (Exception e)
                {
                    // Log exception
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }
    }
}