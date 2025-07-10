using System.Linq;
using System.Reflection.Metadata.Ecma335;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    // Displays a filtered list of users based on their active status ("all", "active", or "inactive").
    // Converts User entities to UserListItemViewModel for the view.
    [HttpGet("list")]
    public ViewResult List(string status = "all")
    {

        IEnumerable<User> users;

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

    // Retrieves and displays details for a specific user by ID.
    // Returns a "NotFound" view if the user does not exist. 
    [HttpGet("details/{id}")]

    public ViewResult Details(long id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return View("NotFound");
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(model);
    }

    // Retrieves a user for deletion confirmation by ID.
    // Returns "NotFound" view if user does not exist.
    // Deletes the user with the specified ID and redirects to the list with a success message.
    [HttpGet("Delete/{id}")]
    public IActionResult Delete(long id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return View("NotFound");
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(model);
    }

    [HttpPost("Delete/{id}")]
    public IActionResult DeleteConfirmed(long id)
    {
        _userService.DeleteById(id);
        TempData["Message"] = "User deleted!";
        return Redirect("/users/list");
    }

    // Retrieves a user by ID for editing.
    // Returns "NotFound" view if user does not exist.
    // Accepts the edited user data, validates, updates the user, then redirects to the list with a confirmation message.
    [HttpGet("Edit/{id}")]
    public ViewResult Edit(long id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return View("NotFound");
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(model);
    }


    [HttpPost("SubmitEdit/{id}")]
    public IActionResult SubmitEdit(long id, UserListItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (id != model.Id)
        {
            return BadRequest();
        }

        var userToUpdate = new User
        {
            Id = model.Id,
            Forename = model.Forename!,
            Surname = model.Surname!,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };

        _userService.EditUser(userToUpdate, id);
        TempData["Message"] = "User updated!";
        return RedirectToAction("List");
    }

    // Displays the Add User form with an empty model.
    // Processes the Add User form submission, validates, creates a new user, and redirects with a success message.
    [HttpGet("Add")]
    public ViewResult Add()
    {
        var model = new UserListItemViewModel();
        return View(model);
    }

    [HttpPost("SubmitAdd")]
    public IActionResult SubmitAdd(UserListItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var newUser = new User
        {
            Forename = model.Forename!,
            Surname = model.Surname!,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };

        _userService.AddUser(newUser);
        TempData["Message"] = "New user added!";
        return RedirectToAction("List");
    }
}
