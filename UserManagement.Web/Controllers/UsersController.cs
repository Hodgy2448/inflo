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

    // OR filter in controller below

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
}
