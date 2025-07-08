using System;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    //Additional tests to return only active or only inactive users
    [Fact]
    public void List_WithActiveStatus_ReturnsOnlyActiveUsers()
    {
        // Arrange
        var controller = CreateController();
        SetupUsers(isActive: true);

        // Act
        var result = controller.List("active");

        // Assert
        var model = result.Model as UserListViewModel;
        model!.Items.Should().OnlyContain(u => u.IsActive);
    }

    [Fact]
    public void List_WithInactiveStatus_ReturnsOnlyInactiveUsers()
    {
        // Arrange
        var controller = CreateController();
        SetupUsers(isActive: false);

        // Act
        var result = controller.List("inactive");

        // Assert
        var model = result.Model as UserListViewModel;
        model!.Items.Should().OnlyContain(u => !u.IsActive);
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true, DateTime? dateOfBirth = null)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = dateOfBirth
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
