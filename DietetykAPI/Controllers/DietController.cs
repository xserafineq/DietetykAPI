using DietetykAPI.Models.Entities;
using DietetykAPI.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DietsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DietsController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DietRecord>>> GetDiets()
    {
        return await _context.Diets.Select(e => new DietRecord(
            e.dietId,
            e.type,
            e.kcalDeficit
        )).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Diet>> GetDiet(int id)
    {
        var diet = await _context.Diets
                                     .FirstOrDefaultAsync(e => e.dietId == id);
        if (diet == null)
        {
            return NotFound(); 
        }

        return diet;
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> GetDietPdf(int id)
    {
        var diet = await _context.Diets
                                 .FirstOrDefaultAsync(e => e.dietId == id);
        if (diet == null || diet.pdf == null)
        {
            return NotFound();
        }
        return File(diet.pdf, "application/pdf", $"diet_{id}.pdf");
    }


    [HttpPost]
    public async Task<ActionResult<Diet>> PostDiet([FromForm] string type, [FromForm] int kcalDeficit, [FromForm] IFormFile pdf)
    {
        byte[] pdfData = Array.Empty<byte>();

        if (pdf?.Length > 0)
        {
            using var ms = new MemoryStream();
            await pdf.CopyToAsync(ms);
            pdfData = ms.ToArray();
        }

        var diet = new Diet
        {
            type = type,
            kcalDeficit = kcalDeficit,
            pdf = pdfData 
        };

        _context.Diets.Add(diet);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDiet), new { id = diet.dietId }, diet);
    }
}