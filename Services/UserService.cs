using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using WidgetService.Controllers;
using WidgetService.Data;
using WidgetService.Models;

namespace WidgetService.Services
{
    public class UserService
    {
       
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<(bool successful, string message, string? apiKey)> GenerateApiKey(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return (false, "User not found.", null);

                user.ApiKey = Guid.NewGuid().ToString();

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return (false, string.Join(", ", result.Errors.Select(e => e.Description)), null);
                }

                return (true, "API key generated successfully.", user.ApiKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate API key");
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool successful, string message)> SetUserDetails(ApplicationUser updatedUser)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(updatedUser.Id);
                if (user == null)
                    return (false, "User not found.");

                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.IsAdmin = updatedUser.IsAdmin;
                user.ApiKey = updatedUser.ApiKey;
                user.Email = updatedUser.Email;
                user.UserName = updatedUser.UserName;
                
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

                return (true, "User updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user");
                return (false, ex.Message);
            }
        }


        public async Task<ApplicationUser?> GetUserByLogin(LoginRequest login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                return null;

            var validPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            return validPassword ? user : null;
        }


        public async Task<(bool successful, string message)> CreateUser(ApplicationUser newUser, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(newUser, password);

                if (!result.Succeeded)
                    return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

                return (true, "User created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user");
                return (false, ex.Message);
            }
        }


        public async Task<bool> CheckAuthKey(string providedKey)
        {
            return await _userManager.Users.AnyAsync(u => u.ApiKey == providedKey);
        }

    }
}
