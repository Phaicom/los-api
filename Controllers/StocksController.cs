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
        return Ok(await _stockRepository.GetAll());
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(Guid id)
    {
      try
      {
        var stock = await _stockRepository.GetById(id);
        if (stock != null)
        {
          return Ok(stock);
        }
        else
        {
          return NotFound();
        }
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
        if (ModelState.IsValid && _productRepository.isExists(stock.ProductId))
        {
          var existStock = await _stockRepository.GetByProductId(stock.ProductId);
          if (existStock == null)
          {
            return Ok(await _stockRepository.Create(stock));
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
        if (ModelState.IsValid && _productRepository.isExists(stock.ProductId))
        {
          if (_stockRepository.isExists(id))
          {
            var existStock = await _stockRepository.GetByProductId(stock.ProductId);
            if (existStock == null)
            {
              return Ok(await _stockRepository.Edit(id, stock));
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
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
      try
      {
        var stock = await _stockRepository.GetById(id);
        if (stock == null)
        {
          return NotFound("Stock with Id = " + id.ToString() + " not found!");
        }
        else
        {
          return Ok(await _stockRepository.Delete(stock));
        }
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }
  }
}