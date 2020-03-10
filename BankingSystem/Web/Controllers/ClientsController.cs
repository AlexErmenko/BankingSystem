using System;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
	[Authorize(Roles = AuthorizationConstants.Roles.MANAGER)]
	public class ClientsController : Controller
	{
		private IAsyncRepository<Client> Repository { get; }

		//TODO: Заменить на репозиторий
		public ClientsController(IAsyncRepository<Client> repository) => Repository = repository;

		// GET: Clients
		public async Task<IActionResult> Index()
		{
			var task = await Repository.GetAll();
			return View(model: task);
		}

		// GET: Clients/Create
		public IActionResult Create() => View();

		// POST: Clients/Create
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdClient,Login,Password,Address,TelNumber")] Client client)
		{
			if(ModelState.IsValid)
			{
				await Repository.AddAsync(entity: client);
				return RedirectToAction(actionName: nameof(Index));
			}

			return View(model: client);
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
		public async Task<IActionResult> Edit(int id, [Bind("IdClient,Login,Password,Address,TelNumber")] Client client)
		{
			if(id != client.Id) return NotFound();

			if(ModelState.IsValid)
			{
				try { await Repository.UpdateAsync(entity: client); } catch(DbUpdateConcurrencyException)
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
