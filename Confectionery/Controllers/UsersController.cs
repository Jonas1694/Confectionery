using Azure;
using Confectionery.Data;
using Confectionery.Data.Entities;
using Confectionery.Enums;
using Confectionery.Helpers;
using Confectionery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Confectionery.Controllers
{
    //[Authorize("Admin")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
		private readonly IUserHelper _userHelper;

		public UsersController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
			_userHelper= userHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
		public async Task<IActionResult> Create()
		{
			AddUserViewModel model = new()
			{
				Id = Guid.Empty.ToString(),
				UserType = UserType.Admin,
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AddUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				Guid imageId = Guid.Empty;


				User user = await _userHelper.AddUserAsync(model);
				if (user == null)
				{
					ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
					return View(model);
				}
				LoginViewModel loginViewModel = new()
				{
					Password = model.Password,
					RememberMe = false,
					Username = model.Username
				};
				var result2 = await _userHelper.LoginAsync(loginViewModel);
				if (result2.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
			}
		
			return View(model);
		}
	}
}
