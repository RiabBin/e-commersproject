using Microsoft.AspNetCore.Mvc;
using EmployeeMennage.AdDbContext;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMennage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepertmentmasterController : ControllerBase
    {
        private readonly AddBbContext _context;

        public DepertmentmasterController(AddBbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepertment()
        {
            var depertments = await _context.depertment.ToListAsync();
            return Ok(depertments);
        }

        // =========================
        // POST
        // =========================
        [HttpPost]
        public async Task<IActionResult> AddDepertment([FromBody] DepertmentTBL depertment)
        {
            if (depertment == null || string.IsNullOrWhiteSpace(depertment.DepertmentName))
                return BadRequest("Depertment name is required.");

            string name = depertment.DepertmentName.Trim().ToLower();

            bool exists = await _context.depertment
                .AnyAsync(d => d.DepertmentName.ToLower() == name);

            if (exists)
                return Conflict("Depertment name already exists.");

            depertment.DepertmentName = depertment.DepertmentName.Trim();

            await _context.depertment.AddAsync(depertment);
            await _context.SaveChangesAsync();

            return Ok(depertment);
        }

        // =========================
        // PUT
        // =========================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepertment(int id, [FromBody] DepertmentTBL depertment)
        {
            var existing = await _context.depertment.FindAsync(id);
            if (existing == null)
                return NotFound("Depertment not found.");

            string name = depertment.DepertmentName.Trim().ToLower();

            bool exists = await _context.depertment
                .AnyAsync(d => d.depertmentId!= id && d.DepertmentName.ToLower() == name);

            if (exists)
                return Conflict("Depertment name already exists.");

            existing.DepertmentName = depertment.DepertmentName.Trim();
            existing.IsActive = depertment.IsActive;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepertment(int id)
        {
            var depertment = await _context.depertment.FindAsync(id);

            if (depertment == null)
                return NotFound(new { message = "Depertment not found" });

            _context.depertment.Remove(depertment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Depertment deleted successfully" });
        }
    }
}