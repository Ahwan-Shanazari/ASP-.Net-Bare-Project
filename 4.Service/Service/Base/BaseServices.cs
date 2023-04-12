using Data.Repositories.Base;
using Service.Interfaces;

namespace Service.Base;

public abstract class BaseServices<TEntity> : IBaseServices where TEntity: class
{
    protected readonly IUnitOfWork _unitOfWork;

    public BaseServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}