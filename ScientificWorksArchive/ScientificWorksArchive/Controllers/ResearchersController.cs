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
public class ResearchersController : ControllerBase
{
    private readonly ScientificWorksArchiveAPIContext _context;

    public ResearchersController(ScientificWorksArchiveAPIContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Researcher>>> GetResearchers()
    {
        return await _context.Researchers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Researcher>> GetResearcher(int id)
    {
        var researcher = await _context.Researchers.FindAsync(id);

        if (researcher == null)
        {
            return NotFound();
        }

        return researcher;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutResearcher(int id, ResearcherInput researcherInput)
    {
        if (id != researcherInput.Id)
        {
            return BadRequest();
        }

        Researcher researcher = new Researcher
        {
            Id = id,
            Name = researcherInput.Name,
            ResearcherStatusId = researcherInput.ResearcherStatusId,
            Salary = researcherInput.Salary
        };

        _context.Entry(researcher).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ResearcherExists(id))
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
    public async Task<ActionResult<Researcher>> PostResearcher(ResearcherInput researcherInput)
    {
        Researcher researcher = new Researcher 
        {
            Name = researcherInput.Name,
            ResearcherStatusId = researcherInput.ResearcherStatusId,
            Salary = researcherInput.Salary
        };

        _context.Researchers.Add(researcher);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetResearcher", new { id = researcher.Id }, researcher);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResearcher(int id)
    {
        var researcher = await _context.Researchers.FindAsync(id);
        if (researcher == null)
        {
            return NotFound();
        }

        _context.Researchers.Remove(researcher);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ResearcherExists(int id)
    {
        return await _context.Researchers.AnyAsync(e => e.Id == id);
    }

    [HttpGet("{id}/works")]
    public async Task<ActionResult<IEnumerable<ScientificWork>>> GetResearchersWorks(int id)
    {
        if (!await ResearcherExists(id))
        {
            return NotFound();
        }

        var relations = _context.ProjectResearcherWorks.Include(rel => rel.ScientificWork);

        var works = relations.Where(rel => rel.ResearcherId == id).Select(rel => rel.ScientificWork);

        return await works.ToListAsync();
    }

    [HttpGet("{id}/projects")]
    public async Task<ActionResult<IEnumerable<Project>>> GetResearchersProjects(int id)
    {
        if (!await ResearcherExists(id))
        {
            return NotFound();
        }

        var relations = _context.ProjectResearcherWorks.Include(rel => rel.Project);

        var projects = relations.Where(rel => rel.ResearcherId == id).Select(rel => rel.Project);

        return await projects.ToListAsync();
    }
}
