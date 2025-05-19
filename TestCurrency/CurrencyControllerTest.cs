using CurrencyConversionService.Controllers;
using CurrencyConversionService.Data;
using CurrencyConversionService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestCurrency
{
    [TestClass]
    public sealed class CurrencyControllerTest
    {
        [TestMethod]
        public void GetLatestDanishKronesRateOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CurrencyDbContext>()
                .UseSqlServer(connectionString: "Server=localhost;Database=Currency;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;
            using (var context = new CurrencyDbContext(options))
            {
                var controller = new CurrencyController(context);
                // Act
                var result = controller.GetLatestDanishKronesRate("USD") as OkObjectResult;
                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(200, result.StatusCode);
            }
        }

        [TestMethod]
        public void GetLatestDanishKronesRateNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CurrencyDbContext>()
                .UseSqlServer(connectionString: "Server=localhost;Database=Currency;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;
            using (var context = new CurrencyDbContext(options))
            {
                var controller = new CurrencyController(context);
                // Act
                var result = controller.GetLatestDanishKronesRate("AAA") as NotFoundObjectResult;
                // Assert
                Assert.AreEqual(404, result.StatusCode);
            }
        }
    }
}
