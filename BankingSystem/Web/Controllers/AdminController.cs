using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Specifications;

using Infrastructure;
using Infrastructure.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Web.ViewModels.Admin;

namespace Web.Controllers
{
	[Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		private bool UserExists(string id) { return _context.Users.Any(predicate: e => e.Id == id); }

		// GET: Admin
		public async Task<IActionResult> ManagerList()
		{
			//Так читабельниее
			if(User.IsInRole(role: AuthorizationConstants.Roles.ADMINISTRATORS))
			{
				var _users = from n in _userManager.Users select n;
				var UserVM = new UserViewModel
				{
					AppUsers = await _users.ToListAsync(),
					ManagerUsers = new List<ApplicationUser>()
				};

				foreach(var user in UserVM.AppUsers)
				{
					if(await _userManager.IsInRoleAsync(user: user, role: "Manager"))
						UserVM.ManagerUsers.Add(item: user);
				}

				// foreach (var item in UserVM.ManagerUsers)
				// {
				// 	Console.WriteLine(item);
				// }
				return View(model: UserVM);
			}

			return NotFound();
		}

		// GET: Admin/Details/5
		public ActionResult Details(int id) => View();

		// GET: Admin/Create
		public ActionResult AddManager() => View();

		// POST: Admin/Create
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> AddManager([Bind("UserName,Email,PhoneNumber")] ApplicationUser applicationUser)
		{
			var user = new ApplicationUser
			{
				UserName = applicationUser.UserName,
				Email = applicationUser.Email,
				PhoneNumber = applicationUser.PhoneNumber
			};

			await _userManager.CreateAsync(user: user, password: AuthorizationConstants.DEFAULT_PASSWORD);
			await _userManager.AddToRoleAsync(user: user, role: AuthorizationConstants.Roles.MANAGER);

			// user = await _userManager.FindByNameAsync(applicationUser.UserName);

			return RedirectToAction(actionName: nameof(ManagerList));
		}

		// GET: Admin/Edit/5
		public async Task<IActionResult> EditManager(string id)
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
			var user = await _userManager.FindByIdAsync(userId: id);
			if(user == null) return NotFound();

			var model = new EditUserViewModel
			{
				UserName = user.UserName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber
			};
			return View(model: model);
		}

		// POST: Admin/Edit/5
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> EditManager(EditUserViewModel applicationUser)
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
			if(ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(userId: applicationUser.Id);
				if(user != null)
				{
					user.Email = applicationUser.Email;
					user.UserName = applicationUser.UserName;
					user.PhoneNumber = applicationUser.PhoneNumber;

					var result = await _userManager.UpdateAsync(user: user);
					if(result.Succeeded)
						return RedirectToAction(actionName: "ManagerList");

					foreach(var error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
				}
			}

			return View(model: applicationUser);
		}

		// GET: Admin/Delete/5
		public async Task<IActionResult> DeleteManager(string id)
		{
			if(id == null) return NotFound();

			var user = await _userManager.FindByIdAsync(userId: id);
			if(user == null) return NotFound();

			return View(model: user);
		}

		// POST: Admin/Delete/5
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteManager(string id, IFormCollection collection)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId: id);
				await _userManager.DeleteAsync(user: user);
				return RedirectToAction(actionName: nameof(ManagerList));
			} catch {
				return View();
			}
		}
	}
}
