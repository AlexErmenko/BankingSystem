using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using Web.ViewModels;

namespace Web.Controllers
{
	public class AdminController : Controller
	{
		// private readonly IAsyncRepository<ApplicationUser> _userRepository;

		public UserManager<ApplicationUser> UserManager { get; set; }
		public RoleManager<ApplicationUser> RoleManager { get; set; }


		public AdminController(UserManager<ApplicationUser> userManager) { UserManager = userManager; }

		// GET: Admin
		public async Task<IActionResult> Index(UserManager<ApplicationUser> userManager,
											   RoleManager<IdentityRole>    roleManager)
		{
			// var users = await userManager.GetUserAsync();
			// var UsersVM = new UsersViewModel {Users = users};
			return View();
		}

		// GET: Admin/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null) { return NotFound(); }

			// var user = await _userRepository.GetById(id);


			return View();
		}

		// GET: Admin/Create
		public async Task<IActionResult> Create()
		{

			return View();
		}

		// POST: Admin/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("UserName,Password,Email,PhoneNumber")]ApplicationUser applicationUser )
		{
			var user = new ApplicationUser()
			{
				UserName    = applicationUser.UserName,
				Email       = applicationUser.Email,
				PhoneNumber = applicationUser.PhoneNumber,
			};

			await UserManager.CreateAsync(user,AuthorizationConstants.DEFAULT_PASSWORD);



			return View(applicationUser);
		}

		// GET: Admin/Edit/5
		public  async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}
			// var user = await _userRepository.GetById(id);

			return View();
		}

		// POST: Admin/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, IFormCollection collection, [Bind("UserName,Email,PhoneNumber")] ApplicationUser applicationUser)
		{
			/*if (id != applicationUser.Id)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				try
				{
					await _userRepository.UpdateAsync(applicationUser);
				}
				catch (DbUpdateConcurrencyException)
				{
				
				}
				return RedirectToAction(nameof(Index));
			}
			*/



			return View();

		}

		// GET: Admin/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			
			if (id == null)
			{
				return NotFound();
			}

			// var user = await _userRepository.GetById(id);

			return View();
		}

		// POST: Admin/Delete/5
		[HttpPost,ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id, IFormCollection collection)
		{
			/*try
			{
				var user = await _userRepository.GetById(id);
				await _userRepository.DeleteAsync(user);
				

				return RedirectToAction(nameof(Index));
			}
	
			catch {  }*/
			return View();
		}
	}
}