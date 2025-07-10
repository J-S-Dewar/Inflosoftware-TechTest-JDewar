using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;


namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>

    // Retrieves users filtered by active status; used by the UsersController's List action.
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        return _dataAccess.GetAll<User>().Where(User => User.IsActive == isActive);
    }
    
    // Gets a single user by ID for use in UsersController methods: Details, Delete, and Edit.
    public User? GetById(long id)
    {
        return _dataAccess.GetAll<User>().FirstOrDefault(user => user.Id == id);
    }

    // Removes a user identified by ID, used by the Delete method in UsersController.
    public void DeleteById(long id)
    {
        var user = _dataAccess.GetAll<User>().FirstOrDefault(user => user.Id == id);
        if (user != null)
        {
            _dataAccess.Delete(user);
        }
    }

    // Updates an existing user's details based on the provided ID; used by the Edit action in UsersController.
    public void EditUser(User user, long id)
    {
        var uneditedUser = _dataAccess.GetAll<User>().FirstOrDefault(u => u.Id == id);
        if (uneditedUser == null) return;

        uneditedUser.Forename = user.Forename;
        uneditedUser.Surname = user.Surname;
        uneditedUser.Email = user.Email;
        uneditedUser.IsActive = user.IsActive;
        uneditedUser.DateOfBirth = user.DateOfBirth;

        _dataAccess.Update(uneditedUser);
    }
    
    // Adds a new user to the data store; called by the Add action in UsersController.
    public void AddUser(User user)
    {
        _dataAccess.Create(user);
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
}
