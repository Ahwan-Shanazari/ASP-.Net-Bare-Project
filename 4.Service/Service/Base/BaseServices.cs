using Data.Repositories.Base;

namespace Service.Base;

public abstract class BaseServices<TEntity> : IBaseServices where TEntity: class
{
    protected readonly IBaseRepository<TEntity> _repository;
    protected readonly IUnitOfWork _unitOfWork;

    public BaseServices(IBaseRepository<TEntity> repository,IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
}