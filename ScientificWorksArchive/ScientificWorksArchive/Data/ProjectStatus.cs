namespace ScientificWorksArchive.Data;

public class ProjectStatus
{
    public ProjectStatus() 
    {
        Projects = new List<Project>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; }
}
