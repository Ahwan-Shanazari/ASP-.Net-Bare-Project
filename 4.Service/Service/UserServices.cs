using Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Service.Base;

namespace Service;

public class UserServices : BaseServices<IdentityUser<long>>
{
    private readonly UserManager<IdentityUser<long>> _userManager;

    public UserServices(BaseRepository<IdentityUser<long>> repository, UserManager<IdentityUser<long>> userManager) :
        base(repository)
    {
        _userManager = userManager;
    }
}