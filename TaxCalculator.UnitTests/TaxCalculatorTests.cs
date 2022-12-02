using Microsoft.Extensions.Options;
using NSubstitute;
using TaxCalculator.Core.Configuration.Models;
using TaxCalculator.Core.Models;

namespace TaxCalculator.UnitTests
{
    public class TaxCalculatorTests
    {
        private readonly IOptionsSnapshot<TaxConfig> _taxConfigMock;
        private readonly TaxConfig _taxConfig;
        private readonly Core.Domain.TaxCalculator _taxCalculator;

        public TaxCalculatorTests()
        {
            _taxConfigMock = Substitute.For<IOptionsSnapshot<TaxConfig>>();
            _taxConfig = new TaxConfig();
            _taxConfig.SocialContribution = new SocialContribution();
            _taxConfigMock.Value.Returns(_taxConfig);
            _taxCalculator = new Core.Domain.TaxCalculator(_taxConfigMock);
        }

        [Fact]
        public void IsNotTaxable_ShouldReturnTrueAndValidCalculationResult_IfGrossIncomeIsSmallerThanMinTaxableAmount()
        {
            // Arrange
            var taxConfig = new TaxConfig();
            taxConfig.MinTaxableAmount = 1000;
            _taxConfigMock.Value.Returns(taxConfig);
            var sut = new Core.Domain.TaxCalculator(_taxConfigMock);
            var grossIncome = taxConfig.MinTaxableAmount - 1;

            // Act
            var isNotTaxable = sut.IsNotTaxable(grossIncome, out TaxCalculationResult? taxCalculationResult);

            // Assert
            Assert.True(isNotTaxable);
            Assert.Equal(grossIncome, taxCalculationResult!.GrossIncome);
        }

        [Fact]
        public void IsNotTaxable_ShouldReturnFalseAndNull_IfGrossIncomeIsBiggerThanMinTaxableAmount()
        {
            // Arrange
            _taxConfig.MinTaxableAmount = 1000;
            var grossIncome = _taxConfig.MinTaxableAmount + 1;

            // Act
            var isNotTaxable = _taxCalculator.IsNotTaxable(grossIncome, out TaxCalculationResult? taxCalculationResult);

            // Assert
            Assert.False(isNotTaxable);
            Assert.Equal(null, taxCalculationResult);
        }


        [Fact]
        public void CalculateCharityAmount_ShouldReturnCharitySpentValue_IfCharitySpentIsLessThanMaxCharityRate()
        {
            // Arrange
            _taxConfig.MaxCharityRate = 0.1m;
            var grossIncome = 1000m;
            var charitySpent = (grossIncome * _taxConfig.MaxCharityRate) - 1;

            // Act
            var charityCalculation = _taxCalculator.CalculateCharityAmount(grossIncome, charitySpent);

            // Assert
            Assert.Equal(charitySpent, charityCalculation);
        }

        [Fact]
        public void CalculateCharityAmount_ShouldReturnMaxCharityValue_IfCharitySpentIsBiggerThanMaxCharityRate()
        {
            // Arrange

            _taxConfig.MaxCharityRate = 0.1m;
            var grossIncome = 1000m;
            var expectedCalculation = (grossIncome * _taxConfig.MaxCharityRate);
            var charitySpent = expectedCalculation + 1;

            // Act
            var charityCalculation = _taxCalculator.CalculateCharityAmount(grossIncome, charitySpent);

            // Assert
            Assert.Equal(expectedCalculation, charityCalculation);
        }

        [Fact]
        public void CalculateTaxableGrossIncome_ShouldSubstractCharitySpentFromGrossIncome()
        {
            // Arrange
            var grossIncome = 1000m;
            var charitySpent = grossIncome / 2;
            var expectedCalculation = grossIncome - charitySpent;
            
            // Act
            var charityCalculation = _taxCalculator.CalculateTaxableGrossIncome(grossIncome, charitySpent);

            // Assert
            Assert.Equal(expectedCalculation, charityCalculation);
        }

        [Fact]
        public void CalculateIncomeTax_ShouldSubstractMinTaxableAmountAndMultyplyByIncomeTax()
        {
            // Arrange
            _taxConfig.MinTaxableAmount = 500m;
            _taxConfig.IncomeTax = 0.1m;
            var grossIncome = 1000m;
            var expectedCalculation = (grossIncome - _taxConfig.MinTaxableAmount) * _taxConfig.IncomeTax;

            // Act
            var charityCalculation = _taxCalculator.CalculateIncomeTax(grossIncome);

            // Assert
            Assert.Equal(expectedCalculation, charityCalculation);
        }

        [Fact]
        public void CalculateSocialTax_ShouldUseGrossIncomeValueInCalculation_IfGrossIncomeIsSmallerThanTopTaxableAmount()
        {
            // Arrange
            var grossIncome = 1000m;
            _taxConfig.SocialContribution!.TopTaxableAmount = grossIncome + 1;
            _taxConfig.SocialContribution!.Value = 0.1m;
            _taxConfig.MinTaxableAmount = 500m;

            var expectedCalculation = (grossIncome - _taxConfig.MinTaxableAmount) * _taxConfig.SocialContribution!.Value;
            
            // Act
            var charityCalculation = _taxCalculator.CalculateSocialTax(grossIncome);

            // Assert
            Assert.Equal(expectedCalculation, charityCalculation);
        }

        [Fact]
        public void CalculateSocialTax_ShouldUseTopTaxableAmountValueInCalculation_IfGrossIncomeIsBiggerThanTopTaxableAmount()
        {
            // Arrange
            var grossIncome = 1000m;
            _taxConfig.SocialContribution!.TopTaxableAmount = grossIncome - 1;
            _taxConfig.SocialContribution!.Value = 0.1m;
            _taxConfig.MinTaxableAmount = 500m;

            var expectedCalculation = (_taxConfig.SocialContribution!.TopTaxableAmount - _taxConfig.MinTaxableAmount) * _taxConfig.SocialContribution!.Value;

            // Act
            var charityCalculation = _taxCalculator.CalculateSocialTax(grossIncome);

            // Assert
            Assert.Equal(expectedCalculation, charityCalculation);
        }
    }
}