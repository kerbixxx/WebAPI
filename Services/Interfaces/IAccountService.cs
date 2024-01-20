using SimbirSoft.Models;

namespace SimbirSoft.Services.Interfaces
{
    public interface IAccountService
    {
        public bool IsValid(AccountRequest request);
        public bool IsValidEmail(string email);
        public List<AccountResponse> Sort(List<AccountResponse> accountList);
    }
}
