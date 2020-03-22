using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Identity.Pages.Account.Manage
{
  public class IndexModel : PageModel
  {
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public string Username { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    public class InputModel
    {
      [Phone, Display(Name = "Phone number")]
      public string PhoneNumber { get; set; }
    }

    private async Task LoadAsync(ApplicationUser user)
    {
      string userName = await _userManager.GetUserNameAsync(user: user);
      string phoneNumber = await _userManager.GetPhoneNumberAsync(user: user);

      Username = userName;

      Input = new InputModel
      {
        PhoneNumber = phoneNumber
      };
    }

    public async Task<IActionResult> OnGetAsync()
    {
      ApplicationUser user = await _userManager.GetUserAsync(principal: User);
      if(user == null)
        return NotFound(value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

      await LoadAsync(user: user);
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      ApplicationUser user = await _userManager.GetUserAsync(principal: User);
      if(user == null)
        return NotFound(value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

      if(!ModelState.IsValid)
      {
        await LoadAsync(user: user);
        return Page();
      }

      string phoneNumber = await _userManager.GetPhoneNumberAsync(user: user);
      if(Input.PhoneNumber != phoneNumber)
      {
        IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user: user, phoneNumber: Input.PhoneNumber);
        if(!setPhoneResult.Succeeded)
        {
          string userId = await _userManager.GetUserIdAsync(user: user);
          throw new InvalidOperationException(message: $"Unexpected error occurred setting phone number for user with ID '{userId}'.");
        }
      }

      await _signInManager.RefreshSignInAsync(user: user);
      StatusMessage = "Your profile has been updated";
      return RedirectToPage();
    }
  }
}
