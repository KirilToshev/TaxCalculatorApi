namespace TaxCalculator.Core.Configuration.Models
{
    public class TaxConfigurationOptions
    {
        public decimal MinTaxableAmount { get; set; }
        public decimal IncomeTax { get; set; }
        public SocialContribution? SocialContribution { get; set; }
        public decimal MaxCharityRate { get; set; }
        public int CacheExpiration { get; set; }
    }

    public class SocialContribution
    {
        public decimal Value { get; set; }
        public decimal TopTaxableAmount { get; set; }
    }
}
