using System;
using Microsoft.AspNetCore.Mvc;
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

    // Test that the List action returns only users marked as active when filtering by "active" status
    [Fact]
    public void List_WithActiveStatus_ReturnsOnlyActiveUsers()
    {
        // Arrange: create the controller and set up test users who are active
        var controller = CreateController();
        SetupUsers(isActive: true);

        // Act: call the List action with the "active" status filter
        var result = controller.List("active");

        // Assert: check that the returned model contains only users where IsActive is true
        var model = result.Model as UserListViewModel;
        model!.Items.Should().OnlyContain(u => u.IsActive);
    }

    // Test that the List action returns only users marked as inactive when filtering by "inactive" status
    [Fact]
    public void List_WithInactiveStatus_ReturnsOnlyInactiveUsers()
    {
        // Arrange: create the controller and set up test users who are inactive
        var controller = CreateController();
        SetupUsers(isActive: false);

        // Act: call the List action with the "inactive" status filter
        var result = controller.List("inactive");

        // Assert: check that the returned model contains only users where IsActive is false
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
