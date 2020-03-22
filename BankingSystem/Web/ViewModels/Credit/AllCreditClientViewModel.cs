using System;
using System.Collections.Generic;

namespace Web.ViewModels.Credit
{
  /// <summary>
  ///     Для получения всех кредитов пользователя
  /// </summary>
  public class AllCreditClientViewModel
  {
    public int? IdClient { get; set; }
    public IEnumerable<ApplicationCore.Entity.Credit> Credits { get; set; }
  }
}
