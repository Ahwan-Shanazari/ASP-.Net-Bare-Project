using Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories.Interfaces;

public interface IUserRepository :IBaseRepository<IdentityUser<long>>
{
}