using System.ComponentModel.DataAnnotations;

namespace FinalProjectMPA201.ViewModels.AcoountViewModels;

public class RegisterVM
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
