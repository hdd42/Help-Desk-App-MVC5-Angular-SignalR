using HelpDeskTicketingApp.Adapters.Interfaces;
using HelpDeskTicketingApp.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTicketingApp.Adapters.DataAdapters
{
    public class StatisticDataAdapter : IStatisticAdapter
    {
        ApplicationDbContext _db;
        public Dictionary<string, int> GeneralIssueStatistic()
        {
           Dictionary<string ,int> model = new Dictionary<string,int>();
            
        

           using (_db= new ApplicationDbContext())
           {
               var issue_solved = _db.Issues.Count(x => x.DateResolved != null);
               var issue_all = _db.Issues.Count();
               var issue_assigned = _db.Issues.Count(x => x.DateResolved == null && x.IsAssigned == true);
               model.Add("Issue Solved", issue_solved);

               model.Add("Working on it", issue_assigned);
               model.Add("All Issues", issue_all);
           }
           

            return model;
        }


        public List<GeneralTechStats> GeneralTechStatistic()
        {
            List<GeneralTechStats> model = new List<GeneralTechStats>();

            using (_db = new ApplicationDbContext())
            {
               model.Add(new GeneralTechStats
                {
                    Name="Coral",
                    a = _db.Assignments.Count(x => x.User.FirstName.Equals("Coral") && x.Issue.DateResolved !=null),
                    b = _db.Assignments.Count(x => x.User.FirstName.Equals("Coral") && x.Issue.DateResolved == null)// _db.Resolutions.Count(x => x.IsResolved == true && x.)
                });

                model.Add(new GeneralTechStats
                {
                    Name = "Huseyin",
                    a = _db.Assignments.Count(x => x.User.FirstName.Equals("Huseyin") && x.Issue.DateResolved != null),
                    b = _db.Assignments.Count(x => x.User.FirstName.Equals("Huseyin") && x.Issue.DateResolved == null)
                });

                model.Add(new GeneralTechStats
                {
                    Name = "Louis",
                    a = _db.Assignments.Count(x => x.User.FirstName.Equals("Louis") && x.Issue.DateResolved != null),
                    b = _db.Assignments.Count(x => x.User.FirstName.Equals("Louis") && x.Issue.DateResolved == null)
                });
            }

           

            return model;
        }

        public List<GeneralIssueTypeStats> GeneralIssueTypesStatistic()
        {
            List<GeneralIssueTypeStats> model = new List<GeneralIssueTypeStats>();
            using (_db = new ApplicationDbContext())
            {
                var issue_types = _db.IssueTypes.ToList();
                foreach (var item in issue_types)
                {
                    model.Add(new GeneralIssueTypeStats
                     {
                         Name = item.IssueTypeDesc,
                         a = _db.Issues.Count(x => x.IssueType.IssueTypeDesc == item.IssueTypeDesc)
                     });
                }
            }
          


            return model;
        }
    }

    public class GeneralTechStats
    {
        public string Name{ get; set; }
        public int  a { get; set; }
        public int b { get; set; }
    }

    public class GeneralIssueTypeStats
    {
        public string Name { get; set; }
        public int a { get; set; }
        
    }

}