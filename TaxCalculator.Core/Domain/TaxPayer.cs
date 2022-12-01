using CSharpFunctionalExtensions;
using TaxCalculator.Core.Domain.Interfaces;
using TaxCalculator.Core.Models;
using TaxCalculator.Core.ValueObjects;

namespace TaxCalculator.Core.Configuration.Models
{
    public class TaxPayer
    {
        private TaxPayer(
            FullName fullName,
            DateTime? dateOfBirth,
            decimal grossIncome,
            SSN ssn,
            decimal charitySpent
            )
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            GrossIncome = grossIncome;
            SSN = ssn;
            CharitySpent = charitySpent;
        }

        public static Result<TaxPayer> Create(
            string fullName,
            DateTime? dateOfBirth,
            decimal grossIncome,
            string ssn,
            decimal? charity
            )
        {
            var fullNameResult = FullName.Create(fullName);
            if (fullNameResult.IsFailure)
            {
                return Result.Failure<TaxPayer>(fullNameResult.Error);
            }

            var ssnResult = SSN.Create(ssn);
            if (ssnResult.IsFailure)
            {
                return Result.Failure<TaxPayer>(ssnResult.Error);
            }

            if (grossIncome <= 0)
            {
                return Result.Failure<TaxPayer>("Gross Income can not be negative or 0");
            }

            var charitySpent = charity.HasValue ? charity.Value : 0;
            if (charitySpent < 0)
            {
                return Result.Failure<TaxPayer>("Charity Spent can not be negative");
            }

            var taxPayer = new TaxPayer(
                fullName: fullNameResult.Value,
                dateOfBirth: dateOfBirth,
                grossIncome: grossIncome,
                ssn: ssnResult.Value,
                charitySpent: charitySpent);

            return Result.Success(taxPayer);
        }

        public FullName FullName { get; private set; }        
        public DateTime? DateOfBirth { get; private set; }
        public decimal GrossIncome { get; private set; }
        public SSN SSN { get; private set; }
        public decimal CharitySpent { get; private set; }

        public TaxCalculationResult CalculateTax(ITaxCalculator taxCalculator)
        {
            if(taxCalculator.IsNotTaxable(GrossIncome, out TaxCalculationResult? result))
            {
                return result!;
            }

            var charityCalculation = taxCalculator.CalculateCharityAmount(GrossIncome, CharitySpent);
            var taxableGrossIncome = taxCalculator.CalculateTaxableGrossIncome(GrossIncome, charityCalculation);
            var incomeTaxCalculation = taxCalculator.CalculateIncomeTax(taxableGrossIncome);
            var socialTaxCalculation = taxCalculator.CalculateSocialTax(taxableGrossIncome);

            return new TaxCalculationResult(GrossIncome, charityCalculation, incomeTaxCalculation, socialTaxCalculation);
        }
    }
}
