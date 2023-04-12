using Microsoft.AspNetCore.Identity;

namespace Service.Interfaces;

public interface IUserServices
{
    Task<bool> CreateAccount(IdentityUser<long> user, string pass);
    Task DeleteAccount(IdentityUser<long> user);
    Task<bool> Login(string userName , string pass);
    Task Logout();
    Task<IdentityUser<long>> FindUser(long id);
}