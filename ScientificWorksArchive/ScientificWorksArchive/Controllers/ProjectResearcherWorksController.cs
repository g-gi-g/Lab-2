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

[Route("api/[controller]")]
[ApiController]
public class ProjectResearcherWorksController : ControllerBase
{
    private readonly ScientificWorksArchiveAPIContext _context;

    public ProjectResearcherWorksController(ScientificWorksArchiveAPIContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectResearcherWork>>> GetProjectResearcherWorks()
    {
        return await _context.ProjectResearcherWorks.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectResearcherWork>> GetProjectResearcherWork(int id)
    {
        var projectResearcherWork = await _context.ProjectResearcherWorks.FindAsync(id);

        if (projectResearcherWork == null)
        {
            return NotFound();
        }

        return projectResearcherWork;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProjectResearcherWork(int id, ProjectResearcherWorkInput projectResearcherWorkInput)
    {
        if (id != projectResearcherWorkInput.Id)
        {
            return BadRequest();
        }

        ProjectResearcherWork projectResearcherWork = new ProjectResearcherWork 
        {
            Id = id,
            ProjectId = projectResearcherWorkInput.ProjectId,
            ResearcherId = projectResearcherWorkInput.ResearcherId,
            ScientificWorkId = projectResearcherWorkInput.ScientificWorkId
        };

        _context.Entry(projectResearcherWork).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProjectResearcherWorkExists(id))
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
    public async Task<ActionResult<ProjectResearcherWork>> PostProjectResearcherWork(ProjectResearcherWorkInput projectResearcherWorkInput)
    {
        ProjectResearcherWork projectResearcherWork = new ProjectResearcherWork
        {
            ProjectId = projectResearcherWorkInput.ProjectId,
            ResearcherId = projectResearcherWorkInput.ResearcherId,
            ScientificWorkId = projectResearcherWorkInput.ScientificWorkId
        };

        _context.ProjectResearcherWorks.Add(projectResearcherWork);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProjectResearcherWork", new { id = projectResearcherWork.Id }, projectResearcherWork);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProjectResearcherWork(int id)
    {
        var projectResearcherWork = await _context.ProjectResearcherWorks.FindAsync(id);
        if (projectResearcherWork == null)
        {
            return NotFound();
        }

        _context.ProjectResearcherWorks.Remove(projectResearcherWork);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ProjectResearcherWorkExists(int id)
    {
        return await _context.ProjectResearcherWorks.AnyAsync(e => e.Id == id);
    }
}
