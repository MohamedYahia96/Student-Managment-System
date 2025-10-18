using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAssignmentManager.Models
{
    public class Reminder
    {
        [Key]
        public int ReminderId { get; set; }

        public int AssignmentId { get; set; }

        public DateTime ReminderDate { get; set; }

        public bool IsSent { get; set; } = false;

        [MaxLength(500)]
        public string Message { get; set; }

        [ForeignKey("AssignmentId")]
        public virtual Assignment Assignment { get; set; }
    }
}