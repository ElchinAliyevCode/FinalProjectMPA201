using System.ComponentModel.DataAnnotations;

namespace FinalProjectMPA201.ViewModels.AcoountViewModels;

public class LoginVM
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
