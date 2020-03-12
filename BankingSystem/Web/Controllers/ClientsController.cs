using System;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Web.ViewModels;

namespace Web.Controllers
{
	[Authorize(Roles = AuthorizationConstants.Roles.MANAGER)]
	public class ClientsController : Controller
	{
		private IAsyncRepository<Client> Repository { get; }
		private readonly UserManager<ApplicationUser> _userManager;

		public ClientsController(IAsyncRepository<Client> repository, UserManager<ApplicationUser> userManager)
		{
			Repository = repository;
			_userManager = userManager;
		}

		// GET: Clients
		public async Task<IActionResult> Index()
		{
			var task = await Repository.GetAll();
			return View(model: task);
		}

		#region Удалить после создания ClientCreate, etc

		// GET: Clients/Create
		public IActionResult Create() => View();

		// POST: Clients/Create
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Login,Password,Address,TelNumber")] Client client)
		{
			if (ModelState.IsValid)
			{
				await Repository.AddAsync(entity: client);
				return RedirectToAction(actionName: nameof(Index));
			}

			return View(model: client);
		}

		#endregion

		// GET: Clients/CreateClient
		public IActionResult CreateClient() => View();

		// POST: Clients/CreateClient
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateClient(ClientCreateViewModel client)
		{
			if (ModelState.IsValid)
			{
				var passwordValidator = new PasswordValidator<ApplicationUser>();
				var result = await passwordValidator.ValidateAsync(_userManager, null, client.Password);

				if (result.Succeeded)
				{
					//if pass is valid
					if (client.IsPhysicalPerson)
					{
						//PhysicalPerson

						return RedirectToAction(nameof(CreatePhysicalPerson),
												routeValues: new
												{
													client.Login,
													client.Email,
													client.Password,
													client.TelNumber,
													client.Address
												});
					}
					else
					{
						//LegalPerson
					}
				} 
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}

			return View(client);
		}

		[HttpGet]
		public IActionResult CreatePhysicalPerson(string login,
												  string email,
												  string password,
												  string telNumber,
												  string address)
		{
			PhysicalPersonCreateViewModel physicalPerson = new PhysicalPersonCreateViewModel()
			{
				Login = login,
				Email = email,
				Password = password,
				TelNumber = telNumber,
				Address = address
			};

			return View(physicalPerson);
		}

		// POST: Clients/CreatePhysicalPerson
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> CreatePhysicalPerson(PhysicalPersonCreateViewModel physicalPerson)
		{
			if (ModelState.IsValid)
			{
				//TODO: Add in table: Identity, Client, PhysicalPerson
			}

			return View(physicalPerson);
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
				try
				{
					await Repository.UpdateAsync(entity: client);
				} 
				catch(DbUpdateConcurrencyException)
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
	}
}
