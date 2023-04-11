using Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Service.Base;

namespace Service;

public class UserServices : BaseServices<IdentityUser<long>>, IUserServices
{
    private readonly UserManager<IdentityUser<long>> _userManager;
    private readonly SignInManager<IdentityUser<long>> _signInManager;

    public UserServices(IBaseRepository<IdentityUser<long>> repository, UserManager<IdentityUser<long>> userManager,
        IUnitOfWork unitOfWork,SignInManager<IdentityUser<long>> signInManager) :
        base(repository, unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> CreateAccount(IdentityUser<long> user, string pass)
    {
        var result = await _userManager.CreateAsync(user, pass);
        
        if (result.Succeeded)
            return true;
        
        return false;
    }

    public async Task DeleteAccount(IdentityUser<long> user)
    {
        _repository.Delete(user);

        await _unitOfWork.CommitChangesAsync();
    }

    public async Task<bool> Login(string userName , string pass)
    {
        var user = await _repository.Read(user => user.UserName == userName);
        var result = await _signInManager.PasswordSignInAsync(user, pass , true , false);
        return result.Succeeded;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}