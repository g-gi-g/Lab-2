using System.ComponentModel.DataAnnotations;

namespace ScientificWorksArchive.Data;

public class Project
{
    public Project()
    {
        ProjectResearcherWorks = new List<ProjectResearcherWork>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "Поле 'Ім'я проєкту' не може бути порожнє")]
    [Display(Name = "Ім'я проєкту")]
    public string ProjectName { get; set; } = null!;

    public int ProjectStatusId { get; set; }

    [Display(Name = "Опис")]
    public string? ProjectDescription { get; set; }

    public virtual ProjectStatus ProjectStatus { get; set; } = null!;

    public virtual ICollection<ProjectResearcherWork> ProjectResearcherWorks { get; set; }
}
