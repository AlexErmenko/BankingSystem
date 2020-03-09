using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
	[Authorize(Roles = AuthorizationConstants.Roles.MANAGER)]
	public class ClientsController : Controller
	{
		private IAsyncRepository<Client> Repository { get; set; }
		
		//TODO: Заменить на репозиторий
		public ClientsController(IAsyncRepository<Client> repository)
		{
			Repository = repository;
		}

		// GET: Clients
		public async Task<IActionResult> Index()
		{
			var task = await Repository.GetAll();
			return View(task);
		}

		// GET: Clients/Create
		public IActionResult Create() { return View(); }

		// POST: Clients/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdClient,Login,Password,Address,TelNumber")]
												Client client)
		{
			if (ModelState.IsValid)
			{
				await Repository.AddAsync(client);
				return RedirectToAction(nameof(Index));
			}

			return View(client);
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
		public async Task<IActionResult> Edit(int id, [Bind("IdClient,Login,Password,Address,TelNumber")]
											  Client client)
		{
			if (id != client.Id) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					await Repository.UpdateAsync(client);
				}
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

		private bool ClientExists(int id)
		{
			return Repository.GetAll().Result.Any(e => e.Id == id);
		}
	}
}