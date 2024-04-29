namespace ScientificWorksArchive.Data;

public class ResearcherStatus
{
    public ResearcherStatus() 
    {
        Researchers = new List<Researcher>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Researcher> Researchers { get; set; }
}
