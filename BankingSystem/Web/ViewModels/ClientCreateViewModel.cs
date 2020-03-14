using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
	public class ClientCreateViewModel
	{
		[Required]
		[Display(Name = "Логин")]
		public string Login { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Пароли не совпадают!")]
		[DataType(DataType.Password)]
		[Display(Name = "Подтвердить пароль")]
		public string PasswordConfirm { get; set; }

		[Required]
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Адрес")]
		public string Address { get; set; }

		[Required]
		[Display(Name = "Телефонный Номер")]
		public string TelNumber { get; set; }

		[Required]
		[Display(Name = "Тип Субъекта")]
		public bool IsPhysicalPerson { get; set; }
	}
}
