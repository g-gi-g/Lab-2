using System.ComponentModel.DataAnnotations;

namespace ScientificWorksArchive.Models;

public class ScientificWorkInput
{
    public int Id { get; set; }

    [Required(ErrorMessage = "'Name' field shouldn't be empty")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Scientific work must be added")]
    [Display(Name = "Paper")]
    public IFormFile WorkFile { get; set; } = null!;

    [Required(ErrorMessage = "Date must be set")]
    [Display(Name = "Registration date")]
    public DateOnly RegistrationDate { get; set; }
}
