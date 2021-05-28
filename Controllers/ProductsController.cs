using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using los_api.Data;
using los_api.Models;
using los_api.Dto;
using Newtonsoft.Json;

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
        var products = await _productRepository.GetAll();
        var productsDto = new List<ProductDto>();
        foreach (Product product in products)
        {
          var productDto = JsonConvert.DeserializeObject<ProductDto>(JsonConvert.SerializeObject(product));
          var stock = await _stockRepository.GetByProductId(productDto.Id);
          if (stock != null)
          {
            productDto.stock = stock;
          }
          productsDto.Add(productDto);
        }
        return Ok(productsDto);
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
        var product = await _productRepository.GetById(id);
        if (product != null)
        {
          var productDto = JsonConvert.DeserializeObject<ProductDto>(JsonConvert.SerializeObject(product));
          var stock = await _stockRepository.GetByProductId(productDto.Id);
          if (stock != null)
          {
            productDto.stock = stock;
          }
          return Ok(productDto);
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
          return Ok(await _productRepository.Create(product));
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

          if (_productRepository.isExists(id))
          {
            return Ok(await _productRepository.Edit(id, product));
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
        var product = await _productRepository.GetById(id);
        if (product == null)
        {
          return NotFound("Product with Id = " + id.ToString() + " not found!");
        }
        else
        {
          var stock = await _stockRepository.GetByProductId(product.Id);
          if (stock != null)
          {
            await _stockRepository.Delete(stock);
          }
          return Ok(await _productRepository.Delete(product));
        }
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex);
      }
    }
  }
}