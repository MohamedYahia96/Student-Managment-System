using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Data;
using StudentAssignmentManager.Models;

namespace StudentAssignmentManager.Repositories
{
    public class StudentRepository : Repository<Student>
    {
        public StudentRepository(StudentSystemDbContext context) : base(context) { }

        public Student GetByUsername(string username)
        {
            return _context.Students
                .Include(s => s.Courses)
                .FirstOrDefault(s => s.Username == username);
        }

        public Student AuthenticateStudent(string username, string password)
        {
            return _context.Students
                .FirstOrDefault(s => s.Username == username && s.PasswordHash == password);
        }
    }
}