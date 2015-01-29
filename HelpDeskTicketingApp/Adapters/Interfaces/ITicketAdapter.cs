using HelpDeskTicketingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTicketingApp.Adapters
{
    public interface ITicketAdapter
    {
        AdmUserIssuesViewModel GetAllIssuesForSingleUser(string userId);
        AdmIssuesViewModel GetAllIssues(string status);
        string DeleteTicket(int id, string userId);
        AdmIssueViewModel GetTicketById(int id);
        AdmUsersViewModel GetTechnicians();
        string UpdateTicket(AdmIssueViewModel model, int id);
        List<AdmUserViewModel> GetUsersByRole(string role);
        AdmUserViewModel GetUserById(string userId);
        string AddUpdateUser(AdmUserViewModel model, string userId);
        string DeleteUser(string userId);
        void AssignTech(AdmIssueViewModel model, int issueId);
        //added by jim. needs to be fixed just wanted it done for now
        HomeIndexViewModel GetAllUserTickets(string userName);
        string AddTicket(CreateIssueViewModel model, string userId, string userName);
        string EditTicket(UpdateTicket model, string userId, string userName);
        UpdateTicket EditTicket(int id, string name);
        CreateIssueViewModel AddTicket();

        string GetUserRole(string userId);
        List<AdmIssueViewModel> GetUserAssignedIssues(string userId);
        List<AdmUserSelectionModel> HydrateAssignedTechs(int issueId);
        AdmResolutionViewModel GetResolutionById(int id);
        string UpdateResolution(AdmResolutionViewModel resolution);
    }
}
