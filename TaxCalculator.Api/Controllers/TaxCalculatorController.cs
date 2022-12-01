using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Api.Dtos;
using TaxCalculator.Core.Configuration.Models;
using TaxCalculator.Core.Domain.Interfaces;
using TaxCalculator.Core.Interfaces;
using TaxCalculator.Core.Models;

namespace TaxCalculator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxCalculatorController : ControllerBase
    {
        private readonly ITaxCalculator _taxCalculator;
        private readonly ITaxPayerRepository _taxPayerRepository;

        public TaxCalculatorController(
            ITaxCalculator taxCalculator,
            ITaxPayerRepository taxPayerRepository
            )
        {
            _taxCalculator = taxCalculator;
            _taxPayerRepository = taxPayerRepository;
        }

        [HttpPost()]
        public async Task<ActionResult<TaxCalculationResult>> Post(TaxPayerDto taxPayerDto)
        {
            var result = await _taxPayerRepository.Get(taxPayerDto.Ssn);
            if (result != null) 
            {
                return Ok(result);
            }

            var taxPayerResult = TaxPayer.Create(
                fullName: taxPayerDto.FullName,
                dateOfBirth: taxPayerDto.DateOfBirth,
                grossIncome: taxPayerDto.GrossIncome,
                ssn: taxPayerDto.Ssn,
                charity: taxPayerDto.CharitySpent);

            if(taxPayerResult.IsFailure)
            {
                return BadRequest(taxPayerResult.Error);
            }

            var taxCalculation = taxPayerResult.Value.CalculateTax(_taxCalculator);
            await _taxPayerRepository.Save(taxPayerDto.Ssn.ToString(), taxCalculation);
            return Ok(taxCalculation);
        }
    }
}