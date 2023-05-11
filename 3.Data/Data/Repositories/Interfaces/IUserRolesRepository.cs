namespace Data.Repositories.Interfaces;

public interface IUserRolesRepository
{
    List<long> GetUserRoleIds(long userId);
}