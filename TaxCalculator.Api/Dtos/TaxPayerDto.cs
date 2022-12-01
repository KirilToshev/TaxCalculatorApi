namespace TaxCalculator.Api.Dtos
{
    public record TaxPayerDto
    {
        public TaxPayerDto(
            string fullName,
            DateTime dateOfBirth,
            decimal grossIncome,
            string ssn,
            decimal charitySpent)
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            GrossIncome = grossIncome;
            Ssn = ssn;
            CharitySpent = charitySpent;
        }

        public string FullName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public decimal GrossIncome { get; private set; }
        public string Ssn { get; private set; }
        public decimal CharitySpent { get; private set; }
    }
}
