using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using los_api.Data;
using los_api.Models;

namespace los_api.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Product>> Index()
        {
            return await _context.Product.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] [Bind("Id,Name,ImageUrl,Price")] Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                return BadRequest($"{nameof(product.Name)} parameter should not be empty");
            if (string.IsNullOrWhiteSpace(product.ImageUrl))
                return BadRequest($"{nameof(product.ImageUrl)} parameter should not be empty");
            

            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid? id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // public ActionResult<List<Product>> Index()
        // {
        //     return _products; //returns all users.
        // }

        // [HttpGet("{guid}")]
        // public ActionResult<Product> GetByGuid(Guid id)
        // {
        //     return Ok(_products.FirstOrDefault(p => p.Id == id));
        // }

        // [HttpPost]
        // public ActionResult<Product> Insert([FromBody] Product product)
        // {
        //     _products.Add(product);
        //     return product;
        // }
     
    }
}