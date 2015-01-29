using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTicketingApp.Data.Models
{
    public class Issue
    {
        [Key]
        public int IssueId { get; set; }

        public int IssueTypeId { get; set; }
        [ForeignKey("IssueTypeId")]
        public virtual IssueType IssueType { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateReported { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateResolved { get; set; }

        public string IssueDesc { get; set; }
        public bool IsAssigned { get; set; }
        
        public int ResolutionId { get; set; }
        [ForeignKey("ResolutionId")]
        public virtual Resolution Resolution { get; set; }
        
    }
}
