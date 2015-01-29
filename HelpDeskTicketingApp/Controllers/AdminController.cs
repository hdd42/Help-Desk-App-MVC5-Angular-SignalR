using HelpDeskTicketingApp.Data;
using HelpDeskTicketingApp.Data.Models;
using HelpDeskTicketingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using HelpDeskTicketingApp.Adapters;
using HelpDeskTicketingApp.Adapters.Interfaces;
using HelpDeskTicketingApp.Adapters.DataAdapters;

namespace HelpDeskTicketingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ITicketAdapter _ticketAdapter;

        //Loggin/Timeline / Stats adapter.

        private ITimeLineAdapter _timeLineAdapter;
        private IStatisticAdapter _statisticAdapter;

        public AdminController()
        {
            _timeLineAdapter = new TimeLineDataAdapter();
            _statisticAdapter = new StatisticDataAdapter();
            _ticketAdapter = TicketAdapterFactory.GetTicketAdapter();
        }
        public AdminController(ITicketAdapter ticketAdapter, ITimeLineAdapter timeLineAdapter, IStatisticAdapter statisticAdapter)
        {
            _ticketAdapter = ticketAdapter;
            _timeLineAdapter = timeLineAdapter;
            _statisticAdapter = statisticAdapter;
        }


        //Stackoverflow Part

        [AllowAnonymous]
        public ActionResult Sof()
        {
            return View();
        }

        // userID can be for technician, admin, or general user - think about renaming
        public ActionResult Technician(string userId)
        {
            AdmUserIssuesViewModel model = _ticketAdapter.GetAllIssuesForSingleUser(userId);
            return View(model);
        }

        // Show all tickets
        public ActionResult Tickets(string status="")
        {
            AdmIssuesViewModel model = new AdmIssuesViewModel();
            if(status != "")
            {
                model = _ticketAdapter.GetAllIssues(status);
            }
            else
            {
                model = _ticketAdapter.GetAllIssues("All");
            }

            return View(model);
        }


        // Need to 1. Remove resolution, 2. Remove assignments, 3. Remove Ticket
        public ActionResult DeleteTicket(int id, string userId)
        {
            string result = _ticketAdapter.DeleteTicket(id, userId);
            if (result == "ok")
            {
                TimeLineHelper.TicketOperations(User.Identity.Name, TimeLineHelper.Operations.Delete, id);
            }
            else
            {
                TimeLineHelper.ErrorOperations(User.Identity.Name, TimeLineHelper.Operations.Delete, result, id);

            }
            // Came from ticket list return
            if (userId == "none")
            {
                return RedirectToAction("Tickets");
            }
            else // came from userpage return there
            {
                return RedirectToAction("Technician", new { userId = userId });
            }
        }


        public ActionResult UpdateTicket(int id)
        {
            AdmIssueViewModel model = _ticketAdapter.GetTicketById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateTicket(AdmIssueViewModel model, int id)
        {
            string result = _ticketAdapter.UpdateTicket(model, id);
            if (result == "ok")
            {
                TimeLineHelper.TicketOperations(User.Identity.Name, TimeLineHelper.Operations.Edit, id);
            }
            else
            {
                TimeLineHelper.ErrorOperations(User.Identity.Name, TimeLineHelper.Operations.Edit, result, id);

            }
            return RedirectToAction("UpdateTicket", new { id = id });
        }


        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }


        public ActionResult Users(string role)
        {
            AdmUsersListViewModel model = new AdmUsersListViewModel();

            model.Techs = new List<AdmUserViewModel>();
            model.Admins = new List<AdmUserViewModel>();
            model.Generals = new List<AdmUserViewModel>();


            if (role == "Technician")
            {
                model.Techs = _ticketAdapter.GetUsersByRole("Technician");
            }
            else if (role == "Admin")
            {
                model.Admins = _ticketAdapter.GetUsersByRole("Admin");
            }
            else if (role == "General")
            {
                model.Generals = _ticketAdapter.GetUsersByRole("General");
            }
            else // All, none passed on or invalid role
            {
                model.Techs = _ticketAdapter.GetUsersByRole("Technician");
                model.Admins = _ticketAdapter.GetUsersByRole("Admin");
                model.Generals = _ticketAdapter.GetUsersByRole("General");
            }

            return View(model);
        }

        public ActionResult Index()
        {
            AdmUsersViewModel models = _ticketAdapter.GetTechnicians();

            return View(models);
        }



        // Entry Point of our admin page
        public ActionResult Dashboard()
        {
            AdminDashboardVidewModel model = _timeLineAdapter.InitDashBoard();
            ViewBag.Page = "Dashboard";
            ViewBag.User = "Admin";
            ViewBag.OnlineUser = OnlineUserHelper.Instance.GetOnlineUsers();
            return View(model);
        }

        //Timeline
        public ActionResult TimeLine()
        {
            List<TimeLine> model = new List<TimeLine>();
            model = _timeLineAdapter.GetAll();
            return View(model.Take(10));
        }
        // Statistics
        public ActionResult Stats()
        {
            List<object> model = new List<object>();

            Dictionary<string, int> DonutChart = _statisticAdapter.GeneralIssueStatistic();
            List<GeneralTechStats> BarChart = _statisticAdapter.GeneralTechStatistic();
            List<GeneralIssueTypeStats> BarChart2 = _statisticAdapter.GeneralIssueTypesStatistic();
            model.Add(DonutChart);
            model.Add(BarChart);
            model.Add(BarChart2);
            return View(model);
        }


        //Those actions are giving ability of RealTime Comminications via
        // SignalR to our app
        //Like; Chat, Tracking online users etc

        public ActionResult Chat()
        {

            return View();
        }


        // If userId is null, this is being called with the intention of creating a tech
        public ActionResult AddUpdateTech(string userId)
        {
            AdmUserViewModel model = _ticketAdapter.GetUserById(userId);

            return View(model);
        }

        [HttpPost]
        // This is used to update/add technician, the only differenct is that if it is a create, the userName will be null
        public ActionResult AddUpdateTech(AdmUserViewModel model, string userId)
        {
            string result = _ticketAdapter.AddUpdateUser(model, userId);
            if (result == "NewOk")
            {
                TimeLineHelper.UserOperations(model.FirstName + model.LastName, User.Identity.Name, TimeLineHelper.Operations.Edit);
            }
            else if (result == "EditOk")
            {
                TimeLineHelper.UserOperations(model.FirstName + model.LastName, User.Identity.Name, TimeLineHelper.Operations.New);

            }
            else
            {
                TimeLineHelper.ErrorOperations(User.Identity.Name, TimeLineHelper.Operations.Edit, result);

            }

            return RedirectToAction("Users", new { role = model.Role });
        }


        public ActionResult DeleteTech(string userId)
        {
            string userRole = _ticketAdapter.GetUserRole(userId);
            string result = _ticketAdapter.DeleteUser(userId);
            if (result == "ok")
            {
                TimeLineHelper.UserOperations(userId, User.Identity.Name, TimeLineHelper.Operations.Delete);
            }
            else
            {
                TimeLineHelper.ErrorOperations(User.Identity.Name, TimeLineHelper.Operations.Edit, result);

            }

            if (userRole != null)
            {
                return RedirectToAction("Users", new { role = userRole });
            }
            else
            {
                return RedirectToAction("Users", new { role = "All" });
            }
        }


        [HttpPost]
        // See if tech already has assignment for the issue, if not create one.
        // If the issue is set to not assigned, then set it to assigned
        // Note - Can add multiple techs to same issue
        public ActionResult AssignTech(AdmIssueViewModel model, int issueId)
        {
            _ticketAdapter.AssignTech(model, issueId);

            return RedirectToAction("UpdateTicket", new { id = issueId });
        }

    }
}
