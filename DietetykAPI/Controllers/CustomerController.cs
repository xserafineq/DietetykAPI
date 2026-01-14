using DietetykAPI.Models.Entities;
using DietetykAPI.Models.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return await _context.Customers
            .Select(c => new Customer (
                c.pesel,
                c.firstName,
                c.lastName,
                c.age,
                c.email,
                c.residentialAddress,
                c.weightDestination,
                c.phone
            ))
            .ToListAsync();
    }

    [HttpGet("{pesel}")]
    public async Task<ActionResult<Customer>> GetCustomer(string pesel)
    {
        var customer = await _context.Customers
            .Include(c => c.Visits)
            .ThenInclude(v => v.Recomendation)
            .FirstOrDefaultAsync(c => c.pesel == pesel);

        if (customer == null)
        {
            return NotFound(); 
        }

        return customer;
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
    {
        if (_context.Customers.Any(c => c.pesel == customer.pesel))
        {
            return Conflict("Klient o tym numerze PESEL już istnieje.");
        }

        if (_context.Customers.Any(c => c.email == customer.email))
        {
            return Conflict("Klient o tym adresie email już istnieje.");
        }

        if (_context.Customers.Any(c => c.phone == customer.phone))
        {
            return Conflict("Klient o tym numerze telefonu już istnieje.");
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomer), new { pesel = customer.pesel }, customer);
    }


    [HttpPut("{pesel}")]
    public async Task<IActionResult> PutCustomer(string pesel, Customer customer)
    {
        if (pesel != customer.pesel)
        {
            return BadRequest("Numer PESEL w ścieżce URL nie zgadza się z numerem w obiekcie.");
        }

        _context.Entry(customer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Customers.Any(c => c.pesel == pesel))
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


    [HttpDelete("{pesel}")]
    public async Task<IActionResult> DeleteCustomer(string pesel)
    {
        var customer = await _context.Customers.FindAsync(pesel);
        if (customer == null)
        {
            return NotFound();
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<Customer>> GetCustomerByEmail(string email)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.email == email);

        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }

    [HttpGet("search/{name}")]
    public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomersByName(string name)
    {
        var customers = await _context.Customers
            .Where(c => c.firstName.Contains(name) || c.lastName.Contains(name))
            .ToListAsync();

        return customers;
    }
}