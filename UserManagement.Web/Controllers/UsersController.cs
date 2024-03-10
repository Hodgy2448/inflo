using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List(bool filter, bool filterByActive)
    {

        IEnumerable<User> users;

        if (filter)
        {
            users = _userService.FilterByActive(filterByActive);
        }
        else
        {
            users = _userService.GetAll();
        }

        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            FilterByActive = filterByActive,
            Items = items.ToList()
        };

        return View(model);

        /* OR filter in controller below */

        // if(filterByActive){
        //     items = items.Where(p => p.IsActive);
        // }

        // var model = new UserListViewModel
        // {
        //     FilterByActive = filterByActive,
        //     Items = items.ToList()
        // };

        // return View(model);

    }
    /* Create new user from _AddUserForm */
    [ActionName("AddUser")]
    public IActionResult AddUserView()
    {
        return PartialView("_AddUserForm");
    }

    [HttpPost, ActionName("AddUser")]
    public IActionResult AddUserAction(UserListItemViewModel model)
    {
        if (ModelState.IsValid)
        {
            var newUser = new User
            {
                Id = 0,
                Forename = model.Forename ?? string.Empty,
                Surname = model.Surname ?? string.Empty,
                Email = model.Email ?? string.Empty,
                IsActive = true, // Set to Active due to using the form?
                DateOfBirth = model.DateOfBirth ?? DateOnly.FromDateTime(DateTime.Now)
            };

            _userService.Create(newUser);

            // Redirect to the list page or refresh the current page
            return RedirectToAction("List", new { filter = false, filterByActive = true });
        }

        // If the model state is not valid, return to the form
        return PartialView("_AddUserForm", model);
    }
}
