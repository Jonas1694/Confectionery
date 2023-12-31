﻿using Confectionery.Common;
using Confectionery.Data;
using Confectionery.Data.Entities;
using Confectionery.Enums;
using Confectionery.Helpers;
using Confectionery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Confectionery.Controllers
{
    public class AccountController:Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
		private readonly IMailHelper _mailHelper;

		public AccountController(IUserHelper userHelper,DataContext context, IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _context = context;
			_mailHelper = mailHelper;
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
                
                SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "El usuario no ha sido habilitado, debes de seguir" +
                        " las instrucciones del correo enviado para poder habilitarte en el sistema.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                }
            }
            //ModelState.AddModelError(string.Empty, "El usuario no ha sido habilitado, debes de seguir" +
            //            " las instrucciones del correo enviado para poder habilitarte en el sistema.");
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
		public async Task<IActionResult> Register()
		{
			AddUserViewModel model = new()
			{
				Id = Guid.Empty.ToString(),
				UserType = UserType.Facturador,
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(AddUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				Guid imageId = Guid.Empty;

				//if (model.ImageFile != null)
				//{
				//	imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
				//}
				//model.ImageId = imageId;

				User user = await _userHelper.AddUserAsync(model);
				if (user == null)
				{
					ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
					
					return View(model);
				}

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
					token = myToken

                }, protocol: HttpContext.Request.Scheme);
				Response response = _mailHelper.SendMail(
				   $"{model.FirstName} {model.LastName}",
				   model.Username,
				   "Confectionery - Confirmación de Email",
				   $"<h1>Confectionery - Confirmación de Email</h1>" +
					   $"Para habilitar el usuario por favor hacer click en el siguiente link:, " +
					   $"<hr/><br/><p><a href = \"{tokenLink}\">Confirmar Email</a></p>");
				if (response.IsSuccess)
				{
					ViewBag.Message = "Las instrucciones para habilitar el usuario han sido enviadas al correo.";
					return View(model);
				}

				ModelState.AddModelError(string.Empty, response.Message);

			}

			return View(model);
		}
		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
			{
				return NotFound();
			}

			User user = await _userHelper.GetUserAsync(new Guid(userId));
			if (user == null)
			{
				return NotFound();
			}

			IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
			if (!result.Succeeded)
			{
				return NotFound();
			}

			return View();
		}
		public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new()
            {
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Id = user.Id,
                Document = user.Document
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;


                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.ImageId = imageId;
                user.Document = model.Document;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.OldPassword == model.NewPassword)
                {
                    ModelState.AddModelError(string.Empty, "Debes ingresar una contraseña diferente");
                    return View(model);
                }

                User? user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    IdentityResult? result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                }
            }

            return View(model);
        }
    }
}
