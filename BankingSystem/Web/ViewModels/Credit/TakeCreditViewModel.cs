using System;

namespace Web.ViewModels.Credit
{
  public class TakeCreditViewModel
  {
    public int IdAccount { get; set; }
    public ApplicationCore.Entity.Credit Credit { get; set; }
  }
}
