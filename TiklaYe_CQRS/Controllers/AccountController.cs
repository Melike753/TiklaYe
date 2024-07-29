using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.Controllers
{

    // Kullanıcı kayıt (register) işlemleri
    public class AccountController : Controller
    {
        private readonly RegisterUserCommandHandler _registerUserCommandHandler;
        private readonly UserExistsQueryHandler _userExistsQueryHandler;

        public AccountController(RegisterUserCommandHandler registerUserCommandHandler, UserExistsQueryHandler userExistsQueryHandler)
        {
            _registerUserCommandHandler = registerUserCommandHandler;
            _userExistsQueryHandler = userExistsQueryHandler;
        }

        [HttpGet]
        // Kullanıcı kayıt formunu görüntülemek için.
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        // Kullanıcı kayıt işlemini gerçekleştirir.
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                var userExistsQuery = new UserExistsQuery
                {
                    Username = model.Username,
                    Email = model.Email
                };

                //  Kullanıcının var olup olmadığını kontrol eder. 
                if (await _userExistsQueryHandler.Handle(userExistsQuery))
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya e-posta zaten kullanılıyor.");
                    return View(model);
                }

                // Yeni bir kullanıcı oluşturmak için.
                var registerUserCommand = new RegisterUserCommand
                {
                    Name = model.Name,
                    Username = model.Username,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Address = model.Address,
                    PostCode = model.PostCode,
                    Password = model.Password
                };

                // Kullanıcı kayıt işlemini gerçekleştirir.
                await _registerUserCommandHandler.Handle(registerUserCommand);

                return RedirectToAction("Login", "Login");
            }

            return View(model);
        }
    }
}   