using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskTicketingApp.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HelpDeskTicketingApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


       public DbSet<TimeLineLog> TimeLineLogs { get; set; }
        
       public DbSet<Assignment> Assignments { get; set; }
       public DbSet<Issue> Issues { get; set; }
       public DbSet<IssueType> IssueTypes { get; set; }
       public DbSet<Resolution> Resolutions { get; set; } 

        public System.Collections.IEnumerable ApplicationUsers { get; set; }
       
    }
}
