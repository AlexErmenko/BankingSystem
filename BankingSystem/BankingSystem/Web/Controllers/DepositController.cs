using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DepositController : Controller
	{
		private IAsyncRepository<Deposit> deposit;
		public DepositController()
		{

		}
        public IActionResult Index()
        {
            return View();
        }
    }
}