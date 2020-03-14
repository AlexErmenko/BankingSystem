using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Specifications;
using Infrastructure;
using Infrastructure.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Web.ViewModels.Admin;

namespace Web.Controllers
{
	[Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private IWebHostEnvironment _appEnvironment;


		public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
									IWebHostEnvironment appEnvironment)
		{
			_userManager = userManager;
			_context = context;
			_appEnvironment = appEnvironment;
		}

		private bool UserExists(string id) { return _context.Users.Any(predicate: e => e.Id == id); }

		// GET: Admin
		public async Task<IActionResult> ManagerList()
		{
			//Так читабельниее
			if(User.IsInRole(role: AuthorizationConstants.Roles.ADMINISTRATORS))
			{
				var _users = await _userManager.Users.ToListAsync();
				var UserVM = new UserViewModel
				{
					AppUsers = _users,
					ManagerUsers = new List<ApplicationUser>()
				};

				foreach(var user in UserVM.AppUsers)
				{
					if(await _userManager.IsInRoleAsync(user: user, role: "Manager"))
						UserVM.ManagerUsers.Add(item: user);
				}

				return View(model: UserVM);
			}

			return NotFound();
		}

		// GET: Admin/Details/5
		public async Task<IActionResult> ManagerDetails(string id, ApplicationUser applicationUser)
		{
			var user = await _userManager.FindByIdAsync(userId:id);
			if (user == null)
			{
				return NotFound();
			}

			var photo = (from m in _context.FileModel
						where m.Id == applicationUser.Id
						select m.Name).FirstOrDefault();



			var managerDetailsVm = new EditUserViewModel
			{
				UserName = user.UserName,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email,
				PhotoPath = photo
			};
			

			return View(managerDetailsVm);
		} 

		// GET: Admin/Create
		public ActionResult AddManager() => View();

	

		// POST: Admin/Create
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> AddManager([Bind("Id,UserName,Email,PhoneNumber")]
													ApplicationUser applicationUser, IFormFile uploadedFile)
		{
			
			var user = new ApplicationUser
			{
				Id = applicationUser.Id,
				UserName = applicationUser.UserName,
				Email = applicationUser.Email,
				PhoneNumber = applicationUser.PhoneNumber
			};

			await _userManager.CreateAsync(user: user, password: AuthorizationConstants.DEFAULT_PASSWORD);
			await _userManager.AddToRoleAsync(user: user, role: AuthorizationConstants.Roles.MANAGER);
			//Getting new users Id
			//Saving file, which we get, of created user
			if (uploadedFile != null)
			{
				// путь к папке Files
				string path = "/Files/" + uploadedFile.FileName;
				// сохраняем файл в папку Files в каталоге wwwroot
				using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
				{
					await uploadedFile.CopyToAsync(fileStream);
				}
				FileModel file = new FileModel {Id = applicationUser.Id, Name = uploadedFile.FileName, Path = path };
				_context.FileModel.Add(file);
				_context.SaveChanges();
			}
		


			return RedirectToAction(actionName: nameof(ManagerList));
		}

		// GET: Admin/Edit/5
		public async Task<IActionResult> EditManager(string id)
		{
			var user = await _userManager.FindByIdAsync(userId: id);
			if(user == null) return NotFound();

			var model = new EditUserViewModel
			{
				Id= user.Id,
				UserName = user.UserName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
			};
			return View(model: model);
		}

		// POST: Admin/Edit/5
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> EditManager(EditUserViewModel applicationUser, IFormFile uploadedFile)
		{
			if(ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(userId: applicationUser.Id);
				if(user != null)
				{
					user.Id = applicationUser.Id;
					user.Email = applicationUser.Email;
					user.UserName = applicationUser.UserName;
					user.PhoneNumber = applicationUser.PhoneNumber;

					var photoId = (from m in _context.FileModel
								 where m.Id == applicationUser.Id
								 select m.Id).FirstOrDefault();

					var result = await _userManager.UpdateAsync(user: user);
					if (uploadedFile != null)
					{
						// путь к папке Files
						string path = "/Files/" + uploadedFile.FileName;
						// сохраняем файл в папку Files в каталоге wwwroot
						using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
						{
							await uploadedFile.CopyToAsync(fileStream);
						}
						FileModel file = new FileModel {Id = user.Id, Name = uploadedFile.FileName, Path = path };
						if (photoId == file.Id)
						{
							_context.FileModel.Update(file);	
						}
						else
						{
							_context.FileModel.Add(file);

						}
						
						_context.SaveChanges();
					}
					if(result.Succeeded)
						return RedirectToAction(actionName: "ManagerList");

					foreach(var error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
				}
			}

			return View(model: applicationUser);
		}

		// GET: Admin/Delete/5
		public async Task<IActionResult> DeleteManager(string id)
		{
			if(id == null) return NotFound();

			var user = await _userManager.FindByIdAsync(userId: id);
			if(user == null) return NotFound();

			return View(model: user);
		}

		// POST: Admin/Delete/5
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteManager(string id, IFormCollection collection)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId: id);
				await _userManager.DeleteAsync(user: user);
				return RedirectToAction(actionName: nameof(ManagerList));
			} catch {
				return View();
			}
		}
		
		
	}
}
