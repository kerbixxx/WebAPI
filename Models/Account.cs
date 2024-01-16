using System.ComponentModel.DataAnnotations;

namespace SimbirSoft.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public Account() { }

        public Account(string firstName, string lastName, string email, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
        }

        public static explicit operator AccountResponse(Account account)
        {
            return new AccountResponse
            {
                Id = account.Id,
                firstName = account.firstName,
                lastName = account.lastName,
                email = account.email
            };
        }
    }

    public class AccountRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public static explicit operator Account(AccountRequest request)
        {
            return new Account
            {
                firstName = request.firstName,
                lastName = request.lastName,
                email = request.email,
                password = request.password
            };
        }
    }

    public class AccountResponse
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

        public AccountResponse() { }

        public AccountResponse(int id, string firstName, string lastName, string email)
        {
            Id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
        }
    }
}
