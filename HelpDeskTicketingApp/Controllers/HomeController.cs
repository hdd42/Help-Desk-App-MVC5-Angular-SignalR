using HelpDeskTicketingApp.Data;
using HelpDeskTicketingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using HelpDeskTicketingApp.Data.Models;
using HelpDeskTicketingApp.Adapters;

namespace HelpDeskTicketingApp.Controllers
{
    public class HomeController : Controller
    {


         private ITicketAdapter _ticketAdapter;

      

        public HomeController()
        {
           
            _ticketAdapter = TicketAdapterFactory.GetTicketAdapter();
        }
        public HomeController(ITicketAdapter ticketAdapter)
        {
            _ticketAdapter = ticketAdapter;
           
        }



        string _username;
        string _email;


        [Authorize]
        public ActionResult Index()
        {

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            if (User.IsInRole("Technician"))
            {
                return RedirectToAction("Index", "Technician");
            }

            return RedirectToAction("ViewTickets");

            //kept the first page how it was for other use

        }



        [Authorize]
        public ActionResult ViewTickets()
        {

            var model = _ticketAdapter.GetAllUserTickets(User.Identity.Name);
 
            return View(model);
        }







        [Authorize]
        public ActionResult AddTicket()
        {

            CreateIssueViewModel model =_ticketAdapter.AddTicket();
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    model.Types = db.IssueTypes.Select(i => new LookUpModel
            //    {
            //        Display = i.IssueTypeDesc,
            //        Value = i.IssueTypeId
            //    }).ToList();
            //}


            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddTicket(CreateIssueViewModel model)
        {
           
          string result =  _ticketAdapter.AddTicket(model, User.Identity.GetUserId(), User.Identity.GetUserName());
          if (result.Contains("error"))
          {
              TimeLineHelper.ErrorOperations(User.Identity.Name, TimeLineHelper.Operations.New, result);
          }
          else
          {
              TimeLineHelper.TicketOperations(User.Identity.Name, TimeLineHelper.Operations.New, int.Parse(result));

          }
            return RedirectToAction("ViewTickets");
        }

        [Authorize]
        public ActionResult EditTicket(int id)
        {
            UpdateTicket model = _ticketAdapter.EditTicket(id, User.Identity.Name);
                
           
            if (model.IssueDescription == null)
            {
                return RedirectToAction("ViewTickets");
            }
            else { return View(model); }
            
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditTicket(UpdateTicket model)
        {
            string result = _ticketAdapter.EditTicket(model, User.Identity.GetUserId(), User.Identity.GetUserName());

            if (result.Contains("error"))
            {
                TimeLineHelper.ErrorOperations(User.Identity.Name, TimeLineHelper.Operations.Edit, result);
            }
            else
            {
                TimeLineHelper.TicketOperations(User.Identity.Name, TimeLineHelper.Operations.Edit, model.IssueId);

            }

        
            return RedirectToAction("ViewTickets");
        }





        [Authorize]
        public ActionResult ViewNotes(int id)
        {

            UpdateTicket model = _ticketAdapter.EditTicket(id, User.Identity.Name);


            if (model.IssueDescription == null)
            {
                return RedirectToAction("ViewTickets");
            }
            else { return View(model); }


        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Having trouble? Send us a message.";

            ContactIssueViewModel model = new ContactIssueViewModel();

            return View(model);
          }

        [HttpPost]
        public ActionResult Contact(ContactIssueViewModel model)
        {
            ViewBag.Message = "Thanks, we got your message.";

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                
                //create the res before the issue to stuff the id into the issue when it is made
                Resolution resolution = new Resolution()
                {
                    IsResolved = false
                };

                resolution = db.Resolutions.Add(resolution);
                db.SaveChanges();
                int resolutionId = resolution.ResolutionId;

                //create the issue
                db.Issues.Add(new Issue
                {

                    DateReported = DateTime.Now,
                    ResolutionId = resolutionId,
                    IssueDesc = model.IssueDescription,
                    UserId = User.Identity.GetUserId(),
                    IssueTypeId = 1
                    
                });

                _username = User.Identity.Name;
                _email = User.Identity.GetUserId();
                EmailManager.SendEmailNewTicket(_email, _username, model.IssueDescription);

                TimeLineLog tl = new TimeLineLog()
                {
                    Description = "New issue created! Issue on: " + DateTime.Now + " User : " + User.Identity.Name + " , Short Desc: " + model.IssueDescription,
                    EntryType = "new",
                    TimeHappened = DateTime.Now,
                    Title = "New issue created!",
                    UserName = User.Identity.Name
                };
                db.TimeLineLogs.Add(tl);


                db.SaveChanges();
            }
            
            return View();
        }
    }
}