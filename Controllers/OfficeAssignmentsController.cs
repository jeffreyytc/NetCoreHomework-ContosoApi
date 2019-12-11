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
    public class OfficeAssignmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public OfficeAssignmentsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/OfficeAssignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfficeAssignment>>> GetOfficeAssignments()
        {
            return await _context.OfficeAssignments.ToListAsync();
        }

        // GET: api/OfficeAssignments/5
        [HttpGet("{instructorId}")]
        public async Task<IActionResult> GetOfficeAssignment(int instructorId)
        {
            var officeAssignment = await _context.OfficeAssignments.Where(o => o.InstructorId == instructorId).Select(o => new
            {
                Instructor = new
                {
                    Id = o.InstructorId,
                    FullName = $"{o.Instructor.FirstName} {o.Instructor.LastName}"
                },
                o.Location
            }).FirstOrDefaultAsync();

            if (officeAssignment == null)
            {
                return NotFound();
            }

            return new JsonResult(officeAssignment);
        }

        // PUT: api/OfficeAssignments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{instructorId}")]
        public async Task<IActionResult> PutOfficeAssignment(int instructorId, OfficeAssignment officeAssignment)
        {
            if (instructorId != officeAssignment.InstructorId)
            {
                return BadRequest();
            }

            _context.Entry(officeAssignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficeAssignmentExists(instructorId))
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

        // POST: api/OfficeAssignments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<OfficeAssignment>> PostOfficeAssignment(OfficeAssignment officeAssignment)
        {
            _context.OfficeAssignments.Add(officeAssignment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OfficeAssignmentExists(officeAssignment.InstructorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOfficeAssignment", new { InstructorId = officeAssignment.InstructorId }, officeAssignment);
        }

        // DELETE: api/OfficeAssignments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OfficeAssignment>> DeleteOfficeAssignment(int id)
        {
            var officeAssignment = await _context.OfficeAssignments.FindAsync(id);
            if (officeAssignment == null)
            {
                return NotFound();
            }

            _context.OfficeAssignments.Remove(officeAssignment);
            await _context.SaveChangesAsync();

            return officeAssignment;
        }

        private bool OfficeAssignmentExists(int instructorId)
        {
            return _context.OfficeAssignments.Any(e => e.InstructorId == instructorId);
        }
    }
}
