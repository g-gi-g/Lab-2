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
public class ScientificWorksController : ControllerBase
{
    private readonly ScientificWorksArchiveAPIContext _context;

    public ScientificWorksController(ScientificWorksArchiveAPIContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScientificWork>>> GetScientificWorks()
    {
        return await _context.ScientificWorks.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScientificWork>> GetScientificWork(int id)
    {
        var scientificWork = await _context.ScientificWorks.FindAsync(id);

        if (scientificWork == null)
        {
            return NotFound();
        }

        return scientificWork;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutScientificWork(int id, ScientificWorkInput scientificWorkInput)
    {
        if (id != scientificWorkInput.Id)
        {
            return BadRequest();
        }

        var fileName = scientificWorkInput.WorkFile.FileName;
        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        if (ext != ".pdf")
        {
            return BadRequest();
        }

        ScientificWork scientificWork = new ScientificWork
        {
            Id = id,
            Name = scientificWorkInput.Name,
            Description = scientificWorkInput.Description,
            RegistrationDate = scientificWorkInput.RegistrationDate,
        };

        Stream stream = scientificWorkInput.WorkFile.OpenReadStream();
        BinaryReader reader = new BinaryReader(stream);
        scientificWork.WorkFile = reader.ReadBytes((int)stream.Length);
        reader.Close();
        stream.Close();

        _context.Entry(scientificWork).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ScientificWorkExists(id))
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
    public async Task<ActionResult<ScientificWork>> PostScientificWork(ScientificWorkInput scientificWorkInput)
    {
        var fileName = scientificWorkInput.WorkFile.FileName;
        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        if (ext != ".pdf")
        {
            return BadRequest();
        }

        ScientificWork scientificWork = new ScientificWork
        {
            Name = scientificWorkInput.Name,
            Description = scientificWorkInput.Description,
            RegistrationDate = scientificWorkInput.RegistrationDate,
        };

        Stream stream = scientificWorkInput.WorkFile.OpenReadStream();
        BinaryReader reader = new BinaryReader(stream);
        scientificWork.WorkFile = reader.ReadBytes((int)stream.Length);
        reader.Close();
        stream.Close();

        _context.ScientificWorks.Add(scientificWork);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetScientificWork", new { id = scientificWork.Id }, scientificWork);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteScientificWork(int id)
    {
        var scientificWork = await _context.ScientificWorks.FindAsync(id);
        if (scientificWork == null)
        {
            return NotFound();
        }

        _context.ScientificWorks.Remove(scientificWork);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ScientificWorkExists(int id)
    {
        return _context.ScientificWorks.Any(e => e.Id == id);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Researcher>>> GetWorkReaserchers(int id)
    {
        if (!ScientificWorkExists(id))
        {
            return NotFound();
        }

        var relations = _context.ProjectResearcherWorks.Include(rel => rel.Researcher);

        var researchers = relations.Where(rel => rel.ScientificWorkId == id).Select(rel => rel.Researcher);

        return await researchers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Project>>> GetWorkProjects(int id)
    {
        if (!ScientificWorkExists(id))
        {
            return NotFound();
        }

        var relations = _context.ProjectResearcherWorks.Include(rel => rel.Project);

        var project = relations.Where(rel => rel.ScientificWorkId == id).Select(rel => rel.Project);

        return await project.ToListAsync();
    }
}
