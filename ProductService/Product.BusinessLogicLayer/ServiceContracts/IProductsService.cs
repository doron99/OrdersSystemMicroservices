
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.ServiceContracts
{
    public interface IProductsService
    {
        Task<List<Product?>> GetAll();
    }
}
