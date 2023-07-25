using Confectionery.Data;
using Confectionery.Helpers;
using Confectionery.Models;
using Microsoft.AspNetCore.Mvc;

namespace Confectionery.Controllers
{
    public class AccountController:Controller
    {
        private readonly IUserHelper _userHelper;
        //private readonly DataContext _context;
        //private readonly ICombosHelper _combosHelper;
        //private readonly IBlobHelper _blobHelper;
        //private readonly IMailHelper _mailHelper;

        public AccountController(IUserHelper userHelper /*DataContext context*//*, ICombosHelper combosHelper, IBlobHelper blobHelper, IMailHelper mailHelper*/)
        {
            _userHelper = userHelper;
            //_context = context;
            //_combosHelper = combosHelper;
            //_blobHelper = blobHelper;
            //_mailHelper = mailHelper;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                //if (result.IsLockedOut)
                //{
                //    ModelState.AddModelError(string.Empty, "Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
                //}
                //else if (result.IsNotAllowed)
                //{
                //    ModelState.AddModelError(string.Empty, "El usuario no ha sido habilitado, debes de seguir" +
                //        " las instrucciones del correo enviado para poder habilitarte en el sistema.");
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                //}
            }
            ModelState.AddModelError(string.Empty, "El usuario no ha sido habilitado, debes de seguir" +
                        " las instrucciones del correo enviado para poder habilitarte en el sistema.");
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
		public IActionResult NotAuthorized()
		{
			return View();
		}
	}
}
