using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using los_api.Data;
using los_api.Models;

namespace los_api.Services
{
  public class ProductRepository
  {
    protected readonly StoreContext _context;
    public ProductRepository(StoreContext context)
    {
      _context = context;
    }

    public async Task<List<Product>> GetAll()
    {
      return await _context.Product.ToListAsync();
    }

    public async Task<Product> GetById(Guid id)
    {
      return await _context.Product.FindAsync(id);
    }

    public async Task<Product> Create(Product product)
    {
      product.Id = Guid.NewGuid();
      _context.Product.Add(product);
      await _context.SaveChangesAsync();
      return product;
    }

    public async Task<Product> Edit(Guid id, Product product)
    {
      product.Id = id;
      _context.Product.Update(product);
      await _context.SaveChangesAsync();
      return product;
    }

    public async Task<List<Product>> Delete(Product product)
    {
      _context.Product.Remove(product);
      await _context.SaveChangesAsync();
      return await _context.Product.ToListAsync();
    }

    public bool isExists(Guid id)
    {
      return _context.Product.Any(e => e.Id == id);
    }
  }
}