using System.Security.Claims;
using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories;

public class UserRepository:BaseRepository<IdentityUser<long>>, IUserRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }
    
}