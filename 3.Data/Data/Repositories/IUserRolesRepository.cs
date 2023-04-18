namespace Data.Repositories;

public interface IUserRolesRepository
{
    List<long> GetUserRoleIds(long userId);
}