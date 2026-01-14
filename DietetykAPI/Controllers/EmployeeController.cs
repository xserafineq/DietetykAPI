using BCrypt;
using DietetykAPI.Models.Entities;
using DietetykAPI.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")] 
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeRecord>>> GetEmployees()
    {
        return await _context.Employees.Select(e => new EmployeeRecord(
            e.EmployeeId,
            e.firstName,
            e.lastName,
            e.email,
            e.password
        )).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _context.Employees
                                     .Include(e => e.Visits)
                                     .FirstOrDefaultAsync(e => e.EmployeeId == id);

        if (employee == null)
        {
            return NotFound(); 
        }

        return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    {
        if (_context.Employees.Any(e => e.email == employee.email))
        {
            return Conflict("Pracownik o tym adresie email już istnieje.");
        }

        employee.password = BCrypt.Net.BCrypt.HashPassword(employee.password);
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, [FromBody] EmployeeUpdate updatedData)
    {
      
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

        if (employee == null) return NotFound("Nie znaleziono pracownika");

        employee.isadmin = updatedData.isadmin;

        if (!string.IsNullOrEmpty(updatedData.password))
        {
            employee.password = BCrypt.Net.BCrypt.HashPassword(updatedData.password);
        }
        await _context.SaveChangesAsync();
        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound(); 
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}