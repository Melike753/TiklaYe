using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.QueryHandlers;

namespace TiklaYe_CQRS.Controllers
{
    public class UserController : Controller
    {
        private readonly GetAllUsersHandler _getAllUsersHandler;
        private readonly DeleteUserHandler _deleteUserHandler;
        private readonly GetUserPurchaseHistoryHandler _getUserPurchaseHistoryHandler;
        private readonly GetUserByUsernameHandler _getUserByUsernameHandler;
        private readonly UpdateProfileHandler _updateProfileHandler;
        private readonly ApplicationDbContext _context;


        public UserController(
            GetAllUsersHandler getAllUsersHandler,
            DeleteUserHandler deleteUserHandler,
           GetUserPurchaseHistoryHandler getUserPurchaseHistoryHandler,
           GetUserByUsernameHandler getUserByUsernameHandler,
            UpdateProfileHandler updateProfileHandler,
             ApplicationDbContext context)

        {
            _getAllUsersHandler = getAllUsersHandler;
            _deleteUserHandler = deleteUserHandler;
            _getUserPurchaseHistoryHandler = getUserPurchaseHistoryHandler;
            _getUserByUsernameHandler = getUserByUsernameHandler;
            _updateProfileHandler = updateProfileHandler;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetAllUsersQuery();
            var users = await _getAllUsersHandler.Handle(query);
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUserCommand { UserId = id };
            var success = await _deleteUserHandler.Handle(command);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            var query = new GetUserByUsernameQuery { Username = User.Identity.Name };
            var user = await _getUserByUsernameHandler.Handle(query);
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
                Password = string.Empty
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            var command = new UpdateProfileCommand
            {
                Username = model.Username,
                Email = model.Email,
                Mobile = model.Mobile,
                Address = model.Address,
                Password = model.Password
            };

            var success = await _updateProfileHandler.Handle(command);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> PurchaseHistory()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
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