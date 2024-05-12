using System.ComponentModel.DataAnnotations;

namespace ScientificWorksArchive.Models;

public class ProjectInput
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле 'Ім'я проєкту' не може бути порожнє")]
    [Display(Name = "Ім'я проєкту")]
    public string ProjectName { get; set; } = null!;

    public int ProjectStatusId { get; set; }

    [Display(Name = "Опис")]
    public string? ProjectDescription { get; set; }
}
