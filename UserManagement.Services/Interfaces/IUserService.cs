using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> FilterByActive(bool isActive);

    // Added new method signatures to support user creation, editing and deletion
    IEnumerable<User> GetAll();
    User? GetById(long id);
    void DeleteById(long id);
    void EditUser(User user, long id);
    void AddUser(User user);
}
