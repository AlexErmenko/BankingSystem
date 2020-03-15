using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Web.Commands
{
	public class PasswordValidatorHendler : IRequestHandler<GetPasswordValidationQuery,IdentityResult>
	{
		private UserManager<ApplicationUser> UserManager;

		public PasswordValidatorHendler(UserManager<ApplicationUser> userManager)
		{
			UserManager = userManager;

		}
		
		public async Task<IdentityResult> Handle(GetPasswordValidationQuery request, CancellationToken cancellationToken)
		{
			var validator = new PasswordValidator<ApplicationUser>();
			var result    = await validator.ValidateAsync(UserManager, request.User, request.Password);

			return result;
		}
	}
}