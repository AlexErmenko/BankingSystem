using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web.Services;

namespace Web.Controllers
{
	

	public class CurrencyController : Controller
	{
		public CurrencyController(CurrencyViewModelSerivce currencyViewModelSerivce)
		{
			_currencyViewModelSerivce = currencyViewModelSerivce;
		}

		private readonly CurrencyViewModelSerivce _currencyViewModelSerivce;


		public async Task<IActionResult> GetInfo()
		{
			var currencyRate = await _currencyViewModelSerivce.GetCurrencyRate();
			return View(currencyRate);
		}


		// GET
		public async Task<IActionResult> Index()
		{

			
			return View();
		}
	}
}