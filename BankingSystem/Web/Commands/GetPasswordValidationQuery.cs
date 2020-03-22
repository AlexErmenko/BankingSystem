using System;

using Infrastructure.Identity;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Web.Commands
{
  public class GetPasswordValidationQuery : IRequest<IdentityResult>
  {
    public ApplicationUser? User { get; set; }
    public string Password { get; set; }

    public GetPasswordValidationQuery(ApplicationUser? user, string password)
    {
      User = user;
      Password = password;
    }
  }
}
