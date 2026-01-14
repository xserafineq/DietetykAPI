using DietetykAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DietetykAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConfigController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Config>> GetConfig()
        {
            var config = await _context.Config.FirstOrDefaultAsync();

            if (config == null)
            {
                return NotFound("Konfiguracja nie została jeszcze utworzona.");
            }

            return Ok(config);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateConfig(Config updatedConfig)
        {
            var config = await _context.Config.OrderBy(config => config.id).FirstOrDefaultAsync();

            if (config == null)
            {
                _context.Config.Add(updatedConfig);
            }
            else
            {
                config.visit_duration = updatedConfig.visit_duration;
                _context.Config.Update(config);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Wystąpił błąd podczas zapisywania konfiguracji.");
            }

            return NoContent();
        }
    }
}