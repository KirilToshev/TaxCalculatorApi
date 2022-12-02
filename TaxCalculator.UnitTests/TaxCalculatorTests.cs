using Microsoft.Extensions.Options;
using NSubstitute;
using TaxCalculator.Core.Configuration.Models;
using TaxCalculator.Core.Models;
using TaxCalculator.Core.ValueObjects;

namespace TaxCalculator.UnitTests
{
    public class TaxPayerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("SingleNameOnly")]
        [InlineData("@##% 85434")]
        public void Create_ShouldFail_IfFullNameIsInvalid(string fullName)
        {
            //Act
            var result = TaxPayer.Create(fullName, DateTime.Now, 1000m, "12345", 0);

            Assert.True(result.IsFailure);
            Assert.Equal("FullName – at least two words separated by space – allowed symbols letters and spaces only",
                result.Error);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("SingleNameOnly")]
        [InlineData("8543")]
        [InlineData("8543423423452342342")]
        public void Create_ShouldFail_IfSsnIsInvalid(string ssn)
        {
            //Act
            var result = TaxPayer.Create("Valid FullName", DateTime.Now, 0, ssn, 0);

            Assert.True(result.IsFailure);
            Assert.Equal("A valid 5 to 10 digits number unique per tax payer", result.Error);
        }

        [Fact]
        public void Create_ShouldFail_IfGrossIncomeIsInvalid()
        {
            //Act
            var invalidGrossIncome = 0m;
            var result = TaxPayer.Create("Valid FullName", DateTime.Now, invalidGrossIncome, "12345", 0);

            Assert.True(result.IsFailure);
            Assert.Equal("Gross Income can not be negative or 0", result.Error);
        }

        [Fact]
        public void Create_ShouldFail_IfCharitySpentIsInvalid()
        {
            //Act
            var invalidCharitySpent = -1m;
            var result = TaxPayer.Create("Valid FullName", DateTime.Now, 1000m, "12345", invalidCharitySpent);

            Assert.True(result.IsFailure);
            Assert.Equal("Charity Spent can not be negative", result.Error);
        }

        [Fact]
        public void Create_ShouldSuccessed_IfAllParameterAreValid()
        {
            //Act
            var result = TaxPayer.Create("Valid FullName", DateTime.Now, 1000m, "12345", 0);

            Assert.True(result.IsSuccess);
        }
    }
}