using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Data;
using StudentAssignmentManager.Models;

namespace StudentAssignmentManager.Repositories
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(StudentSystemDbContext context) : base(context) { }

        public IEnumerable<Course> GetCoursesByStudent(int studentId)
        {
            return _context.Courses
                .Include(c => c.Assignments)
                .Where(c => c.StudentId == studentId)
                .ToList();
        }

        public Course GetCourseWithAssignments(int courseId)
        {
            return _context.Courses
                .Include(c => c.Assignments)
                .FirstOrDefault(c => c.CourseId == courseId);
        }
    }
}