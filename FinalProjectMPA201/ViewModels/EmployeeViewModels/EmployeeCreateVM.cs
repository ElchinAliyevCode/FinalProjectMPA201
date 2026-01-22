using System.ComponentModel.DataAnnotations;

namespace FinalProjectMPA201.ViewModels.EmployeeViewModels;

public class EmployeeCreateVM
{
    [Required, MaxLength(256)]
    public string Name { get; set; }
    [Required]
    public int PositionId { get; set; }
    [Required]
    public IFormFile Image { get; set; }
}
