using Microsoft.AspNetCore.Mvc;
using SimpleInventoryManagement.Model;
using System.Collections.Generic;

namespace SimpleInventoryManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryManagementController : ControllerBase
    {
        private static List<ProductModel> AllProducts = new List<ProductModel> {
            new ProductModel("Product A", 100, 5),
            new ProductModel("Product B", 200, 3),
            new ProductModel("Product C", 50, 10)
        };

        private readonly ILogger<InventoryManagementController> _logger;

        public InventoryManagementController(ILogger<InventoryManagementController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{sortKey},{ascending:bool}")]
        public ActionResult<List<ProductModel>> Get(string sortKey = "name", bool ascending = true)
        {
            var products = from p in AllProducts
                           select p;
            
            switch (sortKey)
            {
                case "name":
                    products = ascending ? products.OrderBy(p => p.Name) : (IEnumerable<ProductModel>)products.OrderByDescending(p => p.Name);
                    break;
                case "price":
                    products = ascending ? products.OrderBy(p => p.Price) : (IEnumerable<ProductModel>)products.OrderByDescending(p => p.Price);
                    break;
                case "stock":
                    products = ascending ? products.OrderBy(p => p.Stock) : (IEnumerable<ProductModel>)products.OrderByDescending(p => p.Stock);
                    break;
                default:
                    break;
            }
           
            return Ok(products);
        }
        
    }
}