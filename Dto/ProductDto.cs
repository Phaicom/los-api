using System;
using los_api.Models;

namespace los_api.Dto
{
  public class ProductDto
  {
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public decimal Price { get; set; }
    public Stock stock { get; set; }
  }
}