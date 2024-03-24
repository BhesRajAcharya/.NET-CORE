using JWT_WEBAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWT_WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly UserDbContext userDbContext;

        public EmployeeController(UserDbContext userDbContext) {
            this.userDbContext = userDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployes()
        {
            var user = await userDbContext.Employees.ToListAsync();
            return Ok(user);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var user = await userDbContext.Employees.FindAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee) {
          var user=await userDbContext.Employees.AddAsync(employee);
            await userDbContext.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id,Employee employee)
        {
            userDbContext.Entry(employee).State = EntityState.Modified;
            await userDbContext.SaveChangesAsync();
            return Ok(employee);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var emp = await userDbContext.Employees.FindAsync(id);
            if (emp != null)
            {
                userDbContext.Remove(emp);
                await userDbContext.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
