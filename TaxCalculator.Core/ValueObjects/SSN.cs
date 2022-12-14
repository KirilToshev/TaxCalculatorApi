using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace TaxCalculator.Core.ValueObjects
{
    public class SSN : ValueObject<SSN, string>
    {
        private SSN(string ssn)
            : base(ssn) { }

        public static Result<SSN> Create(string ssn)
        {
            if (string.IsNullOrWhiteSpace(ssn) || !Regex.IsMatch(ssn, @"^\d{5,10}$")  )
            {
                return Result.Failure<SSN>("A valid 5 to 10 digits number unique per tax payer");
            }

            return Result.Success(new SSN(ssn));
        }
    }
}
