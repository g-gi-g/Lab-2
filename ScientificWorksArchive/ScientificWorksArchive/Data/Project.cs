using System.ComponentModel.DataAnnotations;

namespace ScientificWorksArchive.Data;

public class Project
{
    public Project()
    {
        ProjectResearcherWorks = new List<ProjectResearcherWork>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "'Project name' field shouldn't be empty")]
    [Display(Name = "Project name")]
    public string ProjectName { get; set; } = null!;

    public int ProjectStatusId { get; set; }

    [Display(Name = "Project description")]
    public string? ProjectDescription { get; set; }

    public virtual ProjectStatus ProjectStatus { get; set; } = null!;

    public virtual ICollection<ProjectResearcherWork> ProjectResearcherWorks { get; set; }
}
