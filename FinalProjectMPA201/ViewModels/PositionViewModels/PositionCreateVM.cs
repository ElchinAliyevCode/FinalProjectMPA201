using System.ComponentModel.DataAnnotations;

namespace FinalProjectMPA201.ViewModels.PositionViewModels;

public class PositionCreateVM
{
    [Required, MaxLength(256)]
    public string Name { get; set; }
}
