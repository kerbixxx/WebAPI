using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using SimbirSoft.Repositories;
using System.Linq;
using System.Security.Claims;

namespace SimbirSoft.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepo;
        private readonly IAnimalRepository _animalRepository;

        public AccountsController(IAccountService accountService, IAccountRepository accountRepo, IAnimalRepository animalRepository)
        {
            _accountService = accountService;
            _accountRepo = accountRepo;
            _animalRepository = animalRepository;
        }

        //GET - Account
        [Authorize]
        [SwaggerResponse(200, Type = typeof(AccountResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
        [SwaggerResponse(404, Type = typeof(ProblemDetails), Description = "Аккаунт с таким accountId не найден")]
        [HttpGet("{accountId}", Name = nameof(GetAccount))]
        public ActionResult GetAccount(int accountId)
        {
            if (accountId <= 0) return BadRequest();
            var obj = _accountRepo.Get(accountId);
            if (obj == null) return NotFound();
            return new JsonResult((AccountResponse)obj);
        }

        //PUT - Account
        [Authorize]
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

            //403 Обновление не своего аккаунта, аккаунт не найден
            var user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var email = user?.Value;
            if (email != request.email || _accountRepo.FirstOrDefault(x=>x.email == request.email) == null) return StatusCode(StatusCodes.Status403Forbidden);

            Account obj = (Account)request;
            obj.Id = accountId;
            _accountRepo.Update(obj);
            _accountRepo.Save();
            return new JsonResult((AccountResponse)obj);

        }

        //DELETE - Account
        [HttpDelete("{accountId}")]
        [SwaggerResponse(200, Type = typeof(AccountResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(403, Type = typeof(ProblemDetails), Description = "Аккаунт с таким accountId не найден")]
        public ActionResult DeleteAccount(int accountId)
        {
            if (accountId == 0) return BadRequest();
            if (_animalRepository.FirstOrDefault(a => a.chipperId == accountId) != null) return BadRequest();

            //403 Обновление не своего аккаунта, аккаунт не найден
            var requestedAccount = _accountRepo.Get(accountId);
            var user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var email = user?.Value;
            if (email != requestedAccount.email || _accountRepo.FirstOrDefault(x => x.email == requestedAccount.email) == null) return StatusCode(StatusCodes.Status403Forbidden);

            try
            {
                var obj = _accountRepo.Get(accountId);
                _accountRepo.Delete(obj);
            }
            catch (Exception ex)
            {
                return Forbid(ex.Message);
            }
            return new JsonResult(new AccountResponse());
        }

        [Authorize]
        [HttpGet]
        [Route("search")]
        [SwaggerResponse(200, Type = typeof(AccountResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(401, Type = typeof(ProblemDetails), Description = "Неверные авторизационные данные")]
        public ActionResult SearchAccounts(
        [FromQuery] string? firstName,
        [FromQuery] string? lastName,
        [FromQuery] string? email,
        [FromQuery] int? from,
        [FromQuery] int? size) // Установите значения по умолчанию для параметров from и size
        {
            if (size <= 0 || from < 0) return BadRequest();
            if (size == null) size = 10;
            if (from == null) from = 0;
            var accounts = _accountRepo.GetAll();

            // Проверяем, были ли предоставлены параметры поиска
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                accounts = accounts.Where(a => a.firstName.Contains(firstName)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                accounts = accounts.Where(a => a.lastName.Contains(lastName)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                accounts = accounts.Where(a => a.email.Contains(email)).ToList();
            }

            // Если нет ни одного параметра поиска, вернуть первые 10 аккаунтов
            if (string.IsNullOrWhiteSpace(firstName) &&
                string.IsNullOrWhiteSpace(lastName) &&
                string.IsNullOrWhiteSpace(email))
            {
                accounts = accounts.Take(10).ToList();
            }

            // Применяем пагинацию
            accounts = accounts.Skip((int)from).Take((int)size).ToList();

            var accountResponses = accounts.Select(a => new AccountResponse(a.Id, a.firstName, a.lastName, a.email)).ToList();

            return new JsonResult(accountResponses);
        }
    }
}
