using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Specifications;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin;

namespace Web.Controllers
{
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext         _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context     = context;
		}

		private bool UserExists(string id) { return _context.Users.Any(e => e.Id == id); }

		// GET: Admin
		public async Task<IActionResult> Index()
		{
			//Так читабельниее
			if (User.IsInRole(AuthorizationConstants.Roles.ADMINISTRATORS))
			{
				var _users = from n in _userManager.Users
							 select n;
				var UserVM = new UserViewModel
				{
					AppUsers = await _users.ToListAsync(), ManagerUsers = new List<ApplicationUser>()
				};

				foreach (var user in UserVM.AppUsers)
					if (await _userManager.IsInRoleAsync(user, "Manager"))
						UserVM.ManagerUsers.Add(user);
				// foreach (var item in UserVM.ManagerUsers)
				// {
				// 	Console.WriteLine(item);
				// }
				return View(UserVM);
			}

			return NotFound();
		}

		// GET: Admin/Details/5
		public ActionResult Details(int id) { return View(); }

		// GET: Admin/Create
		public ActionResult Create() { return View(); }

		// POST: Admin/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("UserName,Email,PhoneNumber")] ApplicationUser applicationUser)
		{
			var user = new ApplicationUser
			{
				UserName    = applicationUser.UserName,
				Email       = applicationUser.Email,
				PhoneNumber = applicationUser.PhoneNumber
			};

			await _userManager.CreateAsync(user, AuthorizationConstants.DEFAULT_PASSWORD);
			await _userManager.AddToRoleAsync(user, AuthorizationConstants.Roles.MANAGER);
			user = await _userManager.FindByNameAsync(applicationUser.UserName);


			return RedirectToAction(nameof(Index));
		}

		// GET: Admin/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			// if (id == null)
			// {
			// 	return NotFound();
			// }
			//
			// var user = await _userManager.FindByIdAsync(id);
			// if (user == null)
			// {
			// 	return NotFound();
			// }
			// return View(user);
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();
			var model = new EditUserViewModel
			{
				UserName = user.UserName, Email = user.Email, PhoneNumber = user.PhoneNumber
			};
			return View(model);
		}

		// POST: Admin/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditUserViewModel applicationUser)
		{
			// if (applicationUser ==null)
			// {
			// 	return NotFound();
			// }
			// if (ModelState.IsValid)
			// {
			// 	try
			// 	{
			//
			// 		await _userManager.UpdateAsync(applicationUser);
			// 	}
			// 	catch (DbUpdateConcurrencyException)
			// 	{
			// 		if (!UserExists(applicationUser.Id))
			// 		{
			// 			return NotFound();
			// 		}
			// 		else
			// 		{
			// 			throw;
			// 		}
			// 	}
			// 	return RedirectToAction(nameof(Index));
			// }
			// return View(applicationUser);
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(applicationUser.Id);
				if (user != null)
				{
					user.Email       = applicationUser.Email;
					user.UserName    = applicationUser.UserName;
					user.PhoneNumber = applicationUser.PhoneNumber;

					var result = await _userManager.UpdateAsync(user);
					if (result.Succeeded)
						return RedirectToAction("Index");
					foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(applicationUser);
		}

		// GET: Admin/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null) return NotFound();

			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();

			return View(user);
		}

		// POST: Admin/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id, IFormCollection collection)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(id);
				await _userManager.DeleteAsync(user);
				return RedirectToAction(nameof(Index));
			}
			catch { return View(); }
		}
	}
}