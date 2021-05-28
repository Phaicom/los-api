using System;

namespace los_api.Models
{
  public class Stock
  {
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Amount { get; set; }
  }
}