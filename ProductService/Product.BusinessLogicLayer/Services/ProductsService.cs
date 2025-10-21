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
      
            //string routingKey = "product.all.retrieved";
            //var message1 = new
            //{
            //    products = products,
            //};
            //var message = JsonConvert.SerializeObject(message1);

            //_rabbitMQPublisher.Publish<object>(routingKey, message);
            return products;

        }
        public async Task<List<Product>> GetProductsByListOfSkus(List<string> skus)
        {
            var products = await _productsRepo.GetProductsByListOfSkus(skus);
      
            return products;

        }
    }
}
