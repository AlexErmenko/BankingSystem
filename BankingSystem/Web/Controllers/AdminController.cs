using ApplicationCore.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	public class AdminController : Controller
	{
		private IAsyncRepository<IdentityRole>    _roleRepository;
		private IAsyncRepository<ApplicationUser> _userRepository;

		public AdminController(IAsyncRepository<ApplicationUser> userRepository,
							   IAsyncRepository<IdentityRole>    roleRepository)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
		}

		// GET: Admin
		public ActionResult Index() { return View(); }

		// GET: Admin/Details/5
		public ActionResult Details(int id) { return View(); }

		// GET: Admin/Create
		public ActionResult Create() { return View(); }

		// POST: Admin/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				// TODO: Add insert logic here

				return RedirectToAction(nameof(Index));
			}
			catch { return View(); }
		}

		// GET: Admin/Edit/5
		public ActionResult Edit(int id) { return View(); }

		// POST: Admin/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction(nameof(Index));
			}
			catch { return View(); }
		}

		// GET: Admin/Delete/5
		public ActionResult Delete(int id) { return View(); }

		// POST: Admin/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction(nameof(Index));
			}
			catch { return View(); }
		}
	}
}