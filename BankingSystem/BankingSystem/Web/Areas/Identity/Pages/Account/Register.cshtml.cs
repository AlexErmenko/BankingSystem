using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Infrastructure.Identity;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly IEmailSender _emailSender;
		private readonly ILogger<RegisterModel> _logger;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<RegisterModel> logger, IEmailSender emailSender)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
		}

		public class InputModel
		{
			[Required, EmailAddress, Display(Name = "Email")]
			public string Email { get; set; }

			[Required, StringLength(maximumLength: 100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6), DataType(dataType: DataType.Password), Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(dataType: DataType.Password), Display(Name = "Confirm password"), Compare(otherProperty: "Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			ExternalLogins = ( await _signInManager.GetExternalAuthenticationSchemesAsync() ).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content(contentPath: "~/");
			ExternalLogins = ( await _signInManager.GetExternalAuthenticationSchemesAsync() ).ToList();
			if(ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = Input.Email,
					Email = Input.Email
				};
				var result = await _userManager.CreateAsync(user: user, password: Input.Password);
				if(result.Succeeded)
				{
					_logger.LogInformation(message: "User created a new account with password.");

					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user: user);
					code = WebEncoders.Base64UrlEncode(input: Encoding.UTF8.GetBytes(s: code));
					var callbackUrl = Url.Page(pageName: "/Account/ConfirmEmail", pageHandler: null, values: new
					{
						area = "Identity",
						userId = user.Id,
						code
					}, protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(email: Input.Email, subject: "Confirm your email", htmlMessage: $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(value: callbackUrl)}'>clicking here</a>.");

					if(_userManager.Options.SignIn.RequireConfirmedAccount)
					{
						return RedirectToPage(pageName: "RegisterConfirmation", routeValues: new
						{
							email = Input.Email
						});
					}

					await _signInManager.SignInAsync(user: user, isPersistent: false);
					return LocalRedirect(localUrl: returnUrl);
				}

				foreach(var error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
