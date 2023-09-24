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

        [HttpPost]
        public ActionResult Post([FromBody] ProductModel productModel)
        {
            var existingProducutItem = AllProducts.Find(p => p.Name == productModel.Name);
            if (existingProducutItem != null)
            {
                return Conflict("Cannot create the product because it already exists.");
            }
            else
            {
                AllProducts.Add(productModel);
                var resourceUrl = Request.Path.ToString() + '/' + productModel.Name;
                return Created(resourceUrl, productModel);
            }
        }

        [HttpGet]
        [Route("{name}")]
        public ActionResult<ProductModel> Get(string name)
        {
            var productItem = AllProducts.Find(p => p.Name == name);
            return productItem == null ? NotFound() : Ok(productItem);
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

        [HttpPut]
        public ActionResult Update([FromBody] ProductModel productModel)
        {
            var existingProducutItem = AllProducts.Find(p => p.Name == productModel.Name);
            if (existingProducutItem == null)
            {
                return BadRequest("Cannot update a nont existing product.");
            }
            else
            {
                existingProducutItem.Name = productModel.Name;
                existingProducutItem.Price = productModel.Price;
                existingProducutItem.Stock = productModel.Stock;
                return Ok();
            }
        }

        [HttpDelete]
        [Route("{name}")]
        public ActionResult Delete(string name)
        {
            var productItem = AllProducts.Find(x => x.Name == name);
            if (productItem == null)
            {
                return NotFound();
            }
            else
            {
                AllProducts.Remove(productItem);
                return NoContent();
            }
        }

    }
}