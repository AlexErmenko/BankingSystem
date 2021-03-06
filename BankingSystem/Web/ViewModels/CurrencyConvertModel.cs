﻿using System;

namespace Web.ViewModels
{
  public class CurrencyConvertModel
  {
    public int ToId { get; set; }
    public string To { get; set; }

    public int FromId { get; set; }
    public string From { get; set; }

    public decimal Amount { get; set; }
  }
}
