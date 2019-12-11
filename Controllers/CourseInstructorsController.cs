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
    public class CourseInstructorsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public CourseInstructorsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/CourseInstructors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseInstructor>>> GetCourseInstructors()
        {
            return await _context.CourseInstructors.ToListAsync();
        }

        // GET: api/CourseInstructors/5
        [HttpGet("{courseId}/{instructorId}")]
        public async Task<IActionResult> GetCourseInstructor(int courseId, int instructorId)
        {
            var courseInstructor = await _context.CourseInstructors.Where(ci =>
                ci.CourseId == courseId &&
                ci.InstructorId == instructorId
            ).Select(ci => new
            {
                Course = new
                {
                    Id = ci.CourseId,
                    ci.Course.Title
                },
                Instructor = new
                {
                    Id = ci.InstructorId,
                    FullName = $"{ci.Instructor.FirstName} {ci.Instructor.LastName}"
                }
            }).FirstOrDefaultAsync();

            if (courseInstructor == null)
            {
                return NotFound();
            }

            return new JsonResult(courseInstructor);
        }

        // PUT: api/CourseInstructors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{courseId}/{instructorId}")]
        //public async Task<IActionResult> PutCourseInstructor(int courseId, int instructorId, CourseInstructor courseInstructor)
        //{
        //    if (courseId != courseInstructor.CourseId && instructorId != courseInstructor.InstructorId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(courseInstructor).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CourseInstructorExists(courseId, instructorId))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/CourseInstructors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CourseInstructor>> PostCourseInstructor(CourseInstructor courseInstructor)
        {
            _context.CourseInstructors.Add(courseInstructor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseInstructorExists(courseInstructor.CourseId, courseInstructor.InstructorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCourseInstructor", new
            {
                courseId = courseInstructor.CourseId,
                instructorId = courseInstructor.InstructorId
            }, courseInstructor);
        }

        // DELETE: api/CourseInstructors/5
        [HttpDelete("{courseId}/{instructorId}")]
        public async Task<ActionResult<CourseInstructor>> DeleteCourseInstructor(int courseId, int instructorId)
        {
            var courseInstructor = await _context.CourseInstructors.Where(ci =>
                ci.CourseId == courseId &&
                ci.InstructorId == instructorId
            ).FirstOrDefaultAsync();

            if (courseInstructor == null)
            {
                return NotFound();
            }

            _context.CourseInstructors.Remove(courseInstructor);
            await _context.SaveChangesAsync();

            return courseInstructor;
        }

        private bool CourseInstructorExists(int courseId, int instructorId)
        {
            return _context.CourseInstructors.Any(e => e.CourseId == courseId && e.InstructorId == instructorId);
        }
    }
}
