using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTicketingApp.Data.Models
{
    public class IssueType
    {
        [Key]
        public int IssueTypeId { get; set; }

        public string IssueTypeDesc { get; set; }
    }
}
