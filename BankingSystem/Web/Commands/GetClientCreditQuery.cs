using System;

using ApplicationCore.Entity;

using MediatR;

namespace Web.Commands
{
  public class GetClientCreditQuery : IRequest<Credit>
  {
    public int? IdAccount { get; }
    public bool Status { get; }

    public GetClientCreditQuery(int? IdAccount, bool Status)
    {
      this.IdAccount = IdAccount;
      this.Status = Status;
    }
  }
}
