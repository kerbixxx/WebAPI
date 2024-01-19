using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SimbirSoft.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepo;

        public AccountsController(IAccountService accountService, IAccountRepository accountRepo)
        {
            _accountService = accountService;
            _accountRepo = accountRepo;
        }

        //GET - Account
        //[Authorize]
        [SwaggerResponse(200, Type = typeof(AccountResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Аккаунт с таким accountId не найден")]
        [HttpGet("{accountId}")]
        public ActionResult GetAccount(int accountId)
        {
            if (accountId <= 0) return BadRequest();
            var obj = _accountRepo.Get(accountId);
            if (obj == null) return NotFound();
            return Ok(new JsonResult((AccountResponse)obj));
        }

        //PUT - Account
        //[Authorize]
        [HttpPut("{accountId}")]
        [SwaggerResponse(200, Type = typeof(AccountResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(403, Type = typeof(ProblemDetails), Description = "Аккаунт с таким accountId не найден")]
        [SwaggerResponse(409, Type = typeof(ProblemDetails), Description = "Аккаунт с таким email уже существует")]
        public ActionResult UpdateAccount(AccountRequest request, int accountId)
        {
            if(accountId <= 0) return BadRequest();
            if (!_accountService.IsValid(request)) return BadRequest();
            if (_accountRepo.FirstOrDefault(x => x.email == request.email) != null) return Conflict();

            //TODO : 403 Обновление не своего аккаунта, аккаунт не найден

            Account obj = (Account)request;
            obj.Id = accountId;
            _accountRepo.Update(obj);
            _accountRepo.Save();
            return Ok(new JsonResult((AccountResponse)obj));

        }

        //DELETE - Account
        [HttpDelete("{accountId}")]
        [SwaggerResponse(200, Type = typeof(AccountResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(403, Type = typeof(ProblemDetails), Description = "Аккаунт с таким accountId не найден")]
        public ActionResult DeleteAccount(int accountId)
        {
            if (accountId == 0) return BadRequest();
            //TODO: АККАУНТ СВЯЗАН С ЖИВОТНЫМ
            try
            {
                var obj = _accountRepo.Get(accountId);
                _accountRepo.Delete(obj);
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
            }
            return Ok(new JsonResult(new AccountResponse()));
        }

        //TO DO
        //GET - SearchAccounts
        //[Authorize]
        [HttpGet]
        [Route("search")]
        [SwaggerResponse(200, Type = typeof(AccountResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
        public ActionResult SearchAccounts([FromQuery] string firstName, [FromQuery] string lastName, [FromQuery] string email, [FromQuery] int from, [FromQuery] int size)
        {
            if (size <= 0 || from <= 0) return BadRequest();
            var accounts = _accountRepo.GetAll()
                .Where(a => a.firstName.Contains(firstName))
                .Where(a => a.lastName.Contains(lastName))
                .Where(a => a.email.Contains(email));
            List<AccountResponse> responses = new();
            foreach (var account in accounts)
            {
                responses.Add(new AccountResponse(account.Id, account.firstName, account.lastName, account.email));
            }
            return Ok(new JsonResult(_accountService.Sort(responses)));
        }
    }
}
