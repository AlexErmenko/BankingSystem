﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Models;
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

		public BankAccountController(IAsyncRepository<PhysicalPerson> physicalPersonRepo, 
									 IAsyncRepository<LegalPerson> legalPersonRepo, 
									 IBankAccountRepository bankAccountRepo)
		{
			_bankAccountRepository = bankAccountRepo;
			_physicalPersonRepository = physicalPersonRepo;
			_legalPersonRepository = legalPersonRepo;

			var username = this.User.Identity.Name;
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
			var physicalPerson = await _physicalPersonRepository.GetById(id: idClient);
			var legalPerson = await _legalPersonRepository.GetById(id: idClient);

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

				return RedirectToAction("GetAccounts");
			}

			return View();
		}

		/// <summary>
		/// Отображение всех счетов клиента
		/// </summary>
		/// <returns></returns>
		public IActionResult GetAccounts(int idClient)
		{
			return View(_bankAccountRepository.Accounts
											  .Include(p => p.IdCurrencyNavigation));
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
