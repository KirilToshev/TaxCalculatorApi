using DataProcessing.WebAdmin.Api.IntegrationTests.TestTools;
using System.Net.Http.Json;
using TaxCalculator.Api.Dtos;

namespace TaxCalculator.IntegrationTests
{
    public class TaxCalculatorControllerTests : IClassFixture<TestApplicationFactory<Api.Program>>
    {
        private readonly HttpClient _httpClient;

        public TaxCalculatorControllerTests(TestApplicationFactory<Api.Program> testApplicationFactory)
        {
            _httpClient = testApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Post_ShouldCalculateTax()
        {
            //Arrange
            var dto = new TaxPayerDto("valid name", DateTime.Now.AddYears(-30), 3600m, "12345", 520m);

            //Act
            var response = await _httpClient.PostAsJsonAsync("/TaxCalculator/", dto);
            var contentAsString = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(contentAsString, @"{""grossIncome"":3600,""charitySpent"":360.00,""incomeTax"":224.0000,""socialTax"":300.00,""totalTax"":524.0000,""netIncome"":3076.0000}");
        }
    }
}
