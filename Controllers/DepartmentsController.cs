using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoApi.Models;

namespace ContosoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public DepartmentsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        // GET: api/Departments/CourseCounts
        [HttpGet("CourseCounts")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetDepartmentCourseCounts()
        {
            return await _context.VwDepartmentCourseCounts.FromSqlRaw("SELECT * FROM [vwDepartmentCourseCount]").ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _context.Departments.Where(d => d.DepartmentId == id).Select(d => new
            {
                Id = d.DepartmentId,
                d.Name,
                d.Budget,
                d.StartDate,
                Instructor = new
                {
                    Id = d.InstructorId,
                    FullName = $"{d.Instructor.FirstName} {d.Instructor.LastName}"
                },
                d.RowVersion,
                Courses = d.Courses.Select(c => new
                {
                    Id = c.CourseId,
                    c.Title
                })
            }).FirstOrDefaultAsync();

            if (department == null)
            {
                return NotFound();
            }

            return new JsonResult(department);
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            if (!DepartmentExists(department.DepartmentId))
            {
                return NotFound();
            }

            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC Department_Update {department.DepartmentId}, {department.Name}, {department.Budget}, {department.StartDate}, {department.InstructorId}, {department.RowVersion}"
            );

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public IActionResult PostDepartment(Department department)
        {
            DepartmentInsertResult result = _context.DepartmentInsertResults.FromSqlInterpolated(
                $"EXEC Department_Insert {department.Name}, {department.Budget}, {department.StartDate}, {department.InstructorId}"
            ).ToList()[0];

            return CreatedAtAction("GetDepartment", new { id = result.DepartmentId }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Department>> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            //_context.Departments.Remove(department);
            //await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC Department_Delete {department.DepartmentId}, {department.RowVersion}"
            );

            return department;
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
