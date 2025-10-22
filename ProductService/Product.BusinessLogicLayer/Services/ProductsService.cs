using Newtonsoft.Json;
using BusinessLogicLayer.RabbitMQ;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.RepositoriesContracts;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepo;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public ProductsService(IProductsRepository productsRepo, IRabbitMQPublisher rabbitMQPublisher)
        {
            _productsRepo = productsRepo;
            _rabbitMQPublisher = rabbitMQPublisher;
        }
        public async Task<List<DataAccessLayer.Entities.Product?>> GetAll()
        {
            var products = await _productsRepo.GetAllAsync();
      
            return products;

        }
        public async Task<List<Product>> GetProductsByListOfSkus(List<string> skus)
        {
            var products = await _productsRepo.GetProductsByListOfSkus(skus);
      
            return products;

        }

        public async Task<List<StockTracking?>> GetStockTrackingList()
        {
            return await _productsRepo.GetStockTrackingListAsync();
        }
    }
}
