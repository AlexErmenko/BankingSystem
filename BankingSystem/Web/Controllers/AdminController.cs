﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Specifications;

using Infrastructure.Identity;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Web.Commands;
using Web.ViewModels.Admin;

namespace Web.Controllers
{
  [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
  public class AdminController : Controller
  {
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private IMediator Mediator { get; }

    public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment appEnvironment, IMediator mediator)
    {
      _userManager = userManager;
      _context = context;
      _appEnvironment = appEnvironment;
      Mediator = mediator;
    }

    #region ManagerList

    // GET: Admin
    //TODO: Этот оставить
    public async Task<IActionResult> ManagerList()
    {
      IList<ApplicationUser> managers = await _userManager.GetUsersInRoleAsync(roleName: AuthorizationConstants.Roles.MANAGER);
      return View(model: managers);
    }

    #endregion

    #region Details

    // GET: Admin/Details/5
    public async Task<IActionResult> ManagerDetails(string id, ApplicationUser applicationUser)
    {
      ApplicationUser user = await _userManager.FindByIdAsync(userId: id);
      if(user == null) return NotFound();

      string photo = ( from m in _context.FileModel where m.Id == applicationUser.Id select m.Name ).FirstOrDefault();

      var managerDetailsVm = new EditUserViewModel
      {
        UserName = user.UserName,
        PhoneNumber = user.PhoneNumber,
        Email = user.Email,
        PhotoPath = photo
      };

      return View(model: managerDetailsVm);
    }

    #endregion

    #region AddManager

    // GET: Admin/Create
    public ActionResult AddManager() => View();

    // POST: Admin/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddManager(ApplicationUser applicationUser, IFormFile uploadedFile, string password, string password_confirm)
    {
      var user = new ApplicationUser
      {
        Id = applicationUser.Id,
        UserName = applicationUser.UserName,
        Email = applicationUser.Email,
        PhoneNumber = applicationUser.PhoneNumber
      };
      IdentityResult result = await Mediator.Send(request: new GetPasswordValidationQuery(user: null, password: password));
      if(result.Succeeded
         && password == password_confirm)
      {
        await _userManager.CreateAsync(user: user, password: password);
        await _userManager.AddToRoleAsync(user: user, role: AuthorizationConstants.Roles.MANAGER);
        if(uploadedFile != null)
        {
          // путь к папке Files
          string path = "/Files/" + uploadedFile.FileName;

          // сохраняем файл в папку Files в каталоге wwwroot
          using(var fileStream = new FileStream(path: _appEnvironment.WebRootPath + path, mode: FileMode.Create)) await uploadedFile.CopyToAsync(target: fileStream);
          var file = new FileModel
          {
            Id = applicationUser.Id,
            Name = uploadedFile.FileName,
            Path = path
          };
          _context.FileModel.Add(entity: file);
          _context.SaveChanges();
        }

        return RedirectToAction(actionName: nameof(ManagerList));
      }
      foreach(IdentityError error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);

      return View(model: applicationUser);

      //Getting new users Id
      //Saving file, which we get, of created user
    }

    #endregion

    #region EditManager

    // GET: Admin/Edit/5
    public async Task<IActionResult> EditManager(string id)
    {
      ApplicationUser user = await _userManager.FindByIdAsync(userId: id);
      if(user == null) return NotFound();

      var model = new EditUserViewModel
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber
      };
      return View(model: model);
    }

    // POST: Admin/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditManager(EditUserViewModel applicationUser, IFormFile uploadedFile)
    {
      if(ModelState.IsValid)
      {
        ApplicationUser user = await _userManager.FindByIdAsync(userId: applicationUser.Id);
        if(user != null)
        {
          user.Id = applicationUser.Id;
          user.Email = applicationUser.Email;
          user.UserName = applicationUser.UserName;
          user.PhoneNumber = applicationUser.PhoneNumber;

          string photoId = ( from m in _context.FileModel where m.Id == applicationUser.Id select m.Id ).FirstOrDefault();

          IdentityResult result = await _userManager.UpdateAsync(user: user);
          if(uploadedFile != null)
          {
            // путь к папке Files
            string path = "/Files/" + uploadedFile.FileName;

            // сохраняем файл в папку Files в каталоге wwwroot
            using(var fileStream = new FileStream(path: _appEnvironment.WebRootPath + path, mode: FileMode.Create)) await uploadedFile.CopyToAsync(target: fileStream);
            var file = new FileModel
            {
              Id = user.Id,
              Name = uploadedFile.FileName,
              Path = path
            };
            if(photoId == file.Id) _context.FileModel.Update(entity: file);
            else _context.FileModel.Add(entity: file);

            _context.SaveChanges();
          }
          if(result.Succeeded)
            return RedirectToAction(actionName: "ManagerList");

          foreach(IdentityError error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
        }
      }

      return View(model: applicationUser);
    }

    #endregion

    #region DeleteManager

    // GET: Admin/Delete/5
    public async Task<IActionResult> DeleteManager(string id)
    {
      if(id == null) return NotFound();

      ApplicationUser user = await _userManager.FindByIdAsync(userId: id);
      if(user == null) return NotFound();

      return View(model: user);
    }

    // POST: Admin/Delete/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteManager(string id, IFormCollection collection)
    {
      try
      {
        ApplicationUser user = await _userManager.FindByIdAsync(userId: id);
        await _userManager.DeleteAsync(user: user);
        return RedirectToAction(actionName: nameof(ManagerList));
      } catch
      {
        return View();
      }
    }

    #endregion
  }
}
