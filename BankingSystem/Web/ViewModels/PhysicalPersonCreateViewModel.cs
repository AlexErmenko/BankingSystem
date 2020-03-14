using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
	public class PhysicalPersonCreateViewModel
	{
		[Required]
		[Display(Name = "Логин")]
		public string Login { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

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
		[Display(Name = "Серия Паспорта")]
		public string PassportSeries { get; set; }

		[Required]
		[Display(Name = "Номер Паспорта")]
		public string PassportNumber { get; set; }

		[Required]
		[Display(Name = "Идентификационный Номер")]
		public string IdentificationNumber { get; set; }

		[Required]
		[Display(Name = "Фамилия")]
		public string Surname { get; set; }

		[Required]
		[Display(Name = "Имя")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Отчество")]
		public string Patronymic { get; set; }
	}
}
