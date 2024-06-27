using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQProject.Models;
using RabbitMQProject.RabbitMQ;
using RabbitMQProject.Services;

namespace RabbitMQProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IRabitMQProducer _rabbitMQProducer;

        public ProductController(IProductService _productService, IRabitMQProducer rabbitMQProducer)
        {
            _productService = _productService;
            _rabbitMQProducer = rabbitMQProducer;
        }
        [HttpGet("productlist")]
        public IEnumerable<Product> ProductList()
        {
            var productList = _productService.GetProductList();
            return productList;
        }
        [HttpGet("getproductbyid")]
        public Product GetProductById(int id)
        {
            return _productService.GetProductById(id);
        }

        [HttpPost("addproduct")]
        public Product AddProduct(Product product)
        {
            var ProductData = _productService.AddProduct(product);
            //eklenen ürün verilerini kuyruğa gönderir ve tüketici bu verileri kuyruktan dinler
            _rabbitMQProducer.SendProductMessage(ProductData);
            return ProductData;
        }
        [HttpPut("updateproduct")]
        public Product UpdateProduct(Product product)
        {
            return _productService.UpdateProduct(product);
        }
        [HttpDelete("deleteproduct")]
        public bool DeleteProduct(int Id)
        {
            return _productService.DeleteProduct(Id);
        }

    }
}
