using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTicketingApp.Data.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public int IssueId { get; set; }
        [ForeignKey("IssueId")]
        public virtual Issue Issue { get; set; }

    }
}
