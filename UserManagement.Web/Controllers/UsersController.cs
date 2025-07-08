using System.Linq;
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
        var users = _userService.GetAll();

        //Filter users based on value of 'status'
        if (status == "active")
        {
            users = users.Where(u => u.IsActive);
        }
        else if (status == "inactive")
        {
            users = users.Where(u => !u.IsActive);
        }


        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }
}
