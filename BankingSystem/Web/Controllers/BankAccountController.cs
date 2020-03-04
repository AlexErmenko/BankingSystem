﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Controllers
{
    public class BankAccountController : Controller
	{
		private IAsyncRepository<BankAccount> _bankAccountRepository;
		private readonly BankingSystemContext _bankingSystemContext;
		private IAsyncRepository<PhysicalPerson> _physicalPersonRepository;
		private IAsyncRepository<LegalPerson> _legalPersonRepository;

		public BankAccountController(IAsyncRepository<BankAccount> bankAccountRepositoryAccount, BankingSystemContext bankingSystemCtx,
									 IAsyncRepository<PhysicalPerson> physicalPersonRepo, IAsyncRepository<LegalPerson> legalPersonRepo)
		{
			_bankAccountRepository = bankAccountRepositoryAccount;
			_bankingSystemContext = bankingSystemCtx;
			_physicalPersonRepository = physicalPersonRepo;
			_legalPersonRepository = legalPersonRepo;
		}

		/// <summary>
		/// Добавление нового счета существующего клиента.
		/// Возвращает представление и модель с физическими и юридическими лицами, для вывода частичной информации.
		/// </summary>
		/// <param name="idClient"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> CreateClientAccountForm(int idClient)
		{
			PhysicalPerson physicalPerson = await _physicalPersonRepository.GetById(idClient);
			LegalPerson legalPerson = await _legalPersonRepository.GetById(idClient);

			return View(new CreateClientAccountViewModel()
			{
				PhysicalPerson = physicalPerson,
				LegalPerson = legalPerson,
				ReturnUrl = $"/"
			});
		}

		/// <summary>
		/// Создание счета на основе заполненной формы.
		/// </summary>
		/// <param name="createClientAccountViewModel"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult CreateClientAccountForm(CreateClientAccountViewModel createClientAccountViewModel)
		{
			if (ModelState.IsValid)
			{
				var account = createClientAccountViewModel.Account;
				var idCurrency = _bankingSystemContext
								 .Currencies.FirstOrDefault(c => c.IdCurrency == account.IdCurrency)?.Name;

				if (idCurrency != null)
				{
					_bankingSystemContext.Database.ExecuteSqlRaw($@"EXEC bank_operations.dbo.CreateAccount 
                        @id_client = {account.IdClient}, @account_type = {account.AccountType}, @currency = {idCurrency}, @amount = 0");
				} 
			}

			return View();
		}

		/// <summary>
		/// Закрытие счета клиента
		/// </summary>
		/// <param name="idAccount"></param>
		/// <returns></returns>
		public IActionResult BankAccountClose(int idAccount)
		{
			_bankingSystemContext.Database.ExecuteSqlRaw($@"EXEC CloseAccount @idClient = {idAccount}");

			return View("Index");
		}

		//Перед тем, как удалить счет, нужно его закрыть методом BankAccountClose
		/// <summary>
		/// Удаление счета клиента
		/// </summary>
		/// <param name="idAccount"></param>
		/// <returns></returns>
		public IActionResult BankAccountDelete(int idAccount)
		{
			_bankingSystemContext.Database.ExecuteSqlRaw($@"EXEC bank_operations.dbo.DeleteAccount @id_account = {idAccount}");

			return View("Index");
		}
	}
}