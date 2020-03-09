using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
	public class BankAccountController : Controller
	{
		private readonly IBankAccountRepository           _bankAccountRepository;
		private readonly IAsyncRepository<LegalPerson>    _legalPersonRepository;
		private readonly IAsyncRepository<PhysicalPerson> _physicalPersonRepository;


		public BankAccountController(IAsyncRepository<PhysicalPerson> physicalPersonRepo,
									 IAsyncRepository<LegalPerson>    legalPersonRepo
								   , IBankAccountRepository           bankAccountRepo)
		{
			_bankAccountRepository    = bankAccountRepo;
			_physicalPersonRepository = physicalPersonRepo;
			_legalPersonRepository    = legalPersonRepo;
		}

		/// <summary>
		///     Добавление нового счета существующего клиента.
		///     Возвращает представление и модель с физическими и юридическими лицами, для вывода частичной информации.
		/// </summary>
		/// <param name="idClient"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> CreateClientAccountForm(int idClient)
		{
			var physicalPerson = await _physicalPersonRepository.GetById(idClient);
			var legalPerson    = await _legalPersonRepository.GetById(idClient);

			return View(new CreateClientAccountViewModel
			{
				PhysicalPerson = physicalPerson, LegalPerson = legalPerson, ReturnUrl = "/"
			});
		}


		public void GetAccounts()
		{
			
		}

		/// <summary>
		///     Создание счета на основе заполненной формы.
		/// </summary>
		/// <param name="createClientAccountViewModel"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult CreateClientAccountForm(CreateClientAccountViewModel createClientAccountViewModel)
		{
			if (ModelState.IsValid)
			{
				//сохранение счета
				var account = createClientAccountViewModel.Account;
				_bankAccountRepository.SaveAccount(account);
			}

			return View();
		}

		/// <summary>
		///     Закрытие счета клиента
		/// </summary>
		/// <param name="idAccount"></param>
		/// <returns></returns>
		public IActionResult BankAccountClose(int idAccount)
		{
			_bankAccountRepository.CloseAccount(idAccount);

			return View("Index");
		}

		//Перед тем, как удалить счет, нужно его закрыть методом BankAccountClose
		/// <summary>
		///     Удаление счета клиента
		/// </summary>
		/// <param name="idAccount"></param>
		/// <returns></returns>
		public IActionResult BankAccountDelete(int idAccount)
		{
			_bankAccountRepository.DeleteAccount(idAccount);

			return View("Index");
		}
	}
}