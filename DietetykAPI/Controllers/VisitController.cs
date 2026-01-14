using DietetykAPI.Models.Entities;
using DietetykAPI.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class VisitsController : ControllerBase
{
    private readonly AppDbContext _context;

    public VisitsController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Visit>>> GetVisits()
    {
        return await _context.Visits
       .Include(v => v.Employee)
       .Include(v => v.Customer)
       .Include(v => v.Notification)
       .Include(v => v.Result)
       .Include(v => v.Recomendation)
       .ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<VisitRecord>>> GetVisit(int id)
    {
        return await _context.Visits.Where(v => v.VisitId == id).Select(v => new VisitRecord(
            v.VisitId,
            v.Date,
            v.EmployeeId,
            v.CustomerPesel,
            v.status,
            v.Customer,
            v.Recomendation
            ))
       .ToListAsync();
    }
    [HttpGet("date={date}/employeeId={id}")]
    public async Task<ActionResult<IEnumerable<VisitRecord>>> GetVisit(DateOnly date, int id)
    {
  
        var startOfDay = new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
      
        var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

        return await _context.Visits
            .Where(v => v.EmployeeId == id && v.Date >= startOfDay && v.Date <= endOfDay)
            .OrderBy(v => v.Date)
            .Select(v => new VisitRecord(
                v.VisitId,
                v.Date, 
                v.EmployeeId,
                v.CustomerPesel,
                v.status,
                v.Customer,
                v.Recomendation
            ))
            .ToListAsync();
    }

    [HttpGet("employeeId={id}")]
    public async Task<ActionResult<IEnumerable<Visit>>> GetVisitByEmployeeId(int id)
    {
        return await _context.Visits.Where(v=>v.EmployeeId == id)
      .Include(v => v.Employee)
      .Include(v => v.Customer)
      .ToListAsync();
    }


    [HttpPut("{id}")]
    public IActionResult UpdateVisitStatus(int id, [FromBody] VisitStatus visitUpdated)
    {
        var visit = _context.Visits.FirstOrDefault(v => v.VisitId == id);
        if (visit == null)
            return NotFound("Nie znaleziono wizyty");


        visit.status = visitUpdated.status;

        _context.SaveChanges();

        return Ok(visit);
    }

    [HttpPut("{id}/{date}")]
    public IActionResult UpdateVisitStatus(int id,DateOnly date, [FromBody] PostPonedVisit visitUpdated)
    {
        var visit = _context.Visits.FirstOrDefault(v => v.VisitId == id);
        if (visit == null)
            return NotFound("Nie znaleziono wizyty");
        if (_context.Visits.Any(v => v.Date == visitUpdated.date))
        {
            return Conflict("Już ktoś jest zarejestrowany na tą godzinę");
        }

        visit.status = visitUpdated.status;
        visit.Date = visitUpdated.date.UtcDateTime;

        _context.SaveChanges();

        return Ok(visit);
    }



    [HttpPost]
    public async Task<ActionResult<RegisterVisitRecord>> RegisterClient(RegisterVisitRecord visit)
    {
        if (await _context.Visits.AnyAsync(v => v.Date == visit.Date && v.CustomerPesel == visit.CustomerPesel))
        {
            return Conflict("Klient już jest zarejestrowany na tą wizytę");
        }

        var config = await _context.Config.OrderBy(c => c.id).FirstOrDefaultAsync();
        

        if (await _context.Visits.AnyAsync(v =>
            v.EmployeeId == visit.EmployeeId &&
            visit.Date >= v.Date.AddMinutes(-(config.visit_duration/2)) &&
            visit.Date <= v.Date.AddMinutes((config.visit_duration/2))))
        {
            return Conflict("Ten termin jest zajęty (wizyty muszą być w odstępie min. 30 min)");
        }

        var visitEntity = new Visit
        {
            EmployeeId = visit.EmployeeId,
            CustomerPesel = visit.CustomerPesel,
            status = "active",
            Date = visit.Date.UtcDateTime
        };

        _context.Visits.Add(visitEntity);
        await _context.SaveChangesAsync();

        return Ok(visit);
    }

}
