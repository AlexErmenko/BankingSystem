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
using Web.ViewModels.Clients;

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
				Client = client
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
					UserName    = physicalPersonViewModel.Client.Login,
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
				Client = client
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
					UserName    = legalPersonViewModel.Client.Login,
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
			var physicalClients = await PhysicalPerson.GetAll();
			var legalClients    = await LegalRepository.GetAll();

			var client = await Repository.GetById(id: id);

			if (client == null) 
				return NotFound();

			var user = await _userManager.FindByNameAsync(client.Login);

			HttpContext.Session.Set<string>("IdIdentity", user.Id);

			if (client.PhysicalPerson != null)
			{
				return View("EditPhysicalPerson", new PhysicalPersonViewModel
				{
					Client = client,
					PhysicalPerson = client.PhysicalPerson
				});
			} 
			else
			{
				return View("EditLegalPerson", new LegalPersonViewModel
				{
					Client = client,
					LegalPerson = client.LegalPerson
				});
			}
		}

		// POST: Clients/EditPhysicalPerson/5
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> EditPhysicalPerson(PhysicalPersonViewModel physicalPersonViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var client = physicalPersonViewModel.Client;
					client.PhysicalPerson = physicalPersonViewModel.PhysicalPerson;

					await Repository.UpdateAsync(client);

					var userId = HttpContext.Session.Get<string>("IdIdentity");

					var user = await _userManager.FindByIdAsync(userId);

					user.UserName = physicalPersonViewModel.Client.Login;
					user.Email = physicalPersonViewModel.Client.Login;
					user.PhoneNumber = physicalPersonViewModel.Client.TelNumber;

					await _userManager.UpdateAsync(user);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ClientExists(id: physicalPersonViewModel.Client.Id))
						return NotFound();

					throw;
				}

				return RedirectToAction(actionName: nameof(Index));
			}

			return View(physicalPersonViewModel);
		}

		// POST: Clients/EditPhysicalPerson/5
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> EditLegalPerson(LegalPersonViewModel legalPersonViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var client = legalPersonViewModel.Client;
					client.LegalPerson = legalPersonViewModel.LegalPerson;

					await Repository.UpdateAsync(client);

					var userId = HttpContext.Session.Get<string>("IdIdentity");

					var user = await _userManager.FindByIdAsync(userId);

					user.UserName    = legalPersonViewModel.Client.Login;
					user.Email       = legalPersonViewModel.Client.Login;
					user.PhoneNumber = legalPersonViewModel.Client.TelNumber;

					await _userManager.UpdateAsync(user);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ClientExists(id: legalPersonViewModel.Client.Id))
						return NotFound();

					throw;
				}

				return RedirectToAction(actionName: nameof(Index));
			}

			return View(legalPersonViewModel);
		}

		#endregion

		#region Delete

		// GET: Clients/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var physicalClients = await PhysicalPerson.GetAll();
			var legalClients    = await LegalRepository.GetAll();

			var client = await Repository.GetById(id: id);

			if (client == null)
				return NotFound();

			var user = await _userManager.FindByNameAsync(client.Login);

			HttpContext.Session.Set<string>("IdIdentity", user.Id);

			if (client.PhysicalPerson != null)
			{
				return View("DeletePhysicalPerson", new PhysicalPersonViewModel
				{
					Client         = client,
					PhysicalPerson = client.PhysicalPerson
				});
			} else
			{
				return View("DeleteLegalPerson", new LegalPersonViewModel
				{
					Client      = client,
					LegalPerson = client.LegalPerson
				});
			}
		}

		// POST: Clients/Delete/5
		[HttpPost, ActionName(name: "Delete"), ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			//Delete client from Clients and (PhysicalPerson or LegalPerson)
			var client = await Repository.GetById(id: id);
			await Repository.DeleteAsync(entity: client);

			//Delete user from Identity
			var userId = HttpContext.Session.Get<string>("IdIdentity");
			var user = await _userManager.FindByIdAsync(userId);
			await _userManager.DeleteAsync(user);

			return RedirectToAction(actionName: nameof(Index));
		}

		#endregion

		#region ChangePassword

		public async Task<IActionResult> ChangePassword(string email)
		{
			ApplicationUser user = await _userManager.FindByNameAsync(email);

			if (user == null)
			{
				return NotFound();
			}

			ChangePasswordViewModel model = new ChangePasswordViewModel { Email = user.Email };

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid) 
				return View(model);

			ApplicationUser user = await _userManager.FindByNameAsync(model.Email);

			if (user != null)
			{
				var resultPassValid = await Mediator.Send(new GetPasswordValidationQuery(null, model.NewPassword));

				if (resultPassValid.Succeeded)
				{
					var resultChange = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

					if (resultChange.Succeeded)
					{
						return RedirectToAction("Index");
					} 
					else
					{
						foreach (var error in resultChange.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				} 
				else
				{
					foreach (var error in resultPassValid.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			} else
			{
				ModelState.AddModelError(string.Empty, "Пользователь не найден");
			}
			return View(model);
		}

		#endregion

		private bool ClientExists(int id) { return Repository.GetAll().Result.Any(predicate: e => e.Id == id); }
	}
}
