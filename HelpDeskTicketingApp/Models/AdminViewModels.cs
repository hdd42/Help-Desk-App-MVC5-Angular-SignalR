using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDeskTicketingApp.Models
{
   
    public class AdmUsersListViewModel
    {
        public List<AdmUserViewModel> Admins { get; set; }
        public List<AdmUserViewModel> Techs { get; set; }
        public List<AdmUserViewModel> Generals { get; set; }
    }
    public class AdmUsersViewModel
    {
        public List<AdmUserViewModel> Users { get; set; }
    }
    public class AdmUserViewModel
    {
        public string UserId { get; set; }
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Display(Name = "User Role")]
        public string Role { get; set; }
        public List<String> Roles { get; set; }
    }
    public class AdmUserIssuesViewModel
    {
        public List<AdmIssueViewModel> AssignedIssues { get; set; }
        public List<AdmIssueViewModel> ReportedIssues { get; set; }
        public string UserName { get; set; }
    }
    public class AdmIssuesViewModel
    {
        public List<AdmIssueViewModel> Issues { get; set; }
    }
    public class AdmIssueViewModel
    {
        public int IssueId { get; set; }
        public AdmIssueTypeViewModel IssueType { get; set; }
        [Display(Name = "Issue Types")]
        public List<AdmIssueTypeViewModel> IssueTypes { get; set; }
        public AdmUserViewModel User { get; set; }
        [Display(Name = "Date Reported")]
        public DateTime DateReported { get; set; }
        [Display(Name = "Date Resolved")]
        public DateTime? DateResolved { get; set; }
        public string IssueDesc { get; set; }
        public bool IsAssigned { get; set; }
        public AdmResolutionViewModel Resolution { get; set; }
        public string AssignToId { get; set; }
        [Display(Name = "Select User")]
        public List<AdmUserSelectionModel> Users { get; set; }
        public List<AdmUserSelectionModel> Techs { get; set; }
        public List<AdmUserSelectionModel> AssignedTechs { get; set; }
    }
    public class AdmUserSelectionModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string userEmail { get; set; }
    }
    public class AdmResolutionsViewModel
    {
        public List<AdmResolutionViewModel> Resolutions { get; set; }
    }
    public class AdmResolutionViewModel
    {
        public int ResolutionId { get; set; }
        [Display(Name = "Is Ticket Resolved")]
        public bool IsResolved { get; set; }
        [Display(Name = "Resolution Description")]
        public string ResolutionDesc { get; set; }
        public string Notes { get; set; }
    }

    public class AdmIssuesTypeViewModel
    {
        public List<AdmIssueTypeViewModel> IssueTypes { get; set; }
    }
    public class AdmIssueTypeViewModel
    {
        public int IssueTypeId { get; set; }
        public string IssueTypeDesc { get; set; }
    }

    public class AdmAssignmentsViewModel
    {
        public List<AdmAssignmentViewModel> Assignments { get; set; }
    }
    public class AdmAssignmentViewModel
    {
        public AdmUserViewModel TechnicianId { get; set; }
        public AdmIssueViewModel Issue { get; set; }
        public AdmResolutionViewModel Resolution { get; set; }
    }
}