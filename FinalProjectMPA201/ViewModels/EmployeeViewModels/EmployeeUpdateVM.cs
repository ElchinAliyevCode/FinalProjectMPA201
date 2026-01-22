using System.ComponentModel.DataAnnotations;

namespace FinalProjectMPA201.ViewModels.EmployeeViewModels;

public class EmployeeUpdateVM
{
    public int Id { get; set; }
    [Required, MaxLength(256)]
    public string Name { get; set; }
    [Required]
    public int PositionId { get; set; }
    public IFormFile? Image { get; set; }
}
