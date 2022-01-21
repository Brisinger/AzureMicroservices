using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Products.Interfaces;

namespace ECommerce.API.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider productsProvider;

        public ProductsController(IProductsProvider productsProvider)
        {
            this.productsProvider = productsProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var (IsSuccess, products, _) = await productsProvider.GetProductsAsync();
            if (IsSuccess)
                return Ok(products);
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsyc(int id)
        {
            var (IsSuccess, product, _) = await productsProvider.GetProductAsync(id);
            if (IsSuccess)
                return Ok(product);
            else
            {
                return NotFound();
            }
        }
    }
}