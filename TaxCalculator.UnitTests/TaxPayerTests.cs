using NSubstitute;
using TaxCalculator.Core.Configuration.Models;
using TaxCalculator.Core.Domain.Interfaces;

namespace TaxCalculator.UnitTests
{
    public class TaxPayerTests
    {
        private readonly ITaxCalculator _taxCalculator;

        public TaxPayerTests()
        {
            _taxCalculator = Substitute.For<ITaxCalculator>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("SingleNameOnly")]
        [InlineData("@##% 85434")]
        public void Create_ShouldFail_IfFullNameIsInvalid(string fullName)
        {
            // Act
            var result = TaxPayer.Create(fullName, DateTime.Now, 1000m, "12345", 0);

            // Assert
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
            // Act
            var result = TaxPayer.Create("Valid FullName", DateTime.Now, 0, ssn, 0);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("A valid 5 to 10 digits number unique per tax payer", result.Error);
        }

        [Fact]
        public void Create_ShouldFail_IfGrossIncomeIsInvalid()
        {
            // Act
            var invalidGrossIncome = 0m;
            var result = TaxPayer.Create("Valid FullName", DateTime.Now, invalidGrossIncome, "12345", 0);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Gross Income can not be negative or 0", result.Error);
        }

        [Fact]
        public void Create_ShouldFail_IfCharitySpentIsInvalid()
        {
            // Act
            var invalidCharitySpent = -1m;
            var result = TaxPayer.Create("Valid FullName", DateTime.Now, 1000m, "12345", invalidCharitySpent);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Charity Spent can not be negative", result.Error);
        }

        [Fact]
        public void Create_ShouldSuccessed_IfAllParameterAreValid()
        {
            // Act
            var result = TaxPayer.Create("Valid FullName", DateTime.Now, 1000m, "12345", 0);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void CalculateTax_ShouldReturnCorrectlyCalculatedTax()
        {
            // Arrange
            var grossIncome = 1000m;
            var taxPayer = TaxPayer.Create("Valid FullName", DateTime.Now, grossIncome, "12345", 0).Value;
            var charityCalculation = 100m;
            var taxableGrossIncome = 1000m;
            var incomeTaxCalculation = 200m;
            var socialTaxCalculation = 300m;
            var totalTax = incomeTaxCalculation + socialTaxCalculation;
            var netIncome = grossIncome - totalTax;
            _taxCalculator.CalculateCharityAmount(Arg.Any<decimal>(), Arg.Any<decimal>())
                .Returns(charityCalculation);
            _taxCalculator.CalculateTaxableGrossIncome(Arg.Any<decimal>(), Arg.Any<decimal>())
                .Returns(taxableGrossIncome);
            _taxCalculator.CalculateIncomeTax(Arg.Any<decimal>())
                .Returns(incomeTaxCalculation);
            _taxCalculator.CalculateSocialTax(Arg.Any<decimal>())
                .Returns(socialTaxCalculation);

            // Act
            var taxCalculationResult = taxPayer.CalculateTax(_taxCalculator);

            // Assert
            Assert.Equal(grossIncome, taxCalculationResult.GrossIncome);
            Assert.Equal(charityCalculation, taxCalculationResult.CharitySpent);
            Assert.Equal(incomeTaxCalculation, taxCalculationResult.IncomeTax);
            Assert.Equal(socialTaxCalculation, taxCalculationResult.SocialTax);
            Assert.Equal(netIncome, taxCalculationResult.TotalTax);
            Assert.Equal(netIncome, taxCalculationResult.NetIncome);
        }
    }
}