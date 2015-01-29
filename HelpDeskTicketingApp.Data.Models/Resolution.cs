using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTicketingApp.Data.Models
{
    public class Resolution
    {
        [Key]
        public int ResolutionId { get; set; }

        public bool IsResolved { get; set; }
        public string ResolutionDesc { get; set; }
        public string Notes { get; set; }
    }
}
