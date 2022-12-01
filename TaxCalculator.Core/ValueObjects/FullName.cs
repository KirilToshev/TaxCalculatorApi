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
            if (!Regex.IsMatch(fullName, @"^[^\s]+( [\w]+)+$") || string.IsNullOrWhiteSpace(fullName))
            {
                return Result.Failure<FullName>("FullName – at least two words separated by space – allowed symbols letters and spaces only");
            }

            return Result.Success(new FullName(fullName));    
        }
    }
}
