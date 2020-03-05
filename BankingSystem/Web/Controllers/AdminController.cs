﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Web.Controllers
{
	public class AdminController : Controller
	{
		private UserManager<ApplicationUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;
		private SignInManager<ApplicationUser> _signInManager;

		public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		// GET: Admin
		public async Task<IActionResult> Index()
		{
			if (User.IsInRole("Administrators"))
			{
				var users = from m in _userManager.Users
							select m;
				if (User.IsInRole("Manager")) ;




			}


			


			return View();
		}

		// GET: Admin/Details/5
		public ActionResult Details(int id) { return View(); }

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