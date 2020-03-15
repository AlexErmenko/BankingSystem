﻿using System;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;

using Infrastructure.Identity;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Web.Commands;
using Web.ViewModels;

namespace Web.Controllers
{
	[Authorize(Roles = AuthorizationConstants.Roles.MANAGER)]
	public class ClientsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private IAsyncRepository<Client> Repository { get; }
		private IMediator Mediator { get; }

		public ClientsController(IAsyncRepository<Client> repository, UserManager<ApplicationUser> userManager, IMediator mediator)
		{
			Repository = repository;
			_userManager = userManager;
			Mediator = mediator;
		}

		// GET: Clients
		public async Task<IActionResult> Index()
		{
			var clients = await Repository.GetAll();
			return View(model: clients);
		}

		// GET: Clients/CreateClient
		public IActionResult CreateClient() => View();

		// POST: Clients/CreateClient
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateClient(ClientCreateViewModel clientCreateViewModel)
		{
			if(!ModelState.IsValid)
				return View(model: clientCreateViewModel);

			var result = await Mediator.Send(request: new GetPasswordValidationQuery(user: null, password: clientCreateViewModel.Password));
			if(result.Succeeded)
			{
				//if pass is valid
				if(clientCreateViewModel.IsPhysicalPerson)
				{
					//PhysicalPerson

					return RedirectToAction(actionName: nameof(CreatePhysicalPerson), routeValues: clientCreateViewModel);
				}
			} else foreach(var error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);

			return View(model: clientCreateViewModel);
		}

		[HttpGet]
		public IActionResult CreatePhysicalPerson(ClientCreateViewModel clientCreateViewModel)
		{
			var physicalPersonCreateViewModel = new PhysicalPersonCreateViewModel
			{
				Client = clientCreateViewModel.Client, //TODO: don't work. return null
				Password = clientCreateViewModel.Password,
				Email = clientCreateViewModel.Email
			};

			return View(model: physicalPersonCreateViewModel);
		}

		// POST: Clients/CreatePhysicalPerson
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> CreatePhysicalPerson(PhysicalPersonCreateViewModel physicalPersonCreateViewModel)
		{
			if(ModelState.IsValid)
			{
				var client = physicalPersonCreateViewModel.Client;
				client.PhysicalPerson = physicalPersonCreateViewModel.PhysicalPerson;

				//Add to tables Client and PhysicalPerson
				await Repository.AddAsync(entity: client);

				//Add new User to Identity
				var user = new ApplicationUser
				{
					UserName = physicalPersonCreateViewModel.Client.Login,
					Email = physicalPersonCreateViewModel.Email,
					PhoneNumber = physicalPersonCreateViewModel.Client.TelNumber
				};

				var result = await _userManager.CreateAsync(user: user, password: physicalPersonCreateViewModel.Password);

				if(result.Succeeded)
				{
					//Set Roles CLIENT to new User
					await _userManager.AddToRoleAsync(user: user, role: AuthorizationConstants.Roles.CLIENT);

					return RedirectToAction(actionName: nameof(Index));
				}
				foreach(var error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
			}

			return View(model: physicalPersonCreateViewModel);
		}

		// GET: Clients/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var client = await Repository.GetById(id: id);
			if(client == null) return NotFound();

			return View(model: client);
		}

		// POST: Clients/Edit/5
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,Address,TelNumber")] Client client)
		{
			if(id != client.Id) return NotFound();

			if(ModelState.IsValid)
			{
				try {
					await Repository.UpdateAsync(entity: client);
				} catch(DbUpdateConcurrencyException)
				{
					if(!ClientExists(id: client.Id))
						return NotFound();

					throw;
				}

				return RedirectToAction(actionName: nameof(Index));
			}

			return View(model: client);
		}

		// GET: Clients/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var client = await Repository.GetById(id: id);
			if(client == null) return NotFound();

			return View(model: client);
		}

		// POST: Clients/Delete/5
		[HttpPost, ActionName(name: "Delete"), ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var client = await Repository.GetById(id: id);
			await Repository.DeleteAsync(entity: client);
			return RedirectToAction(actionName: nameof(Index));
		}

		private bool ClientExists(int id) { return Repository.GetAll().Result.Any(predicate: e => e.Id == id); }

		#region CreateLegalPerson

		#endregion
	}
}
