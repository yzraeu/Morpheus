using System.Threading.Tasks;
using UnitTest.Configuration;
using Xunit;

namespace UnitTest.Controllers
{
    public class TestControllerIntegrationTest : BaseIntegrationTest
    {
        private const string BaseUrl = "/v1/Test";

        public TestControllerIntegrationTest(BaseTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task DeveRetornarSucesso(){
            var response = await Client.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            
            var responseString  = await response.Content.ReadAsStringAsync();
            
            Assert.Equal("success", responseString);
        }
    }
}