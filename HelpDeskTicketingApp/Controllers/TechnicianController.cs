using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc; 
using System.Web.Security;
using HelpDeskTicketingApp.Data;
using HelpDeskTicketingApp.Data.Models;
using HelpDeskTicketingApp.Models;
using Microsoft.AspNet.Identity;
using HelpDeskTicketingApp.Adapters;

namespace HelpDeskTicketingApp.Controllers
{
    [Authorize]
    public class TechnicianController : Controller
    {
        private ITicketAdapter _ticketAdapter;

        public TechnicianController()
        {
            _ticketAdapter = TicketAdapterFactory.GetTicketAdapter();
        }

        public TechnicianController(ITicketAdapter ticketAdapter)
        {
            _ticketAdapter = ticketAdapter;
        }

        // GET: Technician Assignments
        [Authorize(Roles = "Technician, Admin")]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            List<AdmIssueViewModel> assignments = _ticketAdapter.GetUserAssignedIssues(userId);
 
            return View(assignments);
        }


        //GET: Technician/AssignmentDetails/id
        public ActionResult AssignmentDetails(int id)
        {

            List<AdmUserSelectionModel> techs = _ticketAdapter.HydrateAssignedTechs(id);
 
            return View(techs);
        }

        //GET: Technician/IssueDetails/id
        public ActionResult IssueDetails(int id)
        {
            AdmIssueViewModel issue = _ticketAdapter.GetTicketById(id);
            return View(issue);
        }

         //GET: Technician/Resolution/id (Edit Resolution)
        public ActionResult Resolution(int id)
        {
            AdmResolutionViewModel resolution = _ticketAdapter.GetResolutionById(id);
            
            return View(resolution);
        }

         // POST: Technician/Resolution/id (Edit Resolution)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Resolution(AdmResolutionViewModel resolution)
        {
            if (ModelState.IsValid)
            {

                string result = _ticketAdapter.UpdateResolution(resolution);

                return RedirectToAction("Index");
            }

            return View(resolution);
        }

    }
}
