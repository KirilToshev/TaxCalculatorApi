using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace TaxCalculator.Core.ValueObjects
{
    public class FullName : ValueObject<FullName, string>
    {
        private FullName(string fullName)
            : base(fullName) { }

        public static Result<FullName> Create(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName) || !Regex.IsMatch(fullName, @"(^[A-Za-z]{2,16})[ ]([A-Za-z]{2,16})"))
            {
                return Result.Failure<FullName>("FullName – at least two words separated by space – allowed symbols letters and spaces only");
            }

            return Result.Success(new FullName(fullName));    
        }
    }
}
