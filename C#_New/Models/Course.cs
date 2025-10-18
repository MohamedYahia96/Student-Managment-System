using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAssignmentManager.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CourseName { get; set; }

        [MaxLength(50)]
        public string CourseCode { get; set; }

        [MaxLength(100)]
        public string Instructor { get; set; }

        public int? Credits { get; set; }

        [MaxLength(100)]
        public string Schedule { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}