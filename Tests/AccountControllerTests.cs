using Microsoft.AspNetCore.Mvc;
using Moq;
using SimbirSoft.Controllers;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Interfaces;

namespace Tests
{
    public class AccountControllerTests
    {
        [Fact]
        public void GetAccount_ReturnsCorrectAccount()
        {
            var testAccountId = 1;
            var mockService = new Mock<IAccountService>();
            var mockRepo = new Mock<IAccountRepository>();
            Account testAccount = new Account { Id = testAccountId, firstName = "petr", lastName = "vasilyev", email = "test@example.com", password = "1234" };
            mockRepo.Setup(x => x.Get(testAccountId)).Returns(testAccount);

            var controller = new AccountsController(mockService.Object, mockRepo.Object);

            var result = controller.GetAccount(testAccountId);

            var actionResult = Assert.IsType<ActionResult<Account>>(result);
            var okResult = Assert.IsType<JsonResult>(actionResult.Result);
            var account = Assert.IsType<AccountResponse>(okResult.Value);
            Assert.Equal(testAccount.Id, account.Id);
            Assert.Equal(testAccount.email, account.email);
            Assert.Equal(testAccount.firstName, account.firstName);
        }

        [Fact]
        public void RegisterAccount_ReturnsAccountResponse_WhenValidRequest_Test()
        {
            var mockService = new Mock<IAccountService>();
            var mockRepo = new Mock<IAccountRepository>();
            var controller = new AccountsController(mockService.Object, mockRepo.Object);

            var request = new AccountRequest
            {
                firstName = "Baki",
                lastName = "Hanma",
                email = "baki.hanma@example.com",
                password = "StrongPassword123"
            };

            mockService.Setup(s => s.IsValid(request)).Returns(true);
            mockRepo.Setup(r => r.FirstOrDefault(It.IsAny<Func<Account, bool>>())).Returns((Account)null);

            var result = controller.Registration(request);

            var actionResult = Assert.IsType<ActionResult<AccountResponse>>(result);
            var createdAtActionResult = Assert.IsType<AccountResponse>(actionResult.Value);
            Assert.NotNull(createdAtActionResult);
        }

        [Fact]
        public void SearchAccounts_ReturnListOfAccounts_WhenValidRequest_Test()
        {
            var mockService = new Mock<IAccountService>();
            var mockRepo = new Mock<IAccountRepository>();
            var controller = new AccountsController(mockService.Object, mockRepo.Object);

            var result = controller.SearchAccounts("John", "Doe", "john", 1, 2);

            var actionResult = Assert.IsType<ActionResult<List<AccountResponse>>>(result);
            var createdAtActionResult = Assert.IsType<List<AccountResponse>>(actionResult.Value);
            Assert.NotNull(createdAtActionResult);
        }
    }
}