namespace TaxCalculator.Core.Models
{
    public class TaxCalculationResult
    {
        public TaxCalculationResult(
            decimal grossIncome,
            decimal charitySpent,
            decimal incomeTax,
            decimal socialTax)
        {
            GrossIncome = grossIncome;
            CharitySpent = charitySpent;
            IncomeTax = incomeTax;
            SocialTax = socialTax;
        }

        public decimal GrossIncome { get; private set; }
        public decimal CharitySpent { get; private set; }
        public decimal IncomeTax { get; private set; }
        public decimal SocialTax { get; private set; }
        public decimal TotalTax => IncomeTax + SocialTax;
        public decimal NetIncome => GrossIncome - TotalTax;
    }
}
