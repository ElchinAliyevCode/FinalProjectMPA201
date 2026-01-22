using System.ComponentModel.DataAnnotations;

namespace FinalProjectMPA201.ViewModels.PositionViewModels;

public class PositionUpdateVM
{
    public int Id { get; set; }
    [Required, MaxLength(256)]
    public string Name { get; set; }
}
