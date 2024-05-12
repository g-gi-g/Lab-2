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
public class ResearcherStatusesController : ControllerBase
{
    private readonly ScientificWorksArchiveAPIContext _context;

    public ResearcherStatusesController(ScientificWorksArchiveAPIContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResearcherStatus>>> GetResearcherStatuses()
    {
        return await _context.ResearcherStatuses.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResearcherStatus>> GetResearcherStatus(int id)
    {
        var researcherStatus = await _context.ResearcherStatuses.FindAsync(id);

        if (researcherStatus == null)
        {
            return NotFound();
        }

        return researcherStatus;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutResearcherStatus(int id, ResearcherStatus researcherStatus)
    {
        if (id != researcherStatus.Id)
        {
            return BadRequest();
        }

        _context.Entry(researcherStatus).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ResearcherStatusExists(id))
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
    public async Task<ActionResult<ResearcherStatus>> PostResearcherStatus(ResearcherStatus researcherStatus)
    {
        _context.ResearcherStatuses.Add(researcherStatus);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetResearcherStatus", new { id = researcherStatus.Id }, researcherStatus);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResearcherStatus(int id)
    {
        var researcherStatus = await _context.ResearcherStatuses.FindAsync(id);
        if (researcherStatus == null)
        {
            return NotFound();
        }

        _context.ResearcherStatuses.Remove(researcherStatus);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ResearcherStatusExists(int id)
    {
        return _context.ResearcherStatuses.Any(e => e.Id == id);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Researcher>>> GetResearchersWithStatus(int id)
    {
        if (!ResearcherStatusExists(id))
        {
            return NotFound();
        }

        var researchers = _context.Researchers.Where(e => e.ResearcherStatusId == id);

        return await researchers.ToListAsync();
    }
}
