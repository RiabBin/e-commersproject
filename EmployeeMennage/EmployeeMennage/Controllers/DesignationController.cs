using EmployeeMennage.AdDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMennage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly AddBbContext _context;

        public DesignationController(AddBbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _context.designation.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _context.designation.FindAsync(id);

                if (data == null)
                    return NotFound("Designation not found");

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================
        // FILTER API (by departmentId)
        // =========================
        [HttpGet("filter")]
        public async Task<IActionResult> FilterByDepartment(int departmentId)
        {
            try
            {
                var data = await _context.designation
                    .Where(x => x.depertmentId == departmentId)
                    .ToListAsync();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DesignationTBL model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _context.designation.AddAsync(model);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = model.designationId }, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================
        // UPDATE
        // =========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DesignationTBL model)
        {
            if (id != model.designationId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _context.designation.FindAsync(id);

                if (existing == null)
                    return NotFound("Designation not found");

                existing.designationName = model.designationName;
                existing.depertmentId = model.depertmentId;

                await _context.SaveChangesAsync();

                return Ok(existing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepertment(int id)
        {
            var depertment = await _context.depertment.FindAsync(id);

            if (depertment == null)
                return NotFound();

            _context.depertment.Remove(depertment);
            await _context.SaveChangesAsync();

            return NoContent(); // Best practice
        }
    }
    }