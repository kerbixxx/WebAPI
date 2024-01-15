using SimbirSoft.Models;
using SimbirSoft.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class AccountServiceTests
    {
        [Fact]
        public void IsValid_ValidRequest_ReturnsTrue()
        {
            var service = new AccountService();
            var request = new AccountRequest
            {
                firstName = "John",
                lastName = "Doe",
                email = "john.doe@example.com",
                password = "StrongPassword123"
            };

            var result = service.IsValid(request);

            Assert.True(result);
        }

        [Theory]
        [InlineData(null, "Doe", "john.doe@example.com", "StrongPassword123")]
        [InlineData("John", null, "john.doe@example.com", "StrongPassword123")]
        // Можно добавить больше InlineData для качественной проверки
        public void IsValid_InvalidRequest_ReturnsFalse(string firstName, string lastName, string email, string password)
        {
            var service = new AccountService();
            var request = new AccountRequest
            {
                firstName = firstName,
                lastName = lastName,
                email = email,
                password = password
            };

            var result = service.IsValid(request);

            Assert.False(result);
        }
    }
}
