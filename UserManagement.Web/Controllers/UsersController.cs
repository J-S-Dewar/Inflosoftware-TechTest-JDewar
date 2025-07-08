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

    //Add new query string param with default
    public ViewResult List(string status = "all")
    {
        //Get all users
        IEnumerable<User> users;

        //Filter users based on value of 'status'
        if (status == "active")
        {
            users = _userService.FilterByActive(true);
        }
        else if (status == "inactive")
        {
            users = users = _userService.FilterByActive(false);
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
            Items = items.ToList()
        };

        return View(model);
    }
}
