using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using los_api.Data;
using los_api.Services;

namespace los_api.Controllers
{
  public abstract class BaseController : Controller
  {
    protected readonly StoreContext _context;
    protected readonly ProductRepository _productRepository;
    protected readonly StockRepository _stockRepository;

    public BaseController(StoreContext context)
    {
      _context = context;
      _productRepository = new ProductRepository(context);
      _stockRepository = new StockRepository(context);
    }
  }
}