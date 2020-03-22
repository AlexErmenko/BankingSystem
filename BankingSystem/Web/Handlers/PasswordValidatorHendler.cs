using System;
using System.Threading;
using System.Threading.Tasks;

using Infrastructure.Identity;

using MediatR;

using Microsoft.AspNetCore.Identity;

using Web.Commands;

namespace Web.Handlers
{
  public class PasswordValidatorHendler : IRequestHandler<GetPasswordValidationQuery, IdentityResult>
  {
    private readonly UserManager<ApplicationUser> UserManager;

    public PasswordValidatorHendler(UserManager<ApplicationUser> userManager) => UserManager = userManager;

    public async Task<IdentityResult> Handle(GetPasswordValidationQuery request, CancellationToken cancellationToken)
    {
      var validator = new PasswordValidator<ApplicationUser>();
      IdentityResult result = await validator.ValidateAsync(manager: UserManager, user: request.User, password: request.Password);

      return result;
    }
  }
}
