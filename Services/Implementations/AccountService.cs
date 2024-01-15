using SimbirSoft.Models;
using SimbirSoft.Services.Interfaces;

namespace SimbirSoft.Services.Implementations
{
    public class AccountService : IAccountService
    {
        public AccountService() { }

        public bool IsValid(AccountRequest request)
        {
            return !(request.firstName == null || request.firstName == "" || request.firstName.Trim() == "") &&
                   !(request.lastName == null || request.lastName == "" || request.lastName.Trim() == "") &&
                   !(request.email == null || request.email == "" || request.email.Trim() == "") &&
                   !(request.password == null || request.password == "" || request.password.Trim() == "");
        }

        public List<AccountResponse> Sort(List<AccountResponse> accountList)
        {
            return accountList.OrderBy(a => a.Id).ToList();
        }
    }
}
