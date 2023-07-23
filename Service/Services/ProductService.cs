using Core.Dtos;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;

namespace Service.Services;

public class ProductService : ServiceGeneric<Product,ProductDto>, IProductService
{
    public ProductService(IUnitOfWork unitOfWork, IGenericRepository<Product> genericRepository) : base(unitOfWork, genericRepository)
    {
    }
}