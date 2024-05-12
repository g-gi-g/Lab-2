using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScientificWorksArchive.Data;
using ScientificWorksArchive.Models;

namespace ScientificWorksArchive.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly ScientificWorksArchiveAPIContext _context;

    public ProjectsController(ScientificWorksArchiveAPIContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        return await _context.Projects.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        return project;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProject(int id, ProjectInput projectInput)
    {
        if (id != projectInput.Id)
        {
            return BadRequest();
        }

        Project project = new Project
        {
            Id = projectInput.Id,
            ProjectName = projectInput.ProjectName,
            ProjectDescription = projectInput.ProjectDescription,
            ProjectStatusId = projectInput.ProjectStatusId
        };

        _context.Entry(project).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProjectExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Project>> PostProject(ProjectInput projectInput)
    {
        Project project = new Project
        {
            ProjectName = projectInput.ProjectName,
            ProjectDescription = projectInput.ProjectDescription,
            ProjectStatusId = projectInput.ProjectStatusId
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProject", new { id = project.Id }, project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.Id == id);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<ScientificWork>>> GetProjectWorks(int id)
    {
        if (!ProjectExists(id))
        {
            return NotFound();
        }

        var relations = _context.ProjectResearcherWorks.Include(rel => rel.ScientificWork);

        var works = relations.Where(rel => rel.ProjectId == id).Select(rel => rel.ScientificWork);

        return await works.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Researcher>>> GetProjectResearchers(int id)
    {
        if (!ProjectExists(id))
        {
            return NotFound();
        }

        var relations = _context.ProjectResearcherWorks.Include(rel => rel.Researcher);

        var researchers = relations.Where(rel => rel.ProjectId == id).Select(rel => rel.Researcher);

        return await researchers.ToListAsync();
    }
}
