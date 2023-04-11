using Data.Repositories.Base;

namespace Service.Base;

public abstract class BaseServices<TEntity> : IBaseServices where TEntity: class
{
    protected readonly IUnitOfWork _unitOfWork;

    public BaseServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}