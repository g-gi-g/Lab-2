using System.ComponentModel.DataAnnotations;

namespace ScientificWorksArchive.Models;

public class ResearcherInput
{ 
    public int Id { get; set; }

    [Required(ErrorMessage = "'Name' field shouldn't be empty")]
    public string Name { get; set; } = null!;

    public int ResearcherStatusId { get; set; }

    [Required(ErrorMessage = "'Salary' field shouldn't be empty")]
    [Range(0,1000000, ErrorMessage = "Salary must be in range from 0 to 1000000")]
    public int Salary { get; set; }
}
