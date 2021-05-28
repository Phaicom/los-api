using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
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
  public class ProductsController : BaseController
  {

    public ProductsController(StoreContext context) : base(context)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
      try
      {
        return Ok(await _context.Product.ToListAsync());
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody][Bind("Id,Name,ImageUrl,Price")] Product product)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(product.Name))
          return BadRequest($"{nameof(product.Name)} parameter should not be empty");
        if (string.IsNullOrWhiteSpace(product.ImageUrl))
          return BadRequest($"{nameof(product.ImageUrl)} parameter should not be empty");

        if (ModelState.IsValid)
        {
          product.Id = Guid.NewGuid();
          _context.Product.Add(product);
          await _context.SaveChangesAsync();
          return Ok(product);
        }
        else
        {
          return BadRequest();
        }
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(Guid id, [FromBody][Bind("Id,Name,ImageUrl,Price")] Product product)
    {
      try
      {
        if (ModelState.IsValid)
        {
          if (string.IsNullOrWhiteSpace(product.Name))
            return BadRequest($"{nameof(product.Name)} parameter should not be empty");
          if (string.IsNullOrWhiteSpace(product.ImageUrl))
            return BadRequest($"{nameof(product.ImageUrl)} parameter should not be empty");

          if (ProductExists(id))
          {
            product.Id = id;
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product);
          }
          else
          {
            return NotFound();
          }
        }
        else
        {
          return BadRequest();
        }
      }
      catch (DbUpdateConcurrencyException)
      {
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
      }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid? id)
    {
      try
      {
        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
          return NotFound("Product with Id = " + id.ToString() + " not found!");
        }
        else
        {
          var stock = await _context.Stock.FirstOrDefaultAsync(s => s.ProductId == product.Id);
          if (stock != null)
          {
            _context.Stock.Remove(stock);
          }
          _context.Product.Remove(product);
          await _context.SaveChangesAsync();
          return Ok(await _context.Product.ToListAsync());
        }
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }
  }
}