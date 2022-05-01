using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using TiendaOnline.Web.Data.Entities;
using TiendaOnline.Web.Models;

namespace TiendaOnline.Web.Interfaces
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<User> AddUserAsync(AddUserViewModel model, Guid imageId);

    }
}
