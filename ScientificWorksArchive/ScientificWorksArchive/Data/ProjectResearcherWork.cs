namespace ScientificWorksArchive.Data;

public class ProjectResearcherWork
{
    public int Id { get; set; }

    public int ResearcherId { get; set; }

    public int ProjectId { get; set; }

    public int ScientificWorkId { get; set; }

    public virtual Researcher Researcher { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual ScientificWork ScientificWork { get; set; } = null!;
}
