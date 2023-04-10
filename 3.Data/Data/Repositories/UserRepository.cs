using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories;

public class UserRepository:BaseRepository<IdentityUser<long>>
{
    public UserRepository(DataContext context) : base(context)
    {
    }   
}