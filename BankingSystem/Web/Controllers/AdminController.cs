using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{	
	public class AdminController : Controller
	{
		private UserManager<ApplicationUser> _userManager;
		// private IAsyncRepository<ApplicationUser> _userRepository;
		// private ApplicationDbContext _context;
	

		public AdminController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
			// _userRepository = userRepository;
			// _context = context;


		}

		// GET: Admin
		public async Task<IActionResult> Index()
		{
			if (User.IsInRole("Administrators"))
			{
				var _users = from n in _userManager.Users
							 select n;
				var UserVM = new UserViewModel
				{
					AppUsers     = await _users.ToListAsync(),
					ManagerUsers = new List<ApplicationUser>()
				};
				
				foreach (var user in UserVM.AppUsers)
				{
					if (await _userManager.IsInRoleAsync(user, "Manager"))
					{
						UserVM.ManagerUsers.Add(user);
					}
				}
				// foreach (var item in UserVM.ManagerUsers)
				// {
				// 	Console.WriteLine(item);
				// }
				return View(UserVM);
			}

			return NotFound();

		}

		// GET: Admin/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Admin/Create
		public ActionResult Create() { return View(); }

		// POST: Admin/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("UserName,Email,PhoneNumber")] ApplicationUser applicationUser )
		{
			var user = new ApplicationUser()
			{
				UserName    = applicationUser.UserName,
				Email       = applicationUser.Email,
				PhoneNumber = applicationUser.PhoneNumber,
			};

			await _userManager.CreateAsync(user, AuthorizationConstants.DEFAULT_PASSWORD);
			await _userManager.AddToRoleAsync(user, AuthorizationConstants.Roles.MANAGER);
			user = await _userManager.FindByNameAsync(applicationUser.UserName);


			return View(applicationUser);
		}

		// GET: Admin/Edit/5
		public async Task<IActionResult> Edit(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return View(user);
		}

		// POST: Admin/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public  async Task<IActionResult> Edit(string? id, IFormCollection collection, [Bind("Id,UserName,Email,PhoneNumber")] ApplicationUser applicationUser)
		{
			if (id != applicationUser.Id)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				try {await _userManager.UpdateAsync(applicationUser); }
				catch (DbUpdateConcurrencyException) { return NotFound(applicationUser);}
				return RedirectToAction(nameof(Index));
			}
			return View();
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