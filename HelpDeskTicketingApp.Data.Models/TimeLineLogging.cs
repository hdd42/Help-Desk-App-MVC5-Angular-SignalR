using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTicketingApp.Data.Models
{
    public class TimeLineLog
    {
        [Key]
        public int EntryId { get; set; }

        public string UserName { get; set; }


        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime? TimeHappened { get; set; }
        public string Notes { get; set; }
        public string EntryType { get; set; }
    }
}
