using Microsoft.AspNetCore.Identity;
using shop.Web.Data.Entities;
using shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Helpers
{
	public interface IUserHelper
	{
		// recordar que el asyn es parasaber si el metodo es asynctrono
		//le paso el email y me devuelve el usuario completo
		Task<User> GetUserByEmailAsync(string email);

		//aqui me devuelve si pudo hacer el logueo o si no pudo me muestra porque no pudo
		Task<IdentityResult> AddUserAsync(User user, string password);
		Task<SignInResult> LoginAsync(LoginViewModel model);

		Task LogoutAsync();

		Task<IdentityResult> UpdateUserAsync(User user);

		Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

		Task<SignInResult> ValidatePasswordAsync(User user, string password);
        Task CheckRoleAsync(string roleName);
		Task AddUserToRoleAsync(User user, string roleName);
		Task<bool> IsUserInRoleAsync(User user, string roleName);

		//aui estamos generando el tken para confirmar 
		Task<string> GenerateEmailConfirmationTokenAsync(User user);

		Task<IdentityResult> ConfirmEmailAsync(User user, string token);

		Task<User> GetUserByIdAsync(string userId);

		Task<string> GeneratePasswordResetTokenAsync(User user);

		Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);


	}

}
