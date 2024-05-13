using System.ComponentModel.DataAnnotations;

namespace ScientificWorksArchive.Models;

public class ProjectInput
{
    public int Id { get; set; }

    [Required(ErrorMessage = "'Project name' field shouldn't be empty")]
    [Display(Name = "Project name")]
    public string ProjectName { get; set; } = null!;

    public int ProjectStatusId { get; set; }

    [Display(Name = "Project description")]
    public string? ProjectDescription { get; set; }
}
