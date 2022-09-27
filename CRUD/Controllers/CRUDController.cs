using CRUD_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CRUD_Service.Controllers
{
    [ApiController]
    [Route("api1")]
    public class CRUDController : ControllerBase
    {
        private readonly DataBaseContext _db;

        public CRUDController(DataBaseContext db)
        {
            _db = db;
        }

        // THIS IS FOR C
        [HttpPost("emp")]
        public IActionResult createEmployee([FromBody] Employee emp)
        {
            _db.Employees.Add(emp);
            _db.SaveChanges();
            return Ok(emp);

        }
        // THIS IS FOR R
        [HttpGet("emp/{id}")]
        public IActionResult getEmployee(long id)
        {
            Employee emp = _db.Employees.Find(id);
            emp.Password = null;

            return Ok(emp);
        }

        // THIS IS FOR U
        [HttpPost("emp/{id}")]
        public async Task<IActionResult> updateEmployee(long id, [FromBody] Employee emp)
        {
            if (id != emp.Id)
            {
                return BadRequest();
            }
            if (_db.Employees.Find(id) == null)
            {
                return NotFound();
            }
            try
            {
                _db.Entry<Employee>(emp).State = EntityState.Modified;

                _db.Entry<Employee>(emp).Property(p => p.Password).IsModified = false;
                _db.Entry<Employee>(emp).Property(p => p.Email).IsModified = false;
                _db.Entry<Employee>(emp).Property(p => p.Id).IsModified = false;

                await _db.SaveChangesAsync();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
            {
                if (_db.Employees.Find(id) == null)
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();

        }

        // THIS IS FOR D
        [HttpDelete("emp/{id}")]
        public async Task<IActionResult> deleteEmployee(long id)
        {
            var emp = await _db.Employees.FindAsync(id);
            if (emp == null)
            {
                return NotFound();
            }
            _db.Employees.Remove(emp);
            await _db.SaveChangesAsync();

            return Ok(emp);

        }
    }
}
