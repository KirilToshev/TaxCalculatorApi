using Microsoft.Extensions.Options;
using TaxCalculator.Core.Configuration.Models;
using TaxCalculator.Core.Domain.Interfaces;
using TaxCalculator.Core.Models;

namespace TaxCalculator.Core.Domain
{
    public class TaxCalculator : ITaxCalculator
    {
        private readonly TaxConfig _taxConfiguration;

        public TaxCalculator(IOptionsSnapshot<TaxConfig> taxConfiguration)
        {
            this._taxConfiguration = taxConfiguration.Value;
        }

        public bool IsNotTaxable(decimal grossIncome, out TaxCalculationResult? taxCalculationResult)
        {
            if (grossIncome <= _taxConfiguration.MinTaxableAmount)
            {
                taxCalculationResult = new TaxCalculationResult(grossIncome, 0, 0, 0);
                return true;
            }

            taxCalculationResult = null;
            return false;
        }

        public decimal CalculateCharityAmount(decimal grossIncome, decimal charitySpent)
        {
            var maxCharityAmount = grossIncome * _taxConfiguration.MaxCharityRate;
            return Math.Min(maxCharityAmount, charitySpent);
        }

        public decimal CalculateTaxableGrossIncome(decimal grossIncome, decimal charitySpent)
        {
            return grossIncome - charitySpent;
        }

        public decimal CalculateIncomeTax(decimal grossIncome)
        {
            return (grossIncome - _taxConfiguration.MinTaxableAmount) * _taxConfiguration.IncomeTax;
        }

        public decimal CalculateSocialTax(decimal grossIncome)
        {
            var maxSocialTaxAmount = Math.Min(grossIncome, _taxConfiguration.SocialContribution!.TopTaxableAmount);
            return (maxSocialTaxAmount - _taxConfiguration.MinTaxableAmount) * _taxConfiguration.SocialContribution.Value;
        }
    }
}
