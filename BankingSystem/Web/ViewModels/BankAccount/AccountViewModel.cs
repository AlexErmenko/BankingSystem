using System;
using System.Collections.Generic;

namespace Web.ViewModels.BankAccount
{
  public class AccountViewModel
  {
    public int? IdClient { get; set; }
    public IEnumerable<ApplicationCore.Entity.BankAccount> BankAccounts { get; set; }
  }
}
