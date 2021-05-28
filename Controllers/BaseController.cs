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
  public abstract class BaseController : Controller
  {
    protected readonly StoreContext _context;

    public BaseController(StoreContext context)
    {
      _context = context;
    }

    protected bool ProductExists(Guid id)
    {
      return _context.Product.Any(e => e.Id == id);
    }

    protected bool StockExists(Guid id)
    {
      return _context.Stock.Any(e => e.Id == id);
    }

    protected bool StockExistsByProduct(Guid id)
    {
      return _context.Stock.Any(e => e.ProductId == id);
    }

  }
}