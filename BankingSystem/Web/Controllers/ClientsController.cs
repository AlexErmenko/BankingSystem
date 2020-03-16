using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Web.Commands;
using Web.Extension;
using Web.ViewModels;

namespace Web.Controllers
{
	[Authorize(Roles = AuthorizationConstants.Roles.MANAGER)]
	public class ClientsController : Controller
	{
		private IAsyncRepository<Client> Repository { get; }
		private IAsyncRepository<LegalPerson> LegalRepository { get; set; }
		private IAsyncRepository<PhysicalPerson> PhysicalPerson { get; set; }

		private readonly UserManager<ApplicationUser> _userManager;
		private IMediator Mediator { get; set; }

		public ClientsController(IAsyncRepository<Client> repository, UserManager<ApplicationUser> userManager, IMediator mediator, IAsyncRepository<LegalPerson> legalRepository, IAsyncRepository<PhysicalPerson> physicalPerson)
		{
			Repository = repository;
			_userManager = userManager;
			Mediator = mediator;
			LegalRepository = legalRepository;
			PhysicalPerson = physicalPerson;
		}

		// GET: Clients
		public async Task<IActionResult> Index()
		{
			var clients = await Repository.GetAll();
			var physicalClients = await PhysicalPerson.GetAll();
			var legalClients = await LegalRepository.GetAll();

			return View(model: clients);
		}

		#region Create

		// GET: Clients/CreateClient
		public IActionResult CreateClient() => View();

		// POST: Clients/CreateClient
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateClient(ClientCreateViewModel clientCreateViewModel)
		{
			if (!ModelState.IsValid)
				return View(clientCreateViewModel);

			var result = await Mediator.Send(new GetPasswordValidationQuery(null, clientCreateViewModel.Password));
			if (result.Succeeded)
			{
				HttpContext.Session.Set<Client>("NewClientData", clientCreateViewModel.Client);
				HttpContext.Session.Set<string>("PassClient", clientCreateViewModel.Password);

				//if pass is valid
				if (clientCreateViewModel.IsPhysicalPerson)
				{
					//PhysicalPerson
					return RedirectToAction(nameof(CreatePhysicalPerson), clientCreateViewModel);
				} else
				{
					//LegalPerson
					return RedirectToAction(nameof(CreateLegalPerson), clientCreateViewModel);
				}
			} else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(clientCreateViewModel);
		}

		#endregion

		#region CreatePhysicalPerson

		[HttpGet]
		public IActionResult CreatePhysicalPerson(ClientCreateViewModel clientCreateViewModel)
		{
			var client = HttpContext.Session.Get<Client>("NewClientData");

			var physicalPersonCreateViewModel = new PhysicalPersonViewModel()
			{
				Client = client,
				UserName  = clientCreateViewModel.UserName
			};

			return View(physicalPersonCreateViewModel);
		}

		// POST: Clients/CreatePhysicalPerson
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> CreatePhysicalPerson(PhysicalPersonViewModel physicalPersonViewModel)
		{
			if (ModelState.IsValid)
			{
				var client = physicalPersonViewModel.Client;
				client.PhysicalPerson = physicalPersonViewModel.PhysicalPerson;

				//Add to tables Client and PhysicalPerson
				await Repository.AddAsync(client);

				//Add new User to Identity
				ApplicationUser user = new ApplicationUser()
				{
					UserName    = physicalPersonViewModel.UserName,
					Email       = physicalPersonViewModel.Client.Login,
					PhoneNumber = physicalPersonViewModel.Client.TelNumber
				};

				var password = HttpContext.Session.Get<string>("PassClient");
				var result   = await _userManager.CreateAsync(user, password);

				if (result.Succeeded)
				{
					//Set Roles CLIENT to new User
					await _userManager.AddToRoleAsync(user, AuthorizationConstants.Roles.CLIENT);

					return RedirectToAction(nameof(Index));
				} else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}

			return View(physicalPersonViewModel);
		}

		#endregion

		#region CreateLegalPerson

		[HttpGet]
		public IActionResult CreateLegalPerson(ClientCreateViewModel clientCreateViewModel)
		{
			var client = HttpContext.Session.Get<Client>("NewClientData");

			var legalPersonCreateViewModel = new LegalPersonViewModel()
			{
				Client = client,
				UserName = clientCreateViewModel.UserName
			};

			return View(legalPersonCreateViewModel);
		}

		// POST: Clients/CreatePhysicalPerson
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateLegalPerson(LegalPersonViewModel legalPersonViewModel)
		{
			if (ModelState.IsValid)
			{
				var client = legalPersonViewModel.Client;
				client.LegalPerson = legalPersonViewModel.LegalPerson;

				//Add to tables Client and LegalPerson
				await Repository.AddAsync(client);

				//Add new User to Identity
				ApplicationUser user = new ApplicationUser()
				{
					UserName    = legalPersonViewModel.UserName,
					Email       = legalPersonViewModel.Client.Login,
					PhoneNumber = legalPersonViewModel.Client.TelNumber
				};

				var password = HttpContext.Session.Get<string>("PassClient");
				var result   = await _userManager.CreateAsync(user, password);

				if (result.Succeeded)
				{
					//Set Roles CLIENT to new User
					await _userManager.AddToRoleAsync(user, AuthorizationConstants.Roles.CLIENT);

					return RedirectToAction(nameof(Index));
				} else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}

			return View(legalPersonViewModel);
		}

		#endregion

		#region Edit

		// GET: Clients/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var client = await Repository.GetById(id: id);
			if (client == null) return NotFound();

			var clientIdentity = await _userManager.FindByEmailAsync(client.Login);
			var userName = clientIdentity.UserName;

			if (client.PhysicalPerson != null)
			{
				return View("EditPhysicalPerson", new PhysicalPersonViewModel
				{
					Client = client,
					UserName = userName,
					PhysicalPerson = client.PhysicalPerson
				});
			} 
			else
			{
				return View("EditLegalPerson", new LegalPersonViewModel
				{
					Client = client,
					UserName = userName,
					LegalPerson = client.LegalPerson
				});
			}
		}

		//// POST: Clients/Edit/5
		//[HttpPost, ValidateAntiForgeryToken]
		//public async Task<IActionResult> EditPhysicalPerson(PhysicalPersonViewModel physicalPersonViewModel)
		//{
		//	if (ModelState.IsValid)
		//	{

		//	}
		//}

		//// POST: Clients/Edit/5
		//[HttpPost, ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,Address,TelNumber")] Client client)
		//{
		//	if (id != client.Id) return NotFound();

		//	if (ModelState.IsValid)
		//	{
		//		try
		//		{
		//			await Repository.UpdateAsync(entity: client);
		//		}
		//		catch (DbUpdateConcurrencyException)
		//		{
		//			if (!ClientExists(id: client.Id))
		//				return NotFound();

		//			throw;
		//		}

		//		return RedirectToAction(actionName: nameof(Index));
		//	}
		//}

		#endregion

		#region Delete

		// GET: Clients/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var client = await Repository.GetById(id: id);
			if (client == null) return NotFound();

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

		#endregion

		private bool ClientExists(int id) { return Repository.GetAll().Result.Any(predicate: e => e.Id == id); }
	}
}
