﻿using System;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Web.Commands;
using Web.ViewModels.BankAccount;

namespace Web.Controllers
{
	[Authorize(Roles = "Client, Manager")]
	public class BankAccountController : Controller
	{
		private readonly IBankAccountRepository _bankAccountRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IAsyncRepository<LegalPerson> _legalPersonRepository;
		private readonly IAsyncRepository<PhysicalPerson> _physicalPersonRepository;
		private readonly IMediator Mediator;

		public BankAccountController(IAsyncRepository<PhysicalPerson> physicalPersonRepo, IAsyncRepository<LegalPerson> legalPersonRepo, IBankAccountRepository bankAccountRepo, IHttpContextAccessor httpContextAccessor, IMediator Mediator)
		{
			_bankAccountRepository = bankAccountRepo;
			_physicalPersonRepository = physicalPersonRepo;
			_legalPersonRepository = legalPersonRepo;
			_httpContextAccessor = httpContextAccessor;
			this.Mediator = Mediator;
		}

		/// <summary>
		///     Добавление нового счета существующего клиента.
		///     Возвращает представление и модель с физическими и юридическими лицами, для вывода частичной информации.
		/// </summary>
		/// <param name="idClient"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> CreateClientAccountForm(int? idClient)
		{
			if(idClient == null)
				idClient = await GetUserId();

			var physicalPerson = await _physicalPersonRepository.GetById(id: idClient);
			var legalPerson = await _legalPersonRepository.GetById(id: idClient);

			//проверка, что хоть какого-то клиента нашло
			if(physicalPerson == null
			   && legalPerson == null) return RedirectToAction(actionName: "GetAccounts");

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
		public async Task<IActionResult> CreateClientAccountForm(CreateClientAccountViewModel createClientAccountViewModel)
		{
			if(ModelState.IsValid)
			{
				//сохранение счета
				var account = createClientAccountViewModel.Account;
				await _bankAccountRepository.SaveAccount(account: account);

				return RedirectToAction(actionName: "GetAccounts", controllerName: "BankAccount", routeValues: createClientAccountViewModel);
			}

			return View();
		}

		/// <summary>
		///     Возвращает ID авторизованого пользователя
		/// </summary>
		/// <returns></returns>
		private async Task<int?> GetUserId() => await Mediator.Send(request: new GetUserByIdQuery(Login: User.Identity.Name));

		/// <summary>
		///     Отображение всех счетов клиента
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetAccounts(int? idClient)
		{
			if(idClient == null
			   && _httpContextAccessor.HttpContext.User.IsInRole(role: "Client")) idClient = await GetUserId();

			var accounts = _bankAccountRepository.Accounts.Include(navigationPropertyPath: p => p.IdCurrencyNavigation).Where(predicate: c => c.IdClient == idClient);

			return View(model: new AccountViewModel
			{
				IdClient = idClient,
				BankAccounts = accounts
			});
		}

		/// <summary>
		///     Закрытие счета клиента
		/// </summary>
		/// <param name="idAccount"></param>
		/// <returns></returns>
		public async Task<IActionResult> BankAccountClose(int idAccount)
		{
			await _bankAccountRepository.CloseAccount(idAccount: idAccount);

			return RedirectToAction(actionName: "GetAccounts");
		}

		//Перед тем, как удалить счет, нужно его закрыть методом BankAccountClose
		/// <summary>
		///     Удаление счета клиента
		/// </summary>
		/// <param name="idAccount"></param>
		/// <returns></returns>
		public async Task<IActionResult> BankAccountDelete(int idAccount)
		{
			await _bankAccountRepository.DeleteAccount(idAccount: idAccount);

			return RedirectToAction(actionName: "GetAccounts");
		}
	}
}
