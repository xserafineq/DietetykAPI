using DietetykAPI.Models.Entities;
using DietetykAPI.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MedicalResultsController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedicalResultsController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalResultRecord>>> GetMedicalResults()
    {
        return await _context.MedicalResults.Select(m => new MedicalResultRecord(
            m.MedicalResultId,
            m.weight,
            m.height,
            m.bodyFat,
            m.waistLine,
            m.sugarLevel,
            m.bmi,
            m.date
            ))
        .ToListAsync();

    }
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalResultRecord>> GetMedicalResult(int id)
    {
        return await _context.MedicalResults
        .Where(m => m.MedicalResultId == id)
        .Select(m => new MedicalResultRecord(
            m.MedicalResultId,
            m.weight,
            m.height,
            m.bodyFat,
            m.waistLine,
            m.sugarLevel,
            m.bmi,
            m.date
        ))
        .FirstOrDefaultAsync();
    }

    [HttpPost]
    public async Task<ActionResult<MedicalResultRecord>> PostMedicalResult(MedicalResultRecord medicalResult)
    {
        if (medicalResult.height <= 0 || medicalResult.weight <= 0)
        {
            return BadRequest("Zle dane pomiarowe");
        }


        var existing = await _context.MedicalResults
        .AnyAsync(m => m.MedicalResultId == medicalResult.MedicalResultId);

        if (existing)
            return Conflict("Dla tej wizyty istnieje już pomiar");

        var entity = new MedicalResult
        {
            MedicalResultId = medicalResult.MedicalResultId,
            weight = medicalResult.weight,
            height = medicalResult.height,
            waistLine = medicalResult.waistLine,
            bodyFat = medicalResult.bodyFat,
            sugarLevel = medicalResult.sugarLevel,
            bmi = Math.Round(medicalResult.weight / Math.Pow(medicalResult.height / 100.0, 2), 2),
            date = DateTime.UtcNow
        };

        _context.MedicalResults.Add(entity);
        await _context.SaveChangesAsync();

        var resultRecord = new MedicalResultRecord(
            entity.MedicalResultId,
            entity.weight,
            entity.height,
            entity.bodyFat,
            entity.waistLine,
            entity.sugarLevel,
            entity.bmi,
            entity.date
        );

        return CreatedAtAction(nameof(GetMedicalResult), new { id = entity.MedicalResultId }, resultRecord);
    }


}