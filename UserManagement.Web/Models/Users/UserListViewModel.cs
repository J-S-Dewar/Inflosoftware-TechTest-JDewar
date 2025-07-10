using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    // Marked fields as required and set custom error messages to display 
    // if user leaves them empty
    public long Id { get; set; }
    
    [Required(ErrorMessage = "Please enter a valid forename")]
    public string? Forename { get; set; }

    [Required(ErrorMessage = "Please enter a valid surname")]
    public string? Surname { get; set; }

    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
