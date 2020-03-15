using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels;
using Web.ViewModels.BankAccount;

namespace Web.Controllers
{
	//[Authorize(Roles = AuthorizationConstants.Roles.MANAGER)]
	[Authorize(Roles = "Client, Manager")]
	public class BankAccountController : Controller
	{
		private readonly IBankAccountRepository _bankAccountRepository;
		private readonly IAsyncRepository<LegalPerson> _legalPersonRepository;
		private readonly IAsyncRepository<PhysicalPerson> _physicalPersonRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IAsyncRepository<Client> _clientRepository;

		public BankAccountController(IAsyncRepository<PhysicalPerson> physicalPersonRepo,
									 IAsyncRepository<LegalPerson> legalPersonRepo,
									 IBankAccountRepository bankAccountRepo,
									 IHttpContextAccessor httpContextAccessor,
									 IAsyncRepository<Client> clientRepository)
		{
			_bankAccountRepository = bankAccountRepo;
			_physicalPersonRepository = physicalPersonRepo;
			_legalPersonRepository = legalPersonRepo;
			_clientRepository = clientRepository;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		///     Добавление нового счета существующего клиента.
		///     Возвращает представление и модель с физическими и юридическими лицами, для вывода частичной информации.
		/// </summary>
		/// <param name="idClient"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> CreateClientAccountForm()
		{
			var idClient = await GetUserId();

			var physicalPerson = await _physicalPersonRepository.GetById(idClient);
			var legalPerson = await _legalPersonRepository.GetById(id: idClient);

			//проверка, что хоть какого-то клиента нашло
			if (physicalPerson == null &&
				legalPerson    == null) { return RedirectToAction("GetAccounts"); }

			return View(model: new CreateClientAccountViewModel
			{
				PhysicalPerson = physicalPerson,
				LegalPerson = legalPerson,
				ReturnUrl = "/"
			});
		}

		/// <summary>
		///     Создание счета на основе заполненной формы
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
				_bankAccountRepository.SaveAccount(account: account);

				return RedirectToAction("GetAccounts" ,"BankAccount", createClientAccountViewModel);
			}

			return View();
		}

		/// <summary>
		/// Возвращает ID авторизованого пользователя
		/// </summary>
		/// <returns></returns>
		private async Task<int?> GetUserId()
		{
			var login = _httpContextAccessor.HttpContext.User.Identity.Name;
			var clients =  await _clientRepository.GetAll();
			var client = clients.FirstOrDefault(c => c.Login.Equals(login));

			return client?.Id;
		}

		/// <summary>
		/// Отображение всех счетов клиента
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetAccounts(int? idClient)
		{
			if (idClient == null) { idClient = await GetUserId(); }

			var accounts = _bankAccountRepository
						   .Accounts
						   .Include(p => p.IdCurrencyNavigation)
						   .Where(c => c.IdClient == idClient);

			return View(accounts);
		}

		/// <summary>
		///     Закрытие счета клиента
		/// </summary>
		/// <param name="idAccount"></param>
		/// <returns></returns>
		public IActionResult BankAccountClose(int idAccount)
		{
			_bankAccountRepository.CloseAccount(idAccount: idAccount);

			return RedirectToAction("GetAccounts");
		}

		//Перед тем, как удалить счет, нужно его закрыть методом BankAccountClose
		/// <summary>
		///     Удаление счета клиента
		/// </summary>
		/// <param name="idAccount"></param>
		/// <returns></returns>
		public IActionResult BankAccountDelete(int idAccount)
		{
			_bankAccountRepository.DeleteAccount(idAccount: idAccount);

			return RedirectToAction("GetAccounts");
		}
	}
}
