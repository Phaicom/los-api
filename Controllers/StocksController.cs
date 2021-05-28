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
  public class StocksController : BaseController
  {
    public StocksController(StoreContext context) : base(context)
    {

    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
      try
      {
        return Ok(await _context.Stock.ToListAsync());
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody][Bind("Id,ProductId,Amount")] Stock stock)
    {
      try
      {
        if (ModelState.IsValid && ProductExists(stock.ProductId))
        {
          var existStock = await _context.Stock.FirstOrDefaultAsync(s => s.ProductId == stock.ProductId);

          if (existStock == null)
          {

            stock.Id = Guid.NewGuid();
            _context.Stock.Add(stock);
            await _context.SaveChangesAsync();
            return Ok(stock);
          }
          else
          {
            return Conflict();
          }
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
    public async Task<IActionResult> Edit(Guid id, [FromBody][Bind("Id,ProductId,Amount")] Stock stock)
    {
      try
      {
        if (ModelState.IsValid && ProductExists(stock.ProductId))
        {
          if (StockExists(id))
          {
            var existStock = await _context.Stock.FirstOrDefaultAsync(s => s.ProductId == stock.ProductId);
            if (existStock == null)
            {

              stock.Id = id;
              _context.Stock.Update(stock);
              await _context.SaveChangesAsync();
              return Ok(stock);
            }
            else
            {
              return Conflict();
            }
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
        var stock = await _context.Stock.FindAsync(id);
        if (stock == null)
        {
          return NotFound("Stock with Id = " + id.ToString() + " not found!");
        }
        else
        {
          _context.Stock.Remove(stock);
          await _context.SaveChangesAsync();
          return Ok(await _context.Stock.ToListAsync());
        }
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }


  }
}