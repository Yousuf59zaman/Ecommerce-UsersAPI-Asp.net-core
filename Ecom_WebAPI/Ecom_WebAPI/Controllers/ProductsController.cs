using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Ecom_WebAPI.Models;

namespace Ecom_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        DAL_product _dal = new DAL_product();


        [HttpPost]
        [Route("add")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            bool result = _dal.AddProduct(product,connectionString);
            if (result)
            {
                return Ok(new Response<Product> { Success = true, Message = "Product added successfully", Data = product });
            }

            return BadRequest(new Response<Product> { Success = false, Message = "Adding product failed" });
        }

        [HttpGet]
        [Route("{productId}")]
        public IActionResult GetProductById(int productId)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            Product product = _dal.GetProductById(productId, connectionString);
            if (product != null)
            {
                return Ok(new Response<Product> { Success = true, Message = "Product found", Data = product });
            }

            return NotFound(new Response<Product> { Success = false, Message = "Product not found" });
        }


        [HttpPut]
        [Route("update")]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            bool result = _dal.UpdateProduct(product,connectionString);
            if (result)
            {
                return Ok(new Response<Product> { Success = true, Message = "Product updated successfully", Data = product });
            }

            return BadRequest(new Response<Product> { Success = false, Message = "Updating product failed" });
        }



    }
}
