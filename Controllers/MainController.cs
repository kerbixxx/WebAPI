using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SimbirSoft.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepo;

        public MainController(IAccountService accountService, IAccountRepository accountRepo)
        {
            _accountService = accountService;
            _accountRepo = accountRepo;
        }
        //POST - Registration
        [HttpPost]
        [Route("registration")]
        [SwaggerResponse(201, Type = typeof(AccountResponse), Description = "Запрос успешно выполнен")]
        [SwaggerResponse(400, Type = typeof(ProblemDetails), Description = "Ошибка валидации")]
        [SwaggerResponse(409, Type = typeof(ProblemDetails), Description = "Аккаунт с таким email уже существует")]
        public ActionResult Registration(AccountRequest request)
        {
            if (!_accountService.IsValid(request)) return BadRequest();
            if (_accountRepo.FirstOrDefault(x => x.email == request.email) != null) return Conflict();

            //TODO:Запрос от авторизованного аккаунта с ошибкой 403
            var obj = (Account)request;
            _accountRepo.Add(obj);
            _accountRepo.Save();
            return new JsonResult((AccountResponse)obj);
        }
    }
}
