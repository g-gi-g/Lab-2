using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScientificWorksArchive.Data;

namespace ScientificWorksArchive.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProjectStatusesController : ControllerBase
{
    private readonly ScientificWorksArchiveAPIContext _context;

    public ProjectStatusesController(ScientificWorksArchiveAPIContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectStatus>>> GetProjectStatuses()
    {
        return await _context.ProjectStatuses.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectStatus>> GetProjectStatus(int id)
    {
        var projectStatus = await _context.ProjectStatuses.FindAsync(id);

        if (projectStatus == null)
        {
            return NotFound();
        }

        return projectStatus;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProjectStatus(int id, ProjectStatus projectStatus)
    {
        if (id != projectStatus.Id)
        {
            return BadRequest();
        }

        _context.Entry(projectStatus).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProjectStatusExists(id))
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
    public async Task<ActionResult<ProjectStatus>> PostProjectStatus(ProjectStatus projectStatus)
    {
        _context.ProjectStatuses.Add(projectStatus);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProjectStatus", new { id = projectStatus.Id }, projectStatus);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProjectStatus(int id)
    {
        var projectStatus = await _context.ProjectStatuses.FindAsync(id);
        if (projectStatus == null)
        {
            return NotFound();
        }

        _context.ProjectStatuses.Remove(projectStatus);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProjectStatusExists(int id)
    {
        return _context.ProjectStatuses.Any(e => e.Id == id);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjectsWithStatus(int id)
    {
        if (!ProjectStatusExists(id))
        {
            return NotFound();
        }

        var projects = _context.Projects.Where(e => e.ProjectStatusId == id);

        return await projects.ToListAsync();
    }
}
