using Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories;

public interface IUserRepository :IBaseRepository<IdentityUser<long>>
{
}