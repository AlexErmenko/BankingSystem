using System;
using System.ComponentModel.DataAnnotations;

using Infrastructure.Identity;

namespace Web.ViewModels.Admin
{
  public class UserViewModel
  {
    public ApplicationUser User { get; set; }

    [Required, Display(Name = "Имя пользователя")]
    public string UserName { get; set; }

    public string Id { get; set; }

    //[Required]
    [Display(Name = "Email"), EmailAddress]
    public string Email { get; set; }

    //[Required]
    [Display(Name = "Номер телефона"), Phone]
    public string PhoneNumber { get; set; }

    [Required, DataType(dataType: DataType.Password), Display(Name = "Пароль")]
    public string Password { get; set; }

    [Required, Compare(otherProperty: "Password", ErrorMessage = "Пароли не совпадают!"), DataType(dataType: DataType.Password), Display(Name = "Подтвердить пароль")]
    public string PasswordConfirm { get; set; }
  }
}
