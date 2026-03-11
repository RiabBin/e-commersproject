using EmployeeMennage.AdDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMennage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AddBbContext _context;

        public EmployeeController(AddBbContext context)
        {
            _context = context;
        }

        // =========================================
        // GET ALL (Filter + Pagination)
        // =========================================
        [HttpGet]
        public async Task<IActionResult> GetAll(
            string? name,
            string? district,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var query = _context.employee.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(x => x.employeeName.Contains(name));

                if (!string.IsNullOrEmpty(district))
                    query = query.Where(x => x.emDistrict.Contains(district));

                var totalRecords = await query.CountAsync();

                var data = await query
                    .OrderByDescending(x => x.employeeId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    totalRecords,
                    pageNumber,
                    pageSize,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================================
        // GET BY ID
        // =========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _context.employee.FindAsync(id);

                if (employee == null)
                    return NotFound("Employee not found");

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================================
        // CREATE
        // =========================================
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeTBL model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (await _context.employee.AnyAsync(x => x.emContact == model.emContact))
                    return BadRequest("Contact already exists");

                if (await _context.employee.AnyAsync(x => x.emMail == model.emMail))
                    return BadRequest("Email already exists");

                model.createDate = DateTime.UtcNow;

                await _context.employee.AddAsync(model);
                await _context.SaveChangesAsync();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================================
        // UPDATE
        // =========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmployeeTBL model)
        {
            if (id != model.employeeId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _context.employee.FindAsync(id);

                if (existing == null)
                    return NotFound("Employee not found");

                if (await _context.employee
                    .AnyAsync(x => x.emContact == model.emContact && x.employeeId != id))
                    return BadRequest("Contact already exists");

                if (await _context.employee
                    .AnyAsync(x => x.emMail == model.emMail && x.employeeId != id))
                    return BadRequest("Email already exists");

                existing.employeeName = model.employeeName;
                existing.emContact = model.emContact;
                existing.emMail = model.emMail;
                existing.emDistrict = model.emDistrict;
                existing.address = model.address;
                existing.designationId = model.designationId;

                await _context.SaveChangesAsync();

                return Ok(existing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================================
        // DELETE
        // =========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _context.employee.FindAsync(id);

                if (employee == null)
                    return NotFound("Employee not found");

                _context.employee.Remove(employee);
                await _context.SaveChangesAsync();

                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // =========================================
        // LOGIN
        // =========================================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Authenticate using email and contact (as provided in LoginDTO)
                var user = await _context.employee
                    .FirstOrDefaultAsync(x => x.emMail == model.emMail && x.emContact == model.emContact);

                if (user == null)
                    return Unauthorized("Invalid email or contact");

                return Ok(new
                {
                    message = "Login Successful",
                    userId = user.employeeId,
                    email = user.emMail,
                    name = user.employeeName,
                    user.role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}