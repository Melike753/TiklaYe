using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.QueryHandlers;

namespace TiklaYe_CQRS.Controllers
{
    public class ProfileController : Controller
    {
        private readonly GetUserProfileQueryHandler _getUserProfileQueryHandler;
        private readonly UpdateProfileCommandHandler _updateProfileHandler;
        private readonly GetUserPurchaseHistoryQueryHandler _getUserPurchaseHistoryHandler;
        private readonly GetUserByUsernameQueryHandler _getUserByUsernameHandler;

        public ProfileController(UpdateProfileCommandHandler updateProfileHandler, GetUserPurchaseHistoryQueryHandler getUserPurchaseHistoryHandler,
           GetUserByUsernameQueryHandler getUserByUsernameHandler, GetUserProfileQueryHandler getUserProfileQueryHandler)
        {
            _updateProfileHandler = updateProfileHandler;
            _getUserPurchaseHistoryHandler = getUserPurchaseHistoryHandler;
            _getUserByUsernameHandler = getUserByUsernameHandler;
            _getUserProfileQueryHandler = getUserProfileQueryHandler;
        }

        // Kullanıcı profilini görüntüleme
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var query = new GetUserProfileQuery { Username = User.Identity.Name };
            var user = await _getUserProfileQueryHandler.Handle(query);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                Address = user.Address,
                Password = string.Empty // Şifreyi göstermiyoruz
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }


            var userId = (await _getUserProfileQueryHandler.Handle(new GetUserProfileQuery { Username = User.Identity.Name }))?.UserId;

            if (userId == null)
            {
                return NotFound();
            }

            var command = new UpdateProfileCommand
            {
                Username = model.Username,
                Email = model.Email,
                Mobile = model.Mobile,
                Address = model.Address,
                Password = model.Password
            };

            try
            {
                await _updateProfileHandler.Handle(command);
                TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", model);
            }
        }

        public async Task<IActionResult> PurchaseHistory()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = new GetUserByUsernameQuery { Username = User.Identity.Name };
            var user = await _getUserByUsernameHandler.Handle(query);
            if (user == null)
            {
                return NotFound();
            }

            var purchaseHistoryQuery = new GetUserPurchaseHistoryQuery { UserId = user.UserId };
            var purchases = await _getUserPurchaseHistoryHandler.Handle(purchaseHistoryQuery);

            return View(purchases);
        }
    }
}