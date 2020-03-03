using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BankAccountController : Controller
	{
		private IAsyncRepository<BankAccount> _repository;

		public BankAccountController(IAsyncRepository<BankAccount> repositoryAccount)
		{
			_repository = repositoryAccount;
		}

		public IActionResult BankAccountClose(int idAccount)
		{
			return View("Index");
		}

		public IActionResult BankAccountDelete(int idAccount)
		{
			return View("Index");
		}
	}
}