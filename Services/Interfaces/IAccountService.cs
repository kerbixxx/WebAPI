using SimbirSoft.Models;

namespace SimbirSoft.Services.Interfaces
{
    public interface IAccountService
    {
        public bool IsValid(AccountRequest request);
        public List<AccountResponse> Sort(List<AccountResponse> accountList);
    }
}
