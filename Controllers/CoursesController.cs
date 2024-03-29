﻿using System;
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
    public class CoursesController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public CoursesController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.Where(c => !c.IsDeleted).ToListAsync();
        }

        // GET: api/Courses/Students
        [HttpGet("Students")]
        public async Task<ActionResult<IEnumerable<VwCourseStudent>>> GetCourseStudents()
        {
            return await _context.VwCourseStudents.ToListAsync();
        }

        // GET: api/Courses/StudentCounts
        [HttpGet("StudentCounts")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetCourseStudentCounts()
        {
            return await _context.VwCourseStudentCounts.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _context.Courses.Where(c =>
                c.CourseId == id &&
                !c.IsDeleted
            ).Select(c => new
            {
                Id = c.CourseId,
                c.Title,
                c.Credits,
                Department = new
                {
                    Id = c.DepartmentId,
                    c.Department.Name
                },
                Instructors = c.CourseInstructors.Select(i => new
                {
                    Id = i.InstructorId,
                    FullName = $"{i.Instructor.FirstName} {i.Instructor.LastName}"
                }),
                Students = c.Enrollments.Select(e => new
                {
                    e.EnrollmentId,
                    Id = e.StudentId,
                    FullName = $"{e.Student.FirstName} {e.Student.LastName}"
                })
            }).FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }

            return new JsonResult(course);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
