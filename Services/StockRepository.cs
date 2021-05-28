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
  public class StockRepository
  {
    protected readonly StoreContext _context;
    public StockRepository(StoreContext context)
    {
      _context = context;
    }

    public async Task<List<Stock>> GetAll()
    {
      return await _context.Stock.ToListAsync();
    }

    public async Task<Stock> GetById(Guid id)
    {
      return await _context.Stock.FindAsync(id);
    }

    public async Task<Stock> GetByProductId(Guid id)
    {
      return await _context.Stock.FirstOrDefaultAsync(s => s.ProductId == id);
    }

    public async Task<Stock> Create(Stock stock)
    {
      stock.Id = Guid.NewGuid();
      _context.Stock.Add(stock);
      await _context.SaveChangesAsync();
      return stock;
    }

    public async Task<Stock> Edit(Guid id, Stock stock)
    {
      stock.Id = id;
      _context.Stock.Update(stock);
      await _context.SaveChangesAsync();
      return stock;
    }

    public async Task<List<Stock>> Delete(Stock stock)
    {
      _context.Stock.Remove(stock);
      await _context.SaveChangesAsync();
      return await _context.Stock.ToListAsync();
    }

    public bool isExists(Guid id)
    {
      return _context.Stock.Any(e => e.Id == id);
    }
  }
}