using System.ComponentModel.DataAnnotations;

namespace ScientificWorksArchive.Data;

public class Researcher
{
    public Researcher()
    {
        ProjectResearcherWorks = new List<ProjectResearcherWork>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "Поле 'Ім'я' не може бути порожнє")]
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    public int ResearcherStatusId { get; set; }

    [Required(ErrorMessage = "Поле 'Зарплата' не може бути порожнє")]
    [Display(Name = "Зарплата")]
    [Range(0,1000000, ErrorMessage = "Зарплата не мое бути від'ємною або перебільшувати 1000000")]
    public int Salary { get; set; }

    public virtual ResearcherStatus ResearcherStatus { get; set; } = null!;

    public virtual ICollection<ProjectResearcherWork> ProjectResearcherWorks { get; set; }
}
