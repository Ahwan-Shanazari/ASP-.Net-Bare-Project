using Data.Repositories.Base;

namespace Service.Base;

public class BaseServices<TEntity> where TEntity: class
{
    private readonly BaseRepository<TEntity> _repository;

    public BaseServices(BaseRepository<TEntity> repository)
    {
        _repository = repository;
    }
}