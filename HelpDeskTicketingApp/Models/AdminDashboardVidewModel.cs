using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTicketingApp.Models
{
    public class AdminDashboardVidewModel
    {
        public int TotalTicketCount { get; set; }
        public int TotalTicketUnsolvedCount { get; set; }
        public int TotalTicketAssignedCount { get; set; }

      
    }
}