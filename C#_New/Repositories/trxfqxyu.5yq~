using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Data;
using StudentAssignmentManager.Models;

namespace StudentAssignmentManager.Repositories
{
    public class AssignmentRepository : Repository<Assignment>
    {
        public AssignmentRepository(StudentSystemDbContext context) : base(context) { }

        public IEnumerable<Assignment> GetAssignmentsByCourse(int courseId)
        {
            return _context.Assignments
                .Include(a => a.Course)
                .Where(a => a.CourseId == courseId)
                .OrderBy(a => a.DueDate)
                .ToList();
        }

        public IEnumerable<Assignment> GetAssignmentsByStudent(int studentId)
        {
            return _context.Assignments
                .Include(a => a.Course)
                .Where(a => a.Course.StudentId == studentId)
                .OrderBy(a => a.DueDate)
                .ToList();
        }

        public IEnumerable<Assignment> GetUpcomingAssignments(int studentId, int days)
        {
            var futureDate = DateTime.Now.AddDays(days);
            return _context.Assignments
                .Include(a => a.Course)
                .Where(a => a.Course.StudentId == studentId
                    && a.DueDate <= futureDate
                    && a.DueDate >= DateTime.Now
                    && a.Status != "Complete")
                .OrderBy(a => a.DueDate)
                .ToList();
        }

        public IEnumerable<Assignment> SearchAssignments(int studentId, string searchTerm,
            List<string> priorities = null, List<string> statuses = null,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Assignments
                .Include(a => a.Course)
                .Where(a => a.Course.StudentId == studentId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a => a.Title.Contains(searchTerm)
                    || a.Description.Contains(searchTerm)
                    || a.Course.CourseName.Contains(searchTerm));
            }

            if (priorities != null && priorities.Any())
            {
                query = query.Where(a => priorities.Contains(a.Priority));
            }

            if (statuses != null && statuses.Any())
            {
                query = query.Where(a => statuses.Contains(a.Status));
            }

            if (startDate.HasValue)
            {
                query = query.Where(a => a.DueDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.DueDate <= endDate.Value);
            }

            return query.OrderBy(a => a.DueDate).ToList();
        }
    }
}