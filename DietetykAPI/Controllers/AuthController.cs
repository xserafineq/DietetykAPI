using DietetykAPI.Models.Entities;
using DietetykAPI.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuthService _authService;

    public AuthController(AppDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(EmployeeLoginRecord emp)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.email == emp.email);
        if (employee == null)
            return Unauthorized("Nieprawidłowy email lub hasło.");

        bool result = BCrypt.Net.BCrypt.Verify(emp.password, employee.password);

        if (result == false)
            return Unauthorized("Nieprawidłowy email lub hasło.");

        var token = _authService.GenerateJwt(employee);
        return Ok(new { token });
    }
}
