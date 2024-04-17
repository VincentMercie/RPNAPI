using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using RPNAPI;

namespace RPN.TESTS.TestsIntegrations
{
    [TestFixture]
    public class RPNSolverControllerTests
    {
        private WebApplicationFactory<RPNAPI.Program> _factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new WebApplicationFactory<RPNAPI.Program>();
        }

        [Test]
        [TestCase("5 3 +", "8")]
        [TestCase("10 2 ^", "100")]
        [TestCase("3 4 *", "12")]
        [TestCase("10 5 -", "5")]
        public async Task Post_ValidRPNExpression_ReturnsCorrectResult(string expression, string expected)
        {
            // Arrange
            var client = _factory.CreateClient();
            var requestContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync($"/RPNSolver/{expression}", requestContent);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(responseString, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("5 + 3", HttpStatusCode.BadRequest)]
        [TestCase("10 0 /", HttpStatusCode.BadRequest)]
        public async Task Post_InvalidRPNExpression_ReturnsBadRequest(string expression, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var client = _factory.CreateClient();
            var requestContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync($"/RPNSolver/{expression}", requestContent);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        [Test]
        [TestCase("", HttpStatusCode.NotFound)]
        public async Task Post_EmptyRPNExpression_ReturnsNotFound(string expression, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var client = _factory.CreateClient();
            var requestContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync($"/RPNSolver/{expression}", requestContent);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory.Dispose();
        }
    }
}