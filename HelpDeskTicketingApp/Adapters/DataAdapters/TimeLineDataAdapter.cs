using HelpDeskTicketingApp.Adapters.Interfaces;
using HelpDeskTicketingApp.Data;
using HelpDeskTicketingApp.Data.Models;
using HelpDeskTicketingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTicketingApp.Adapters.DataAdapters
{
    public class TimeLineDataAdapter:ITimeLineAdapter
    {
        ApplicationDbContext _db;
        public List<Models.TimeLine> GetAll()
        {
            List<TimeLine> model; 

            using (_db = new ApplicationDbContext())
            {
                model = _db.TimeLineLogs.Select(tm => new TimeLine
                {
                    Description = tm.Description,
                    EntryType = tm.EntryType,
                    TimeHappened = tm.TimeHappened,
                    Title = tm.Title,
                    UserName = tm.UserName,

                }).OrderByDescending(x => x.TimeHappened).ToList();

            }
            return model;
        }

        public void AddNewEntry(TimeLineLog t)
        {
            using (_db = new ApplicationDbContext())
            {
                _db.TimeLineLogs.Add(t);
                _db.SaveChanges();
            }
        }


        public AdminDashboardVidewModel InitDashBoard()
        {
            AdminDashboardVidewModel model = new AdminDashboardVidewModel();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model = db.Issues.Select(i => new AdminDashboardVidewModel
                {
                    TotalTicketAssignedCount = db.Issues.Count(ta => ta.IsAssigned == true),
                    TotalTicketCount = db.Issues.Count(),
                    TotalTicketUnsolvedCount = db.Issues.Count(ta => ta.IsAssigned == false)
                }).FirstOrDefault();


            }
            if (model == null)
            {
                model = new AdminDashboardVidewModel();
            }

            return model;
        }
    }
}