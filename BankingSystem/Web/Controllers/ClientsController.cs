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
using Web.ViewModels;

namespace Web.Controllers
{
	[Authorize(Roles = AuthorizationConstants.Roles.MANAGER)]
	public class ClientsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private          IAsyncRepository<Client>     Repository { get; }

		public ClientsController(IAsyncRepository<Client> repository, UserManager<ApplicationUser> userManager)
		{
			Repository   = repository;
			_userManager = userManager;
		}

		// GET: Clients
		public async Task<IActionResult> Index()
		{
			var clients = await Repository.GetAll();
			return View(clients);
		}

		// GET: Clients/CreateClient
		public IActionResult CreateClient() { return View(); }

		// POST: Clients/CreateClient
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateClient(ClientCreateViewModel client)
		{
			if (ModelState.IsValid)
			{
				var passwordValidator = new PasswordValidator<ApplicationUser>();
				var result =
					await passwordValidator.ValidateAsync(_userManager, user: null, client.Password);

				if (result.Succeeded)
				{
					//if pass is valid
					if (client.IsPhysicalPerson)
						//PhysicalPerson

						return RedirectToAction(nameof(CreatePhysicalPerson), client);
				} else
				{
					foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(client);
		}

		[HttpGet]
		public IActionResult CreatePhysicalPerson(ClientCreateViewModel clientCreate)
		{
			var physicalPerson = new PhysicalPersonCreateViewModel
			{
				Login     = clientCreate.Login,
				Email     = clientCreate.Email,
				Password  = clientCreate.Password,
				TelNumber = clientCreate.TelNumber,
				Address   = clientCreate.Address
			};

			return View(physicalPerson);
		}

		// POST: Clients/CreatePhysicalPerson
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreatePhysicalPerson(PhysicalPersonCreateViewModel physicalPerson)
		{
			if (ModelState.IsValid)
			{
				//Add to tables Client and PhysicalPerson
				await Repository.AddAsync(new Client
				{
					Login     = physicalPerson.Login,
					Password  = physicalPerson.Password,
					Address   = physicalPerson.Address,
					TelNumber = physicalPerson.TelNumber,
					PhysicalPerson = new PhysicalPerson
					{
						IdentificationNumber = physicalPerson.IdentificationNumber,
						PassportNumber       = physicalPerson.PassportNumber,
						PassportSeries       = physicalPerson.PassportSeries,
						Name                 = physicalPerson.Name,
						Surname              = physicalPerson.Surname,
						Patronymic           = physicalPerson.Patronymic
					}
				});

				//Add new User to Identity
				var user = new ApplicationUser
				{
					UserName    = physicalPerson.Login,
					Email       = physicalPerson.Email,
					PhoneNumber = physicalPerson.TelNumber
				};

				var result = await _userManager.CreateAsync(user, physicalPerson.Password);

				if (result.Succeeded)
				{
					//Set Roles CLIENT to new User
					await _userManager.AddToRoleAsync(user, AuthorizationConstants.Roles.CLIENT);

					return RedirectToAction(nameof(Index));
				}

				foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
			}

			return View(physicalPerson);
		}

		// GET: Clients/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var client = await Repository.GetById(id);
			if (client == null) return NotFound();

			return View(client);
		}

		// POST: Clients/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,Address,TelNumber")]
											  Client client)
		{
			if (id != client.Id) return NotFound();

			if (ModelState.IsValid)
			{
				try { await Repository.UpdateAsync(client); }
				catch (DbUpdateConcurrencyException)
				{
					if (!ClientExists(client.Id))
						return NotFound();

					throw;
				}

				return RedirectToAction(nameof(Index));
			}

			return View(client);
		}

		// GET: Clients/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var client = await Repository.GetById(id);
			if (client == null) return NotFound();

			return View(client);
		}

		// POST: Clients/Delete/5
		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var client = await Repository.GetById(id);
			await Repository.DeleteAsync(client);
			return RedirectToAction(nameof(Index));
		}

		private bool ClientExists(int id) { return Repository.GetAll().Result.Any(e => e.Id == id); }

		#region Удалить после создания ClientCreate, etc

		// GET: Clients/Create
		public IActionResult Create() { return View(); }

		// POST: Clients/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Login,Password,Address,TelNumber")]
												Client client)
		{
			if (ModelState.IsValid)
			{
				await Repository.AddAsync(client);
				return RedirectToAction(nameof(Index));
			}

			return View(client);
		}

		#endregion
	}
}
