using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.Credit;

namespace Web.Controllers
{
	/// <summary>
	/// Оформление, жизненный цикл кредитов
	/// </summary>
	[Authorize(Roles = "Client, Manager")]
	public class CreditController : Controller
	{
		private readonly IAsyncRepository<Credit> _creditRepository;

		public CreditController(IAsyncRepository<Credit> creditRepository) { _creditRepository = creditRepository; }

		/// <summary>
		/// Оформление кредита пользователем или менеджером
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> TakeCreditForm(int idAccount)
		{
			return View(new TakeCreditViewModel()
			{
				IdAccount = idAccount
			});
		}

		/// <summary>
		/// Получение всех кредитов пользователя
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> AllCredits(int idClient)
		{
			var data = _creditRepository.GetAll().Result
										.Where(c => c.IdAccountNavigation.IdClient == idClient);

			return View(new AllCreditClientViewModel()
			{
				Credits = data,
				IdClient = idClient
			});
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}