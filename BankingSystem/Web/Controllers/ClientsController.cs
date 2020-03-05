﻿using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entity;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
	public class ClientsController : Controller
	{
		private readonly BankingSystemContext _context;

		//TODO: Заменить на репозиторий
		public ClientsController(BankingSystemContext context) { _context = context; }

		// GET: Clients
		public async Task<IActionResult> Index() { return View(await _context.Clients.ToListAsync()); }

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
				_context.Add(client);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(client);
		}

		// GET: Clients/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var client = await _context.Clients.FindAsync(id);
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
					_context.Update(client);
					await _context.SaveChangesAsync();
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
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var client = await _context.Clients
									   .FirstOrDefaultAsync(m => m.Id == id);
			if (client == null) return NotFound();

			return View(client);
		}

		// POST: Clients/Delete/5
		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var client = await _context.Clients.FindAsync(id);
			_context.Clients.Remove(client);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ClientExists(int id) { return _context.Clients.Any(e => e.Id == id); }
	}
}