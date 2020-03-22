using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;

using Infrastructure.Identity;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Web.Commands;
using Web.Extension;
using Web.ViewModels.Clients;

namespace Web.Controllers
{
  [Authorize(Roles = AuthorizationConstants.Roles.MANAGER)]
  public class ClientsController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private IAsyncRepository<Client> Repository { get; }
    private IAsyncRepository<LegalPerson> LegalRepository { get; }
    private IAsyncRepository<PhysicalPerson> PhysicalPerson { get; }
    private IMediator Mediator { get; }

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
      List<Client> clients = await Repository.GetAll();
      List<PhysicalPerson> physicalClients = await PhysicalPerson.GetAll();
      List<LegalPerson> legalClients = await LegalRepository.GetAll();

      return View(model: clients);
    }

    private bool ClientExists(int id)
    {
      return Repository.GetAll().Result.Any(predicate: e => e.Id == id);
    }

    #region Create

    // GET: Clients/CreateClient
    public IActionResult CreateClient() => View();

    // POST: Clients/CreateClient
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateClient(ClientCreateViewModel clientCreateViewModel)
    {
      if(!ModelState.IsValid)
        return View(model: clientCreateViewModel);

      IdentityResult result = await Mediator.Send(request: new GetPasswordValidationQuery(user: null, password: clientCreateViewModel.Password));
      if(result.Succeeded)
      {
        HttpContext.Session.Set(key: "NewClientData", value: clientCreateViewModel.Client);
        HttpContext.Session.Set(key: "PassClient", value: clientCreateViewModel.Password);

        //if pass is valid
        if(clientCreateViewModel.IsPhysicalPerson)
        {
          //PhysicalPerson
          return RedirectToAction(actionName: nameof(CreatePhysicalPerson), routeValues: clientCreateViewModel);
        }

        //LegalPerson
        return RedirectToAction(actionName: nameof(CreateLegalPerson), routeValues: clientCreateViewModel);
      }
      foreach(IdentityError error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);

      return View(model: clientCreateViewModel);
    }

    #endregion

    #region CreatePhysicalPerson

    [HttpGet]
    public IActionResult CreatePhysicalPerson(ClientCreateViewModel clientCreateViewModel)
    {
      var client = HttpContext.Session.Get<Client>(key: "NewClientData");

      var physicalPersonCreateViewModel = new PhysicalPersonViewModel
      {
        Client = client
      };

      return View(model: physicalPersonCreateViewModel);
    }

    // POST: Clients/CreatePhysicalPerson
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePhysicalPerson(PhysicalPersonViewModel physicalPersonViewModel)
    {
      if(ModelState.IsValid)
      {
        Client client = physicalPersonViewModel.Client;
        client.PhysicalPerson = physicalPersonViewModel.PhysicalPerson;

        //Add to tables Client and PhysicalPerson
        await Repository.AddAsync(entity: client);

        //Add new User to Identity
        var user = new ApplicationUser
        {
          UserName = physicalPersonViewModel.Client.Login,
          Email = physicalPersonViewModel.Client.Login,
          PhoneNumber = physicalPersonViewModel.Client.TelNumber
        };

        var password = HttpContext.Session.Get<string>(key: "PassClient");
        IdentityResult result = await _userManager.CreateAsync(user: user, password: password);

        if(result.Succeeded)
        {
          //Set Roles CLIENT to new User
          await _userManager.AddToRoleAsync(user: user, role: AuthorizationConstants.Roles.CLIENT);

          return RedirectToAction(actionName: nameof(Index));
        }
        foreach(IdentityError error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
      }

      return View(model: physicalPersonViewModel);
    }

    #endregion

    #region CreateLegalPerson

    [HttpGet]
    public IActionResult CreateLegalPerson(ClientCreateViewModel clientCreateViewModel)
    {
      var client = HttpContext.Session.Get<Client>(key: "NewClientData");

      var legalPersonCreateViewModel = new LegalPersonViewModel
      {
        Client = client
      };

      return View(model: legalPersonCreateViewModel);
    }

    // POST: Clients/CreatePhysicalPerson
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLegalPerson(LegalPersonViewModel legalPersonViewModel)
    {
      if(ModelState.IsValid)
      {
        Client client = legalPersonViewModel.Client;
        client.LegalPerson = legalPersonViewModel.LegalPerson;

        //Add to tables Client and LegalPerson
        await Repository.AddAsync(entity: client);

        //Add new User to Identity
        var user = new ApplicationUser
        {
          UserName = legalPersonViewModel.Client.Login,
          Email = legalPersonViewModel.Client.Login,
          PhoneNumber = legalPersonViewModel.Client.TelNumber
        };

        var password = HttpContext.Session.Get<string>(key: "PassClient");
        IdentityResult result = await _userManager.CreateAsync(user: user, password: password);

        if(result.Succeeded)
        {
          //Set Roles CLIENT to new User
          await _userManager.AddToRoleAsync(user: user, role: AuthorizationConstants.Roles.CLIENT);

          return RedirectToAction(actionName: nameof(Index));
        }
        foreach(IdentityError error in result.Errors) ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
      }

      return View(model: legalPersonViewModel);
    }

    #endregion

    #region Edit

    // GET: Clients/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
      List<PhysicalPerson> physicalClients = await PhysicalPerson.GetAll();
      List<LegalPerson> legalClients = await LegalRepository.GetAll();

      Client client = await Repository.GetById(id: id);

      if(client == null)
        return NotFound();

      ApplicationUser user = await _userManager.FindByNameAsync(userName: client.Login);

      HttpContext.Session.Set(key: "IdIdentity", value: user.Id);

      if(client.PhysicalPerson != null)
      {
        return View(viewName: "EditPhysicalPerson", model: new PhysicalPersonViewModel
        {
          Client = client,
          PhysicalPerson = client.PhysicalPerson
        });
      }
      return View(viewName: "EditLegalPerson", model: new LegalPersonViewModel
      {
        Client = client,
        LegalPerson = client.LegalPerson
      });
    }

    // POST: Clients/EditPhysicalPerson/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPhysicalPerson(PhysicalPersonViewModel physicalPersonViewModel)
    {
      if(ModelState.IsValid)
      {
        try
        {
          Client client = physicalPersonViewModel.Client;
          client.PhysicalPerson = physicalPersonViewModel.PhysicalPerson;

          await Repository.UpdateAsync(entity: client);

          var userId = HttpContext.Session.Get<string>(key: "IdIdentity");

          ApplicationUser user = await _userManager.FindByIdAsync(userId: userId);

          user.UserName = physicalPersonViewModel.Client.Login;
          user.Email = physicalPersonViewModel.Client.Login;
          user.PhoneNumber = physicalPersonViewModel.Client.TelNumber;

          await _userManager.UpdateAsync(user: user);
        } catch(DbUpdateConcurrencyException)
        {
          if(!ClientExists(id: physicalPersonViewModel.Client.Id))
            return NotFound();

          throw;
        }

        return RedirectToAction(actionName: nameof(Index));
      }

      return View(model: physicalPersonViewModel);
    }

    // POST: Clients/EditPhysicalPerson/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLegalPerson(LegalPersonViewModel legalPersonViewModel)
    {
      if(ModelState.IsValid)
      {
        try
        {
          Client client = legalPersonViewModel.Client;
          client.LegalPerson = legalPersonViewModel.LegalPerson;

          await Repository.UpdateAsync(entity: client);

          var userId = HttpContext.Session.Get<string>(key: "IdIdentity");

          ApplicationUser user = await _userManager.FindByIdAsync(userId: userId);

          user.UserName = legalPersonViewModel.Client.Login;
          user.Email = legalPersonViewModel.Client.Login;
          user.PhoneNumber = legalPersonViewModel.Client.TelNumber;

          await _userManager.UpdateAsync(user: user);
        } catch(DbUpdateConcurrencyException)
        {
          if(!ClientExists(id: legalPersonViewModel.Client.Id))
            return NotFound();

          throw;
        }

        return RedirectToAction(actionName: nameof(Index));
      }

      return View(model: legalPersonViewModel);
    }

    #endregion

    #region Delete

    // GET: Clients/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
      List<PhysicalPerson> physicalClients = await PhysicalPerson.GetAll();
      List<LegalPerson> legalClients = await LegalRepository.GetAll();

      Client client = await Repository.GetById(id: id);

      if(client == null)
        return NotFound();

      ApplicationUser user = await _userManager.FindByNameAsync(userName: client.Login);

      HttpContext.Session.Set(key: "IdIdentity", value: user.Id);

      if(client.PhysicalPerson != null)
      {
        return View(viewName: "DeletePhysicalPerson", model: new PhysicalPersonViewModel
        {
          Client = client,
          PhysicalPerson = client.PhysicalPerson
        });
      }
      return View(viewName: "DeleteLegalPerson", model: new LegalPersonViewModel
      {
        Client = client,
        LegalPerson = client.LegalPerson
      });
    }

    // POST: Clients/Delete/5
    [HttpPost, ActionName(name: "Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      //Delete client from Clients and (PhysicalPerson or LegalPerson)
      Client client = await Repository.GetById(id: id);
      await Repository.DeleteAsync(entity: client);

      //Delete user from Identity
      var userId = HttpContext.Session.Get<string>(key: "IdIdentity");
      ApplicationUser user = await _userManager.FindByIdAsync(userId: userId);
      await _userManager.DeleteAsync(user: user);

      return RedirectToAction(actionName: nameof(Index));
    }

    #endregion
  }
}
