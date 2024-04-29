using System.ComponentModel.DataAnnotations;

namespace ScientificWorksArchive.Data;

public class ScientificWork
{
    public ScientificWork() 
    {
        ProjectResearcherWorks = new List<ProjectResearcherWork>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "Поле 'Назва' не може бути порожнє")]
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Має бути додана наукова праця")]
    [Display(Name = "Праця")]
    public byte[] WorkFile { get; set; } = null!;

    public DateOnly RegistrationDate { get; set; }

    public virtual ICollection<ProjectResearcherWork> ProjectResearcherWorks { get; set; }
}
