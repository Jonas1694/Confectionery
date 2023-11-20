﻿using Confectionery.Data.Entities;
using Confectionery.Models;
using Microsoft.AspNetCore.Identity;

namespace Confectionery.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);
        //Task<User> GetUserAsync(Guid userId); // sobrecarga de metodos

        Task<IdentityResult> AddUserAsync(User user, string password);
		//Task<User> AddUserAsync(AddUserViewModel model);
		Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsyng(LoginViewModel model);
        Task LogoutAsync();
        //      Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        //      Task<IdentityResult> UpdateUserAsync(User user);
        //Task<string> GenerateEmailConfirmationTokenAsync(User user);
        //Task<IdentityResult> ConfirmEmailAsync(User user, string token);
    }
}
