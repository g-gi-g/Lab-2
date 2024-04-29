using Microsoft.EntityFrameworkCore;

namespace ScientificWorksArchive.Data;

public class ScientificWorksArchiveAPIContext : DbContext
{
    public ScientificWorksArchiveAPIContext(DbContextOptions<ScientificWorksArchiveAPIContext> options)
    : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Researcher> Researchers { get; set; }

    public virtual DbSet<ScientificWork> ScientificWorks { get; set; }

    public virtual DbSet<ProjectResearcherWork> ProjectResearcherWorks { get; set; }

    public virtual DbSet<ResearcherStatus> ResearcherStatuses { get; set; }

    public virtual DbSet<ProjectStatus> ProjectStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.ProjectName).HasMaxLength(100);
            entity.Property(e => e.ProjectDescription).HasMaxLength(200);

            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.ProjectStatus).WithMany(p => p.Projects)
            .HasForeignKey(d => d.ProjectStatusId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Projects_ProjectStatuses");
        });

        modelBuilder.Entity<Researcher>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasKey(e => e.Id).HasName("PK_Researchers");

            entity.HasOne(d => d.ResearcherStatus).WithMany(p => p.Researchers)
            .HasForeignKey(d => d.ResearcherStatusId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Researchers_ResearchersStatuses");
        });

        modelBuilder.Entity<ScientificWork>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(200);

            entity.HasKey(e => e.Id).HasName("PK_ScientificWorks");
        });

        modelBuilder.Entity<ResearcherStatus>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(200);

            entity.HasKey(e => e.Id).HasName("PK_ResearcherStatuses");
        });

        modelBuilder.Entity<ProjectStatus>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(200);

            entity.HasKey(e => e.Id).HasName("PK_ProjectStatuses");
        });

        modelBuilder.Entity<ProjectResearcherWork>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProjectResearcherWorks");

            entity.HasOne(d => d.Researcher).WithMany(p => p.ProjectResearcherWorks)
            .HasForeignKey(d => d.ResearcherId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ProjectResearcherWorks_Researchers");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectResearcherWorks)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ProjectResearcherWorks_Projects");

            entity.HasOne(d => d.ScientificWork).WithMany(p => p.ProjectResearcherWorks)
            .HasForeignKey(d => d.ScientificWorkId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ProjectResearcherWorks_ScientificWorks");
        });
    }
}
