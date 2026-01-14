using DietetykAPI.Models.Entities;
using DietetykAPI.Models.Records;
using DietetykAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static DietetykAPI.Services.SendMedicalRecommendation;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MedicalRecommendationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedicalRecommendationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<MedicalRecommendationRecord>> PostMedicalRecommendation(MedicalRecommendationRecord medicalRecommendation)
    {
        if (string.IsNullOrWhiteSpace(medicalRecommendation.note))
            return BadRequest("Notatka nie może być pusta.");

     
        var entity = new MedicalRecomendations
        {
            dietId = medicalRecommendation.dietId,
            note = medicalRecommendation.note,
            MedicalRecomendationsId = medicalRecommendation.MedicalRecomendationsId,
            date = DateTimeOffset.UtcNow
        };

        _context.MedicalRecommendations.Add(entity);
        await _context.SaveChangesAsync();

        var resultRecord = new MedicalRecommendationRecord(
            entity.MedicalRecomendationsId,
            entity.dietId,
            entity.note,
            entity.date
        );

      
        _ = Task.Run(async () =>
        {
            try
            {
     
                using var scope = HttpContext.RequestServices.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var diet = await db.Diets.FirstOrDefaultAsync(d => d.dietId == medicalRecommendation.dietId)
                           ?? throw new Exception("Nie znaleziono diety.");
                var visit = await db.Visits.FirstOrDefaultAsync(v => v.VisitId == medicalRecommendation.MedicalRecomendationsId)
                            ?? throw new Exception("Nie znaleziono wizyty.");
                var customer = await db.Customers.FirstOrDefaultAsync(c => c.pesel == visit.CustomerPesel)
                               ?? throw new Exception("Nie znaleziono klienta.");
                var employee = await db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == visit.EmployeeId)
                               ?? throw new Exception("Nie znaleziono pracownika.");

                try
                {
                    await SendMedicalRecommendation.sendMedicalRecommendation(
                        medicalRecommendation.note,
                        diet.pdf,
                        customer.firstName,
                        customer.email,
                        employee.firstName + " " + employee.lastName
                    );
                }
                catch (Exception mailEx)
                {
                    Console.WriteLine($"Błąd wysyłki maila w tle: {mailEx}");
                }
            }
            catch (Exception dbEx)
            {
                Console.WriteLine($"Błąd pobierania danych w tle: {dbEx}");
            }
        });
        return CreatedAtAction(nameof(GetMedicalRecommendation), new { id = entity.MedicalRecomendationsId }, resultRecord);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalRecommendationRecord>> GetMedicalRecommendation(int id)
    {
        var entity = await _context.MedicalRecommendations.FindAsync(id);
        if (entity == null) return NotFound();

        return new MedicalRecommendationRecord(
            entity.MedicalRecomendationsId,
            entity.dietId,
            entity.note,
            entity.date = DateTimeOffset.UtcNow
        );
    }

}